namespace AbsenceManagementSystem.DTOs.Children
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using AbsenceManagementSystem.Common.Attributes;
    using AbsenceManagementSystem.Common.Validations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ChildDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(ChildValidations.MaxLengthChildName,
            MinimumLength = ChildValidations.MinLengthChildName),
            Display(Name = "First name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(ChildValidations.MaxLengthChildName,
            MinimumLength = ChildValidations.MinLengthChildName),
            Display(Name = "Last name")]
        public string LastName { get; set; } = null!;

        [Required]
        [NotNegativeNumber(ErrorMessage = "Select a group"),
            Display(Name = "Group")]
        public int GroupId { get; set; }

        public IEnumerable<SelectListItem> Groups { get; set; }
            = new List<SelectListItem>();
    }
}

