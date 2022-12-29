#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSlab13
{
    public class Building
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<AuditoriumGroup> AuditoriumGroups { get; set; } = new();
    }
}