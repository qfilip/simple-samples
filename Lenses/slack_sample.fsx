#r "nuget: FSharpPlus"
open FSharpPlus.Lens

type Page = {
    No: int
    Content: string
}

type Chapter = {
    Title: string
    Pages: Page list
}
module Chapter =
    let inline _pages f (chapter: Chapter) =
        f chapter.Pages <&> fun x -> { chapter with Pages = x }


type Book = {
    Title: string
    Chapters: Chapter list
}
module Book =
    let inline _chapters f (book: Book) =
        f book.Chapters <&> fun x -> { book with Chapters = x }


let book = {
    Title = "How I failed with lenses"
    Chapters = [
        {
            Title = "Wtf???"
            Pages = [
                { No = 1; Content = "As I started reading" }
                { No = 2; Content = "I was getting confused" }
            ]
        }
        {
            Title = "Again... Wtf???"
            Pages = [
                { No = 3; Content = "I cannot figure this out." }
                { No = 4; Content = "I am not amused" }
            ]
        }
        {
            Title = "Slack Post"
            Pages = [
                { No = 5; Content = "I thought Slack should be used" }
                { No = 6; Content = "Heeeeelp!" }
            ]
        }
    ]
}

[<RequireQualifiedAccess>]
module rec List =
    open FSharpPlus.Control
    let removeAt i lst =
        if List.length lst > i then
            lst.[0..i-1] @ lst.[i+1..]
        else lst
    let setAt i x lst =
        if List.length lst > i then
            lst.[0..i-1] @ x::lst.[i+1..]
        else lst
    /// Given a specific key, produces a Lens from a Map<key, value> to an Option<value>.  When setting,
    /// a Some(value) will insert or replace the value into the map at the given key.  Setting a value of
    /// None will delete the value at the specified key.  Works well together with non.
    let inline _item i f t = Map.InvokeOnInstance
                              (function | None -> List.removeAt i t | Some x -> List.setAt i x t)
                              (f (List.tryItem i t))

let updatedBook =
    book
    |> setl 
        (Book._chapters << List._item 2 << _Some << Chapter._pages << List._item 1)
        (Some { No = 6; Content = "Thanks, but now I'm more confused" })


printfn "%A" updatedBook


// use F# Plus lenses to change content of a single page... how?
