namespace AbsenceManagementSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using AbsenceManagementSystem.Models.Enum;

    public class Absence
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "DATETIME2")]
        public DateTime Date { get; set; }

        public AbsenceType Type { get; set; }

        [StringLength(250)]
        public string? Note { get; set; }

        [ForeignKey(nameof(Child))]
        public int ChildId { get; set; }

        public Child Child { get; set; } = null!;
    }
}

