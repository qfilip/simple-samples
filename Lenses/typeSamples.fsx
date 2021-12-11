type Page = {
    HasImage: bool
    WordCount: int
}

module Page = 
    let inline _wordCount f p =
        f p.WordCount <&> fun x -> { p with WordCount = x }


type Chapter = {
    Index: int
    Pages: Page list
}

module Chapter = 
    let inline _pages f c =
        f c.Pages <&> fun x -> { c with Pages = x }


type Book = {
    Name: string
    Chapters: Chapter list
}

module Book = 
    let inline _chapters f b =
        f b.Chapters <&> fun x -> { b with Chapters = x }

    let inline _pages b =
        _chapters << Chapter._pages <| b

