namespace EstimateLiabilitiesLife

open System
open System.Collections.Generic

module Time =

    /// Calculates the number of years between two dates
    let years (startDate: DateTime) (endDate: DateTime) : float =
        (endDate - startDate).TotalDays / 365.25

    let monthsBetweenDates (startDate: DateTime) (endDate: DateTime) : int =
        12 * (endDate.Year - startDate.Year) + endDate.Month - startDate.Month

    type T =
        { startDate: DateTime
          months: int }

        member this.Months = [ 0 .. this.months ]
        member this.Dates = this.Months |> List.map this.startDate.AddMonths
        member this.Years = this.Dates |> List.map (fun d -> years this.Dates.Head d)

module Simpson =
    // Simpson's rule for numerical integration
    let compositeSimpson n a b f =
        if n % 2 <> 0 then
            invalidArg "n" "must be even"

        let h = (b - a) / float n
        let sumOdd = seq { 1..2 .. n - 1 } |> Seq.sumBy (fun i -> f (a + float i * h))
        let sumEven = seq { 2..2 .. n - 2 } |> Seq.sumBy (fun i -> f (a + float i * h))

        h / 3.0 * (f a + 4.0 * sumOdd + 2.0 * sumEven + f b)

