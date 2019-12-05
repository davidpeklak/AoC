﻿module Day05

open System
open System.IO
open Xunit
open Day02 // initial impletementation

let CodeIn = 3
let CodeOut = 4

let In : int = 
    printf "Input:"
    Console.ReadLine() |> int

let Out arg = printf "%d" arg 

let GetOpCode (instruction:int) : int = instruction % 100 // last 2 digits

[<Theory>]
[<InlineData(1002, 2)>]
let TestGetOperation instruction expectedOperation = 
    Assert.Equal( expectedOperation, GetOpCode instruction )

type Operation = 
    | End
    | Add of a: int * b: int * r: int
    | Mul of a: int * b: int * r: int
    | In of a: int
    | Out of a: int

let OperationLength (opcode:int) =
    match opcode with 
    | CodeEnd -> 1
    | CodeAdd -> 4
    | CodeMul -> 4
    | CodeIn  -> 2
    | CodeOut -> 2

let GetParametersCount (opcode:int) = ( OperationLength opcode ) - 1 // minus first position that is opcode and modes

type ParameterMode =
    | PositionMode
    | ImmediateMode

let GetMode (digit:char) = 
    match digit with
    | '0' -> PositionMode
    | '1' -> ImmediateMode

let GetModes (instruction:int) = 
    let modes = instruction / 100
    let s = seq { yield! modes.ToString().ToCharArray() |> Array.rev; '0'; '0'; '0' }
    s |> Seq.map (fun digit -> GetMode (digit) )        

let GetValue (m:ParameterMode) (parameter:int) (code:array<int>) =
    match m with 
    | PositionMode -> code.[parameter]
    | ImmediateMode -> parameter

let ReadOp pos (code: array<int>) : Operation = 
    let instruction = code.[pos]
    let opcode = GetOpCode instruction
    let modes = GetModes instruction |> Seq.take (GetParametersCount opcode ) |> Seq.toArray
    match opcode with
    | CodeEnd -> End
    | CodeAdd -> Add(code.[pos+1], code.[pos+2], code.[pos+3])
    | CodeMul -> Mul(code.[pos+1], code.[pos+2], code.[pos+3])
    | CodeIn -> In(code.[pos+1])
    | CodeOut -> Out(code.[pos+1])