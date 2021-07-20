using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealerLead.Entities
{
    public class SupportedState
    {
        [Key]
        [Column("StateAbbreviation")]
        public string Abbreviation { get; set; }

        [Column("StateName")]
        public string Name { get; set; }
    }
}
