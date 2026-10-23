namespace AbsenceManagementSystem.DTOs.Teachers
{
    using System;
    using AbsenceManagementSystem.Common.ExceptionMessages;
    using System.ComponentModel.DataAnnotations;
    using AbsenceManagementSystem.Common.Attributes;

    public class TeacherDto
    {
        public int Id { get; set; }

        [Required, StringLength(50, MinimumLength = 2), Display(Name = "First name")]
        public string FirstName { get; set; } = null!;


        [Required, StringLength(50, MinimumLength = 2), Display(Name = "Last name")]
        public string LastName { get; set; } = null!;


        [NotNegativeNumber(ErrorMessage = ErrorMessage.NotNegativeGroupId), Display(Name = "Group")]
        public int GroupId { get; set; }
    }
}

