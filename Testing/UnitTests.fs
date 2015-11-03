namespace TestingInFSharp

// You can use the testing frameworks you're familiar with


open NUnit.Framework

(* NUnit 'classy' way *)
[<TestFixture>]
type NUnitClass() =

    [<Test>]
    member this.SimpleTest() =
        Assert.AreEqual("F#", "F" + "#")
    





(* NUnit *)
[<NUnit.Framework.TestFixture>]
module NUnit =
    open NUnit.Framework

    [<Test>]
    let simpleTest() =
        Assert.AreEqual("F#", "F" + "#")






(* XUnit *)
module Xunit =
    open Xunit

    [<Fact>]
    let simpleTest() =
        Assert.Equal<string>("F#", "F" + "#")







// Or ones designed for F#

module Fuchu =
    open Fuchu

    let simpleTest = 
        testCase "String concat" 
            (fun _ -> Assert.Equal("F# should equal F#!", "F#", "F" + "#"))

    [<Tests>]
    let allTests = 
        testList "All tests" [
            simpleTest
            testList "Other tests" []
            ]

    // to make an .exe:
    //[<EntryPoint>]
    //let main args = defaultMainThisAssembly args
