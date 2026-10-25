namespace AbsenceManagementSystem.DTOs.Absences
{
    using System;
    using AbsenceManagementSystem.Common.ExceptionMessages;
    using AbsenceManagementSystem.Models.Enum;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Linq;

    public class AbsenceDto
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select a child."), Display(Name = "Child")]
        public int ChildId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime? Date { get; set; } = DateTime.Today;

        [Required, EnumDataType(typeof(AbsenceType))]
        public AbsenceType? Type { get; set; }

        [StringLength(250)]
        public string? Note { get; set; }
    }
}

