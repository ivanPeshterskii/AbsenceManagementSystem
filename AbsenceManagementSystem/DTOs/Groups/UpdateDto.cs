namespace AbsenceManagementSystem.DTOs.Groups
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using AbsenceManagementSystem.Common.Attributes;
    using AbsenceManagementSystem.Common.ExceptionMessages;
    using AbsenceManagementSystem.Common.Validations;

    public class UpdateDto
	{
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMessage.RequiredNameOfGroup)]
        [StringLength(GroupValidations.MaximumLenghtName,
            MinimumLength = GroupValidations.MinimumLenghtName)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = ErrorMessage.RequiredAgeOfGroup)]
        [NotNegativeNumber(ErrorMessage = ErrorMessage.NegativeAgeOfGroup)]
        public int AgeGroup { get; set; }
    }
}

