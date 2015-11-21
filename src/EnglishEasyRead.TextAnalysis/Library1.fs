namespace EnglishEasyRead.TextAnalysis

module Tokenizer = 
    open System.Text.RegularExpressions
    
    let tokenize text = 
        Regex.Matches(text, @"([a-z]+)", RegexOptions.IgnoreCase)
        |> Seq.cast<Match>
        |> Seq.map (fun m -> 
               if (m.Success) then Some(m.Groups.[1].Value)
               else None)
        |> Seq.filter (fun w -> w.IsSome)
        |> Seq.map (fun w -> w.Value.ToLower())
        |> Array.ofSeq

