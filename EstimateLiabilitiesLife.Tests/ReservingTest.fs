namespace EstimateLiabilitiesLife.Tests

open NUnit.Framework
open System
open EstimateLiabilitiesLife

module TimeTests =

    [<SetUp>]
    let Setup () = ()

    [<Test>]
    let tidur_1_year () =
        let startDate = DateTime(2015, 1, 1)
        let endDate = DateTime(2016, 1, 1)
        let actual = Time.years startDate endDate
        Assert.That(actual, Is.EqualTo(1.0).Within(0.01))


    [<Test>]
    let ``tidur 1.5 years`` () =
        let startDate = DateTime(2015, 1, 1)
        let endDate = DateTime(2016, 7, 1)
        let actual = Time.years startDate endDate
        Assert.That(actual, Is.EqualTo(1.5).Within(0.01))

    [<Test>]
    let ``Dates created`` () =
        let months = 3
        let startDate = DateTime(2015, 12, 1)

        let time =
            { Time.startDate = startDate
              Time.months = months }

        let dates = time.Dates

        let expected =
            [ DateTime(2015, 12, 1)
              DateTime(2016, 1, 1)
              DateTime(2016, 2, 1)
              DateTime(2016, 3, 1) ]

        Assert.That(dates, Is.EqualTo(expected))

// [<Test>]
// let ``Years created`` () =
//     let months = 3
//     let startDate = DateTime(2015, 12, 1)
//
//     let time =
//         { Time.startDate = startDate
//           Time.months = months }
//
//     let years = time.Years
//
//     Assert.That(years.Head, Is.EqualTo(0.0))
//     Assert.That(years.Tail.Head, Is.EqualTo(0.08487).Within(0.001))
//     Assert.That(years.Tail.Tail.Head, Is.EqualTo(0.16974).Within(0.001))
//     Assert.That(years.Tail.Tail.Tail.Head, Is.EqualTo(0.249144).Within(0.001))
