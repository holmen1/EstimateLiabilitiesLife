namespace EstimateLiabilitiesLife

open System


module Mortality =
    type Sex =
        | F
        | M

    type Generation =
        | Before1940
        | G1940
        | G1950
        | G1960
        | G1970
        | G1980
        | After1980

    let makeham06 (birth: DateTime) (sex: Sex) =
        (* Contract -> float * float * float
        mu = a + b e^cx *)
        let generation =
            match birth.Year / 10 with
            | i when i < 194 -> Before1940
            | 194 -> G1940
            | 195 -> G1950
            | 196 -> G1960
            | 197 -> G1970
            | 198 -> G1980
            | _ -> After1980

        match sex, generation with
        | F, G1940 -> (0.0014, 0.000001129, 0.127)
        | F, G1950 -> (0.0011, 0.000000879, 0.129)
        | F, G1960 -> (0.0011, 0.000000411, 0.137)
        | F, G1970 -> (0.0011, 0.000000129, 0.150)
        | F, G1980 -> (0.001, 0.000000092, 0.154)
        | F, _ -> (0.001, 0.000000092, 0.154)
        | M, Before1940 -> (0.0017, 0.000003094, 0.120)
        | M, G1940 -> (0.0017, 0.000003094, 0.120)
        | M, G1950 -> (0.0015, 0.000001159, 0.130)
        | M, G1960 -> (0.0013, 0.000000457, 0.140)
        | M, G1970 -> (0.0011, 0.000000147, 0.152)
        | M, G1980 -> (0.001, 0.000000051, 0.163)
        | M, _ -> (0.001, 0.000000051, 0.163)

    let private mu a b c x = a + b * Math.Exp(c * x)


    /// survival probability at age x, l(x)
    let survival (a, b, c) (x: float) =
        let integral = Simpson.compositeSimpson 1000
        let mortality = mu a b c
        Math.Exp(-integral 0.0 x mortality)

    // survival probabilites at age x + t, list of l(x+t)/l(x)
    let survivalProbs (birth: DateTime) (sex: Sex) (age: float) (t: float list) =
        let l = makeham06 birth sex |> survival
        List.map (fun t' -> l (age + t') / l age) t
