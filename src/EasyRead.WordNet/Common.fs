namespace EasyRead.WordNet

type SyntacticCategory = 
    | Noun
    | Verb
    | Adverb
    | Adjective

type ExceptionFileRecord =
    {
        InflectedForm : string;
        Lemmas : string array
    }

type IndexFileRecord =
    {
        Lemma : string;
        SyntacticCategory : SyntacticCategory;
        SynsetsOffsets : int array
    }

type DataFileRecordWord = 
    {
        // In future lex_id might be addad here which is linked to the word.
        // This is the reason why the Word field is placed into a separate record.
        Word : string;
    }

type DataFileRecord =
    {
        SynsetOffset : int;
        Words : DataFileRecordWord array;
        Gloss : string
    }
