namespace EstimateLiabilitiesLife

open System
open EstimateLiabilitiesLife.Mortality


module Insurance =

    type Table =
        | APG
        | AP

    type Contract =
        { contractNo: int
          birthDate: DateTime
          sex: Sex
          z: int
          guarantee: float
          payPeriod: int
          table: Table }
