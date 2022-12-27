#nullable enable
using System.ComponentModel.DataAnnotations;

namespace CSlab13
{
    public class Detail
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
    }
}