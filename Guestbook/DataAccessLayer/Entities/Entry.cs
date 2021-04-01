using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    public class Entry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Email { get; set; }

        [Required, MinLength(5), MaxLength(600)]
        public string Comment { get; set; }

        [Required]
        public DateTime EntryTime { get; set; }
    }
}
