namespace EstimateLiabilitiesLife


open System
open Insurance


module Reserving =

    type cashflow =
        { month: int; t: float; benefit: float }

    type cashflows = cashflow list


    let private benefits index benefit z_month end_month =
        index
        |> List.map (fun t -> if t >= z_month && t <= end_month then benefit else 0.0)


    let private projectCashflows
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
        let x = Time.months birthdate valuedate
        let z = 12 * vestingAge - x
        let end_month = z + 12 * T - 1

        let time =
            { Time.startDate = valuedate
              Time.months = end_month }

        let index = time.Index // months
        let t = time.Years

        let cf =
            let gb = benefits index monthBenefit z end_month

            match table with
            | APG -> gb
            | AP -> Mortality.survivalProbs birthdate sex age t |> List.map2 (fun g l -> g * l) gb

        // Returns a list of cashflows
        List.map
            (fun i ->
                { month = i
                  t = t.[i]
                  benefit = cf.[i] })
            index

    let reserve (valuedate: DateTime) (contract: Contract) (discount: float list) =
        projectCashflows valuedate contract
        |> List.map (fun c -> c.benefit)
        |> fun gb -> List.map2 (fun d c -> d * c) gb discount[.. gb.Length - 1]
        |> List.sum

    let technicalProvision (valuedate: DateTime) (contract: Contract) =
        let discount = Rate.discountFactors

        projectCashflows valuedate contract
        |> List.map (fun c -> c.benefit)
        |> fun gb -> List.map2 (fun d c -> d * c) gb discount[.. gb.Length - 1]
        |> List.sum
