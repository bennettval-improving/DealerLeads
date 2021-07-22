using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealerLead.Entities
{
    public class DealerLeadUser
    {
        [Key]
        [Column("UserId")]
        public int Id { get; set; }

        public Guid AzureADId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreateDate { get; set; }
    }
}
