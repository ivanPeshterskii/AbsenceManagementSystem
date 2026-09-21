namespace AbsenceManagementSystem.Services.Contracts
{
    using System;
    using AbsenceManagementSystem.Models;

    public interface ITeacherService
    {
        public Task<IEnumerable<Child>> GetAsyncAllPupils();

        public Task GetAsyncPupilById(int pupilId);

        public Task AddAsyncPupil(Child child);

        public Task RemoveAsyncPupil(int pupilId);

    }
}

