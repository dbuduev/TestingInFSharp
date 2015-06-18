module CustomFsCheck

open Xunit
open FsCheck
open FsCheck.Xunit

type Color = Red | Green | Blue

let arbitraryColor =
      { new Arbitrary<Color>() with

        member this.Generator = 
            Gen.elements [ Red; Blue ]
      }

type Generators =
    static member Color() = arbitraryColor

[<Arbitrary(typeof<Generators>)>]
module Tests =

    [<Property>]
    let ``No greens are generated`` color =
        color <> Green

// Combinators for Gen<'a>
// https://fsharp.github.io/FsCheck/TestData.html

// elements : seq<'a> -> Gen<'a>
let elementsGen = Gen.elements ['a'..'z'] 

// choose : (int, int) -> Gen<int>
let fiveToTen = Gen.choose (5, 10)
let twentyToThirty = Gen.choose (20, 30)

// oneof : seq<Gen<'a>> -> Gen<'a>
let numbers =
    Gen.oneof [ 
        fiveToTen 
        twentyToThirty
    ]

// listOf : Gen<'a> -> Gen<List<'a>>
let listOfFivesToTens = Gen.listOf fiveToTen

// to combine multiple "Gen"s together
// use the 'gen' computation expression
let combining = gen {
        let! left = fiveToTen
        let! right = twentyToThirty
        return (left, right)
    }


type Animal = { legs : int ; height : int }

let animalGenerator = 
      { new Arbitrary<Animal>() with

          override x.Generator = gen {
                let! l = Gen.choose (2, 4)
                let! h = Gen.choose (10, 350)
                                
                return { legs = l; height = h }
            }

          override x.Shrinker animal = seq {
                for l in Arb.shrink animal.legs do
                    for h in Arb.shrink animal.height do
                        yield { legs = l; height = h }
            }
      }

type Generators with
    static member Animal() = animalGenerator


// Generic types:
type Container<'a> = Item of 'a

type Generators with
  static member Container() =
      { new Arbitrary<Container<'a>>() with

          override x.Generator = gen {

                let! x = Arb.generate<'a>
                return (Item x)
            }
      }
