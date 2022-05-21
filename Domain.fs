[<AutoOpen>]
module Domain

open FSharpPlus
open FSharpPlus.Lens

type MyRecord = { Key: string; Value: string }

module MyRecord =
    let inline _key f mr =
        f mr.Key <&> fun key -> { mr with Key = key }
