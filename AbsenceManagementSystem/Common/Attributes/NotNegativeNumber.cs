namespace AbsenceManagementSystem.Common.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class NotNegativeNumber : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is int number)
            {
                return number > 0;
            }

            return false;
        }
    }
}

