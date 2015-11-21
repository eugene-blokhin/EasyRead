namespace EnglishEasyRead.WordNet

open EnglishEasyRead.WordNet.WordNetProvider

type WordNetClient(databasePath) = 
    
    let morphologyExceptions = 
        lazy (Seq.concat [ loadExceptions databasePath Noun
                           loadExceptions databasePath Verb
                           loadExceptions databasePath Adverb
                           loadExceptions databasePath Adjective ]
              |> Seq.map (fun e -> (e.InflectedForm, e.Lemmas))
              |> Map)
    
    let toIndexDictionary intexRecords = 
        intexRecords
        |> Seq.map (fun ir -> (ir.Lemma, ir))
        |> Map
    
    let nounIndex = lazy (loadIndex databasePath Noun |> toIndexDictionary)
    let verbIndex = lazy (loadIndex databasePath Verb |> toIndexDictionary)
    let adjectiveIndex = lazy (loadIndex databasePath Adjective |> toIndexDictionary)
    let adverbIndex = lazy (loadIndex databasePath Adverb |> toIndexDictionary)
    
    let indexSeek cyntacticCategory lemma = 
        let index = 
            match cyntacticCategory with
            | Noun -> nounIndex.Value
            | Verb -> verbIndex.Value
            | Adjective -> adjectiveIndex.Value
            | Adverb -> adverbIndex.Value
        index.TryFind lemma
    
    member this.GetBaseForms word = 
        match Map.tryFind word morphologyExceptions.Value with
        | Some(baseForms) -> baseForms
        | None -> 
            let (|GetStem|_|) (ending : string) (replacements : string seq) (str : string) = 
                if str.EndsWith(ending) then 
                    let wordWithoutEnding = str.Substring(0, str.Length - ending.Length)
                    seq { for replacement in replacements -> (wordWithoutEnding + replacement) }
                    |> Some
                else None
            match word with
            | GetStem "ses" [ "s" ] baseForms -> baseForms
            | GetStem "xes" [ "x" ] baseForms -> baseForms
            | GetStem "zes" [ "z" ] baseForms -> baseForms
            | GetStem "ches" [ "ch" ] baseForms -> baseForms
            | GetStem "shes" [ "sh" ] baseForms -> baseForms
            | GetStem "men" [ "man" ] baseForms -> baseForms
            | GetStem "ies" [ "y" ] baseForms -> baseForms
            | GetStem "es" [ "e"; "" ] baseForms -> baseForms
            | GetStem "ed" [ "e"; "" ] baseForms -> baseForms
            | GetStem "ing" [ "e"; "" ] baseForms -> baseForms
            | GetStem "er" [ ""; "e" ] baseForms -> baseForms
            | GetStem "est" [ ""; "e" ] baseForms -> baseForms
            | GetStem "s" [ "" ] baseForms -> baseForms
            | _ -> seq { yield word }
    
    member this.IndexSeek lemma = 
        seq { 
            yield indexSeek Noun lemma
            yield indexSeek Verb lemma
            yield indexSeek Adverb lemma
            yield indexSeek Adjective lemma
        }
        |> Seq.choose id
        |> Array.ofSeq
