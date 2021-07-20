using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealerLead.Entities
{
    public class SupportedMake
    {
        [Key]
        [Column("MakeId")]
        public int Id { get; set; }

        [Column("MakeName")]
        public string Name { get; set; }

        public List<SupportedModel> Models { get; set; }

        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreateDate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifyDate { get; set; }
    }
}
