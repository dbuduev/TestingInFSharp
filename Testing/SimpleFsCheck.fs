module SimpleFsCheck

open Xunit
open FsCheck
open FsCheck.Xunit


// Simple:

[<Property>]
let ``Doubles`` (x : int) =
    x + x = x * 2



[<Property>]
let ``Triples`` (x : int) =
    x + x = x * 3

// you might already be doing randomization?



// Generics work:

[<Property>]
let ``Reversing list doesn't change the length`` (l : List<int>) =
    List.length l = List.length (List.rev l)







[<Property>]
let ``Reversed list is the same as the list`` (l : List<int>) =
    l = List.rev l







// Even functions:

[<Property>]
let ``Mapping twice is the same as mapping composition`` (l : List<int>) (f : int -> int) (g : int -> int) =

    let mappedTwice = l |> List.map f |> List.map g
    let mappedComposition = l |> List.map (f >> g)

    mappedTwice = mappedComposition


[<Property>]
let ``Mapping twice is the same as mapping composition (better)`` (l : List<int>) (F(_, f)) (F(_, g)) =

    let mappedTwice = l |> List.map f |> List.map g
    let mappedComposition = l |> List.map (f >> g)

    mappedTwice = mappedComposition





// Conditionals:


[<Property>]
let ``If two numbers are equal the difference is zero (bad)`` (x : int) (y : int) =
    if x = y
    then Assert.Equal(0, x - y)






// use ==>
[<Property>]
let ``If two numbers are equal the difference is zero (good)`` (x : int) (y : int) =
    x = y ==> ((x - y) = 0)








[<Property>]
let ``Exclude div by zero (bad)`` (x : int) (y : int) =
    (x > 0 && y <> 0) ==> ((x / y) <= x)

    




[<Property>]
let ``Exclude div by zero (good)`` (x : int) (y : int) =
    (x > 0 && y <> 0) ==> lazy ((x / y) <= x)






// Using your own types:


type Animal = { legs : int ; name : string }

open Newtonsoft.Json

let roundtrip (x : 'a) =
    let json = JsonConvert.SerializeObject x
    JsonConvert.DeserializeObject<'a> json

[<Property>]
let ``Animal can be roundtripped from JSON`` (l : int) (n : string) =
    let animal = { legs = l ; name = n }

    let fromJson = roundtrip animal

    Assert.Equal(animal, fromJson)


// Better:

[<Property>]
let ``Animal can be roundtripped from JSON (better)`` (animal : Animal) =
    // printfn "%A" animal
    let fromJson = roundtrip animal

    Assert.Equal(animal, fromJson)


// Use C# types:
open Testable
[<Property>]
let ``Test the incrementer`` (incrementer : Incrementer) (value : int) =
    incrementer.Increment(value) > value
        