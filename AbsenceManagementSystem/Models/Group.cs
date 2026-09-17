namespace AbsenceManagementSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "NVARCHAR(35)")]
        public string Name { get; set; } = null!;

        public int AgeGroup { get; set; }

        public ICollection<Child> Children { get; set; }
            = new List<Child>();

        public ICollection<Teacher> Teachers { get; set; }
            = new List<Teacher>();
    }
}

