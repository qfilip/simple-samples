type Page = {
    HasImage: bool
    WordCount: int
}

type Chapter = {
    Index: int
    Pages: Page list
}

type Book = {
    Name: string
    Chapters: Chapter list
}

module Page =
    let setWordCount (t: int -> int) (p: Page) =
        { p with WordCount = p.WordCount |> t }


module Chapter =
    let setPages (t: Page list -> Page list) (c: Chapter) =
        { c with Pages = c.Pages |> t }


module Book =
    let setChapters (t: Chapter list -> Chapter list) (b: Book) =
        { b with Chapters = b.Chapters |> t }


    let setPages (p: Page list) =
        (Chapter.setPages p