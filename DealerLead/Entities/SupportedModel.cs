using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealerLead.Entities
{
    public class SupportedModel
    {
        [Key]
        [Column("ModelId")]
        public int Id { get; set; }

        [Column("ModelName")]
        public string Name { get; set; }

        [ForeignKey("MakeId")]
        public int MakeId { get; set; }

        public SupportedMake Make { get; set; }

        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreateDate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifyDate { get; set; }
    }
}
