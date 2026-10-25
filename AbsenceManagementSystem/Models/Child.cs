namespace AbsenceManagementSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using AbsenceManagementSystem.Common.Validations;

    public class Child
    {
        [Key]
        public int Id { get; set; }

        [StringLength(ChildValidations.MaxLengthChildName)]
        public string FirstName { get; set; } = null!;

        [StringLength(ChildValidations.MaxLengthChildName)]
        public string LastName { get; set; } = null!;

        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }

        public Group Group { get; set; } = null!;

        public ICollection<Absence> Absences { get; set; }
            = new List<Absence>();
    }
}