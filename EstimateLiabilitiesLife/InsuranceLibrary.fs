namespace EstimateLiabilitiesLife

open System


module Insurance =

    type Table =
        | APG
        | AP

    type Contract =
        { contractNo: int
          birthDate: DateTime
          sex: Mortality.Sex
          z: int
          guarantee: float
          payPeriod: int
          table: Table }
