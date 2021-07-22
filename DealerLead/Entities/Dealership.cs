using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealerLead.Entities
{
    public class Dealership
    {
        [Key]
        [Column("DealershipId")]
        public int Id { get; set; }

        [Required]
        [Column("DealershipName")]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string StreetAddress1 { get; set; }

        [StringLength(200)]
        public string StreetAddress2 { get; set; }

        [StringLength(75)]
        public string City { get; set; }

        public string State { get; set; }

        [StringLength(10)]
        public string Zipcode { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public int CreatingUserId { get; set; }
    }
}
