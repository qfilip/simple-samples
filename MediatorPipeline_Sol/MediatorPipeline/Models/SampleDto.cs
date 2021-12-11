using System;

namespace MediatorPipeline.Models
{
    public class SampleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsValidDto { get; set; }
        public bool ThrowException { get; set; }

        public static SampleDto GetSelf(bool isValid, bool throwEx)
        {
            var name = string.Empty;
            name += isValid ? "Valid Dto " : "Invalid Dto";
            name += throwEx ? "with exception" : "without exception";

            return new SampleDto()
            {
                Id = new Random().Next(0, 1001),
                Name = name,
                IsValidDto = isValid,
                ThrowException = throwEx
            };
        }
    }
}
