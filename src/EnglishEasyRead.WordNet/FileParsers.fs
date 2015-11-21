namespace EnglishEasyRead.WordNet

module internal ParsersHelper = 
    open System
    open System.IO
    
    let getFields (str : string) = str.Split([| ' ' |], StringSplitOptions.RemoveEmptyEntries)
    let isCommentsLine (str : string) = str.StartsWith("  ")
    
    let (|SyntacticCategory|_|) = 
        function 
        | "n" -> Some(Noun)
        | "v" -> Some(Verb)
        | "a" -> Some(Adjective)
        | "r" -> Some(Adverb)
        | _ -> None
    
    let parseFile lineParser (filePath : string) = File.ReadAllLines filePath |> Array.choose lineParser

module IndexFileParser = 
    open System
    open ParsersHelper
    
    [<CompiledNameAttribute("ParseLine")>]
    let parseLine (str : string) = 
        //See the format description here: https://wordnet.princeton.edu/wordnet/man/wndb.5WN.html.
        if not <| isCommentsLine str then 
            let fields = getFields str
            let lemma = fields.[0]
            let posField = fields.[1]
            let synsetCntField = Int32.Parse(fields.[2])
            let pCntField = Int32.Parse(fields.[3])
            
            let syntacticCategory = 
                match posField with
                | SyntacticCategory category -> category
                | _ -> 
                    raise 
                        (new FormatException(sprintf 
                                                 "Pos value %s is unknown and cannot be mapped to a SyntacticCategory value." 
                                                 posField))
            
            let synetsOffsets = 
                fields
                |> Seq.skip (6 + pCntField)
                |> Seq.map Int32.Parse
            
            { Lemma = lemma
              SyntacticCategory = syntacticCategory
              SynsetsOffsets = synetsOffsets }
            |> Some
        else None
    
    [<CompiledNameAttribute("ParseIndexFile")>]
    let parseIndexFile filePath = parseFile parseLine filePath

module ExceptionFileParser = 
    open System
    open ParsersHelper
    
    [<CompiledNameAttribute("ParseLine")>]
    let parseLine (str : string) = 
        //See the format description here: https://wordnet.princeton.edu/wordnet/man/wndb.5WN.html.
        if not <| isCommentsLine str then 
            let fields = getFields str
            let inflectedForm = fields.[0]
            let lemmas = fields |> Seq.skip 1
            { InflectedForm = inflectedForm
              Lemmas = lemmas }
            |> Some
        else None
    
    [<CompiledNameAttribute("ParseExceptionFile")>]
    let parseExceptionFile filePath = parseFile parseLine filePath
