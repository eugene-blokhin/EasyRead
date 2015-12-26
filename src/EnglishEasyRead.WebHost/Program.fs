module Program

open EnglishEasyRead.DictionaryApi
open EnglishEasyRead.WordNet
open Nancy.TinyIoc
open System

type Bootstrapper() = 
    inherit Nancy.DefaultNancyBootstrapper()
    override this.ConfigureApplicationContainer(container : TinyIoCContainer) = 
        let wordNetDatabase = @"C:\WordNet\2.1\dict"
        container.Register<IWordNetFacade, WordNetFacade>(new WordNetFacade(wordNetDatabase)) |> ignore
        container.Register<IMorpher, Morpher>(new Morpher(wordNetDatabase)) |> ignore
        container.Register<ITextAnalysisService, TextAnalysisService>().AsSingleton() |> ignore
        ()

[<EntryPoint>]
let main args = 
    let nancy = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:8080"))
    nancy.Start()
    while true do
        Console.ReadLine() |> ignore
    0
