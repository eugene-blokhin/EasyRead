namespace EnglishEasyRead.WordNet

//See the format description for WordNet database files here: https://wordnet.princeton.edu/wordnet/man/wndb.5WN.html.
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
        if not <| isCommentsLine str then 
            let fields = getFields str
            let lemma = fields.[0]
            let posField = fields.[1]
            let synsetCntField = Int32.Parse(fields.[2])
            let pCntField = Int32.Parse(fields.[3])
            
            let syntacticCategory = 
                match posField with
                | SyntacticCategory category -> category
                | _ -> raise (new FormatException(sprintf "Pos value %s is unknown and cannot be mapped to a SyntacticCategory value." posField))
            
            let synetsOffsets = 
                fields
                |> Seq.skip (6 + pCntField)
                |> Seq.take synsetCntField
                |> Seq.map Int32.Parse
            
            { Lemma = lemma
              SyntacticCategory = syntacticCategory
              SynsetsOffsets = synetsOffsets |> Array.ofSeq }
            |> Some
        else None
    
    [<CompiledNameAttribute("ParseIndexFile")>]
    let parseIndexFile filePath = parseFile parseLine filePath

module ExceptionFileParser = 
    open ParsersHelper
    
    [<CompiledNameAttribute("ParseLine")>]
    let parseLine (str : string) = 
        if not <| isCommentsLine str then 
            let fields = getFields str
            let inflectedForm = fields.[0]
            let lemmas = fields |> Seq.skip 1
            { InflectedForm = inflectedForm
              Lemmas = lemmas |> Array.ofSeq }
            |> Some
        else None
    
    [<CompiledNameAttribute("ParseExceptionFile")>]
    let parseExceptionFile filePath = parseFile parseLine filePath

module DataFileParser = 
    open ParsersHelper
    open System
    
    [<CompiledNameAttribute("ParseLine")>]
    let parseLine (str : string) = 
        if not <| isCommentsLine str then 
            let fields = getFields str
            let synsetOffsetField = Int32.Parse(fields.[0])
            let wCntField = Convert.ToInt32(fields.[3], 16)
            let glossField = (str.LastIndexOf('|') + 1 |> str.Substring).Trim()
            
            let words = 
                [| for i in 0..wCntField - 1 -> { Word = fields.[4 + i * 2] } |]
            { SynsetOffset = synsetOffsetField
              Words = words
              Gloss = glossField }
            |> Some
        else None
    
    [<CompiledNameAttribute("ParseDataFile")>]
    let parseDataFile filePath = parseFile parseLine filePath
