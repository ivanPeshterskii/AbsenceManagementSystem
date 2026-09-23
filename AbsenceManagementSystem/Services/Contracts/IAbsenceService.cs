namespace AbsenceManagementSystem.Services.Contracts
{
    using System;
    using AbsenceManagementSystem.Models;

    public interface IAbsenceService
    {
        public Task<IEnumerable<Absence>> GetAllAsync();

        public Task<Absence?> GetByIdAsync(int id);

        public Task<IEnumerable<Absence>> GetByChildIdAsync(
            int childId);

        public Task<IEnumerable<Absence>> GetByDateAsync(
            DateTime date);

        public Task<IEnumerable<Absence>> GetByGroupAndDateAsync(
            int groupId,
            DateTime date);

        public Task AddAsync(Absence absence);

        public Task UpdateAsync(Absence absence);

        public Task<bool> DeleteAsync(int id);
    }
}

