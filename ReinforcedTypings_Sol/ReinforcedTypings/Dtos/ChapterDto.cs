using System;
using System.Collections.Generic;

namespace ReinforcedTypings.Dtos
{
    public class ChapterDto : BaseDto
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public List<PageDto> Pages { get; set; }
    }
}
