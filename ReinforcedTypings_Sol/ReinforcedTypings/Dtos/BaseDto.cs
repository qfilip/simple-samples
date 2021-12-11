using System;
using ReinforcedTypings.Enumerations;

namespace ReinforcedTypings.Dtos
{   
    public class BaseDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public eEntityStatus EntityStatus { get; set; }
    }
}
