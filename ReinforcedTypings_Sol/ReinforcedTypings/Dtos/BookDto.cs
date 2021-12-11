using System.Collections.Generic;

namespace ReinforcedTypings.Dtos
{
    public class BookDto : BaseDto
    {
        public string Title { get; set; }
        public List<ChapterDto> Chapters { get; set; }
    }
}
