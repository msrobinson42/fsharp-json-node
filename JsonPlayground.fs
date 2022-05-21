module Playground

open Domain
open FSharpPlus
open FSharpPlus.Lens

let sampleRecords: MyRecord list =
    [ { Key = "A"; Value = "Another Value" }
      { Key = "C"; Value = "Another Value" }
      { Key = "E"; Value = "Another Value" }
      { Key = "F"; Value = "Another Value" } ]

let mergeLists matcher list2 list1 =
    let keys: string list = List.map matcher list2

    [ for item in list1 do
          if List.contains item.Key keys then
              yield List.find (fun x -> matcher x = item.Key) list2
          else
              yield item ]

let app args =
    let filePath = Array.tryHead args
    let configKey = "Things"
    let fileMaybe = tryReadFile filePath

    let getKey = view MyRecord._key
    let merge = mergeLists getKey sampleRecords

    let rootNode = Option.map parseNode fileMaybe

    let resultNode =
        fileMaybe
        |> Option.bind (tryDeserialize configKey)
        |> Option.map merge
        |> Option.map trySerialize
        |> Option.apply (trySetNode configKey rootNode)

    (resultNode, filePath)
    ||> (lift2 overwriteJson)
    |> ignore

    0
