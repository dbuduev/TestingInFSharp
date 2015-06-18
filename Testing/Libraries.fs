module Libraries

(* Two libraries for asserting *)

open Xunit

// Using FsUnit lets you write easily-read tests

open FsUnit.Xunit

[<Fact>]
let ``Our simple assert`` () = 
    "F" + "#" |> should equal "F#"


[<Fact>]
let ``Try to find item in list`` () =
    [ 1 ; 2 ; 3 ] |> List.tryFind (fun x -> x > 10) |> should be Null


[<Fact>]
let ``Contains check`` () =
    [1..10] |> should contain 4

// See more: https://github.com/fsprojects/FsUnit







// Unquote lets you write assertions directly

open Swensen.Unquote

[<Fact>]
let ``Once again`` () =
   test <@ "F" + "#" <> "F#" @>


[<Fact>]
let ``Another example`` () =
    test <@ [1..10] |> List.tryFind (fun x -> x > 11) |> Option.isNone @>

// See more: https://code.google.com/p/unquote/

