module Features

open Xunit

[<Fact>]
let FSharpHasACleanSyntax() =
    let x = 1
    let y = 2

    Assert.Equal(3, x + y)










[<Fact>]
let ``I can describe my tests very easily`` () = 
    Assert.Empty []













[<Fact>]
let ``F# is very declarative`` () =
    [1..5]
        |> List.map (fun x -> x * 10)
        |> List.sum
        |> fun result -> Assert.InRange(result, 50, 1000000)





// Object expressions make test objects really easy

type IApply =
    abstract member Apply : int -> int

// function to test
let applyTwice (a : IApply) (value : int) =
    a.Apply (a.Apply value)




[<Fact>]
let ``A test with fake object`` () =

    let myApply = { new IApply with
        member this.Apply x = x + 1
        }

    // check that applyTwice calls Apply twice
    Assert.Equal(2, applyTwice myApply 0)

