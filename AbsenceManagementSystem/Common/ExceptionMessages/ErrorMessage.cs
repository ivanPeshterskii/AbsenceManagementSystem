namespace AbsenceManagementSystem.Common.ExceptionMessages
{
    using System;

    public static class ErrorMessage
    {
        /* Teacher service exceptions begin */

        public const string TeacherWithThisIdDoesNotExist = "A teacher with this id doesn't exist!";

        public const string TeacherDoesNotExist = "This teacher doesn't exist!";

        /* Teacher service exceptions end */

        /* Child service exceptions begin */

        public static string TwoPupilsAreNotAllowed = "There is already a pupil with same data!";

        public const string ChildAlreadyExist = "Child already exists!";

        public const string ChildDoesNotExist = "Child doesn't exist!";

        /* Child service exceptions end */

        /* Group service exceptions begin */

        public static string GroupWithThisNameDoesNotExist = "A group with this name doesn't exist!";

        public const string GroupDoesNotExist = "This group doesn't exist!";

        /* Group service exceptions end */

        /* Absence service exceptions begin */

        public static string InvalidAbsenceType = "This type is invalid!";

        public static string NotFoundAbsence = "This absence hasn't been found!";

        /* Absence service exceptions end */
    }
}

