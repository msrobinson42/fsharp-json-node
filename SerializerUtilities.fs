[<AutoOpen>]
module SerializerUtilities

open Domain
open System.IO
open System.Text.Json.Nodes
open System.Text.Json
open FSharpPlus

let private unsafeDo f x =
    try
        Some(f x)
    with
    | _ -> None

let private parseNode (text: string) = JsonNode.Parse text

let private getNode (key: string) (node: JsonNode) = Option.ofObj node.[key]

let private serialize obj = JsonSerializer.Serialize obj

let private deserialize (node: JsonNode) =
    JsonSerializer.Deserialize<MyRecord list> node

let private setNode (key: string) (root: JsonNode) (node: JsonNode) =
    root.[key] <- node
    root

let tryReadFile = Option.bind (unsafeDo File.ReadAllText)

let trySerialize<'a> = serialize >> parseNode

let tryDeserialize key =
    parseNode
    >> (getNode key)
    >> (Option.map deserialize)

let overwriteJson (node: JsonNode) path =
    let options = new JsonSerializerOptions()
    options.WriteIndented <- true

    (path, (node.ToJsonString options))
    |> File.WriteAllText
    |> ignore

let trySetNode text rootNode = lift2 setNode (Some text) rootNode
