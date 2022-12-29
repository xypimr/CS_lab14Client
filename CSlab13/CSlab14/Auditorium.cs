#nullable enable
using System.ComponentModel.DataAnnotations;

namespace CSlab13
{
    public class Auditorium
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int NumberOfSeats { get; set; }
        public string? Description { get; set; }
    }
}