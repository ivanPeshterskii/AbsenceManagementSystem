namespace AbsenceManagementSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using AbsenceManagementSystem.Common.Validations;

    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        [StringLength(TeacherValidations.MaxLengthTeacherName)]
        public string FirstName { get; set; } = null!;

        [StringLength(TeacherValidations.MaxLengthTeacherName)]
        public string LastName { get; set; } = null!;

        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }

        public Group Group { get; set; } = null!;
    }
}

