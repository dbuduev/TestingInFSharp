module Model2

// som

type Expr = 
    | Value of int
    | Plus of Expr * Expr
    | Minus of Expr * Expr

let rec eval = function
    | Value x -> x
    | Plus (e1, e2) -> eval e1 + eval e2
    | Minus (e1, e2) -> eval e1 - eval e2

let rec stringify = function
    | Value x -> x.ToString()
    | Plus (e1, e2) -> "(" + stringify e1 + "+" + stringify e2 + ")"
    | Minus (e1, e2) -> "(" + stringify e1 + "-" + stringify e2 + ")"

open System
open FsCheck.Xunit

let nativeFunction (s : string) : int = raise (NotImplementedException())

[<Property>]
let ``Check parser`` (expr : Expr) =
    printfn "%s" (stringify expr)
    //(eval expr) = nativeFunction (stringify expr)