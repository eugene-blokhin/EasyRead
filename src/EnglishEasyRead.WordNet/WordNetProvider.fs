namespace EnglishEasyRead.WordNet

open System
open System.IO
open System.Text.RegularExpressions

module WordNetProvider = 
    type SyntacticCategory = 
        | Noun
        | Verb
        | Adverb
        | Adjective
    
    let (|SyntacticCategory|_|) = 
        function 
        | "n" -> Some(Noun)
        | "v" -> Some(Verb)
        | "a" -> Some(Adjective)
        | "r" -> Some(Adverb)
        | _ -> None
    
    // Exceptions
    type MorphologyException = 
        { InflectedForm : string
          BaseForms : string list }
    
    [<CompiledNameAttribute("LoadExceptions")>]
    let loadExceptions databasePath syntacticCategory = 
        let filename = 
            match syntacticCategory with
            | Noun -> "noun.exc"
            | Verb -> "verb.exc"
            | Adverb -> "adv.exc"
            | Adjective -> "adj.exc"
        
        let parseLine (parts : string array) = 
            { InflectedForm = parts.[0]
              BaseForms = 
                  parts
                  |> Array.skip 1
                  |> List.ofArray }
        
        File.ReadAllLines(Path.Combine(databasePath, filename))
        |> Array.map (fun line -> line.Split(' '))
        |> Array.filter (fun parts -> parts.Length > 1)
        |> Array.map parseLine
    
    // Indexes
    type IndexRecord = 
        { Lemma : string
          Pos : SyntacticCategory
          SynsetOffsetList : int seq }
    
    let loadIndex databasePath syntacticCategory = 
        let filename = 
            match syntacticCategory with
            | Noun -> "index.noun"
            | Verb -> "index.verb"
            | Adverb -> "index.adv"
            | Adjective -> "index.adj"
        
        let parseLine (parts : string array) = 
            { Lemma = parts.[0].Replace('_', ' ')
              Pos = 
                  match parts.[1] with
                  | SyntacticCategory category -> category
                  | _ -> raise (new ArgumentOutOfRangeException())
              SynsetOffsetList = 
                  parts
                  |> Array.skip (6 + Int32.Parse(parts.[3]))
                  |> Array.map Int32.Parse }
        
        File.ReadAllLines(Path.Combine(databasePath, filename))
        |> Array.filter (fun line -> not (line.StartsWith(" ")))
        |> Array.map (fun line -> line.Split([| ' ' |], StringSplitOptions.RemoveEmptyEntries))
        |> Array.filter (fun parts -> parts.Length >= 8)
        |> Array.map parseLine
