namespace EnglishEasyRead.WordNet

open System.Text.RegularExpressions

type IMorpher = 
    abstract GetPossibleLemmas : inflection:string -> string array

type Morpher(exceptions : ExceptionFileRecord seq) = 
    
    let morphologyExceptions = 
        lazy (exceptions
              |> Seq.map (fun ex -> (ex.InflectedForm, ex.Lemmas))
              |> Map)
    
    let getLemmasByDetachment (inflection : string) = 
        let (|GetLemmas|_|) (ending : string) (replacements : string seq) (str : string) = 
            if str.EndsWith(ending) then 
                replacements
                |> Seq.map (fun replacement -> Regex.Replace(str, ending + "$", replacement))
                |> Array.ofSeq
                |> Some
            else None
        match inflection with
        | GetLemmas "ses" [ "s" ] baseForms -> baseForms
        | GetLemmas "xes" [ "x" ] baseForms -> baseForms
        | GetLemmas "zes" [ "z" ] baseForms -> baseForms
        | GetLemmas "ches" [ "ch" ] baseForms -> baseForms
        | GetLemmas "shes" [ "sh" ] baseForms -> baseForms
        | GetLemmas "men" [ "man" ] baseForms -> baseForms
        | GetLemmas "ies" [ "y" ] baseForms -> baseForms
        | GetLemmas "es" [ "e"; "" ] baseForms -> baseForms
        | GetLemmas "ed" [ "e"; "" ] baseForms -> baseForms
        | GetLemmas "ing" [ "e"; "" ] baseForms -> baseForms
        | GetLemmas "er" [ "e"; "" ] baseForms -> baseForms
        | GetLemmas "est" [ "e"; "" ] baseForms -> baseForms
        | GetLemmas "s" [ "" ] baseForms -> baseForms
        | _ -> [| inflection |]
    
    new(databasePath : string) = 
        let exceptions = 
            [ Noun; Verb; Adverb; Adjective ]
            |> Seq.map (fun c -> WordNetProvider.loadExceptions databasePath c)
            |> Seq.concat
            |> Array.ofSeq
        Morpher(exceptions)
    
    interface IMorpher with
        member this.GetPossibleLemmas inflection = 
            match morphologyExceptions.Value.TryFind inflection with
            | Some(lemmas) -> lemmas
            | _ -> getLemmasByDetachment inflection
    
    member this.GetPossibleLemmas inflection = (this :> IMorpher).GetPossibleLemmas inflection
