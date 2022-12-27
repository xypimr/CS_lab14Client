#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSlab13
{
    public class Assembly
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Part> Parts { get; set; } = new();
    }
}