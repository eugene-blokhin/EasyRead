namespace EnglishEasyRead.CoreApi


open System
open Nancy
open Nancy.ModelBinding

[<CLIMutableAttribute>]
type UploadTextModel =
    {
        Name : string
        TextBody : string;
        Lemmas : string array;
    }

type CoreApiModule(textService:ITextService) as this =
    inherit NancyModule("coreApi")

    let (?) (parameters:obj) param : DynamicDictionaryValue = 
        (parameters :?> Nancy.DynamicDictionary).[param] :?> DynamicDictionaryValue

    do
        this.Post.["text"] <- fun _ ->
            try
                let model = this.Bind<UploadTextModel>()
                textService.SaveText model.Name model.TextBody model.Lemmas :> obj
            with 
            | :? ModelBindingException as e -> HttpStatusCode.BadRequest :> obj
        
        this.Get.["text/{id:guid}/lemmas"] <- fun request ->
            textService.GetLemmasByTextId ((request?id).TryParse<Guid>()) :> obj