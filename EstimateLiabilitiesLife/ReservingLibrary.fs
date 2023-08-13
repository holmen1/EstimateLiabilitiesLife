namespace EstimateLiabilitiesLife


open System
open Insurance


module Reserving =
    type cashflow = { month: int; benefit: float }
    type cashflows = cashflow list

    let private benefits benefit z_month end_month : cashflows =
        let z_payout = max z_month 0
        [ z_payout..end_month ] |> List.map (fun t -> { month = t; benefit = benefit })


    let private mortalityDiscountedBenefits birthdate sex age (cf: cashflows) : cashflows =
        let months = List.map (fun c -> c.month) cf
        let benefits = List.map (fun c -> c.benefit) cf

        List.map (fun m -> float m / 12.0) months
        |> Mortality.survivalProbs birthdate sex age
        |> List.map3 (fun m b l -> { month = m; benefit = l * b }) months benefits

    let projectCashflows
        (valuedate: DateTime)
        { Insurance.birthDate = birthdate
          Insurance.sex = sex
          Insurance.z = vestingAge
          Insurance.guarantee = monthBenefit
          Insurance.payPeriod = T
          Insurance.table = table }
        : cashflows =

        let age = Time.years birthdate valuedate

        if age < 0.0 || T < 0 || vestingAge < 0 then
            invalidArg "contract" "Invalid contract"


        // Ages expressed in months
        let max_age = 120 * 12
        let x = Time.monthsBetweenDates birthdate valuedate
        let z = 12 * vestingAge - x
        let end_month = min (z + 12 * T - 1) max_age


        let cf =
            let gb = benefits monthBenefit z end_month

            match table with
            | APG -> gb
            | AP -> gb |> mortalityDiscountedBenefits birthdate sex age

        cf
