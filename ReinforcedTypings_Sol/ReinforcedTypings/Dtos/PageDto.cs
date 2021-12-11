using System;

namespace ReinforcedTypings.Dtos
{
    public class PageDto : BaseDto
    {
        public Guid ChapterId { get; set; }
        public int ParagraphCount { get; set; }
        public int Number { get; set; }
    }
}
