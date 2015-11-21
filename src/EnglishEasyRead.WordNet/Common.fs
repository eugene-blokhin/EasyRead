namespace EnglishEasyRead.WordNet

type SyntacticCategory = 
    | Noun
    | Verb
    | Adverb
    | Adjective

type ExceptionFileRecord =
    {
        InflectedForm : string;
        Lemmas : string seq
    }

type IndexFileRecord =
    {
        Lemma : string;
        SyntacticCategory : SyntacticCategory;
        SynsetsOffsets : int seq
    }