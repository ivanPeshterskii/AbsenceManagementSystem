namespace AbsenceManagementSystem.Services.Contracts
{
    using System;
    using AbsenceManagementSystem.Models;

    public interface IChildService
    {
        public Task<IEnumerable<Child>> GetAllAsync();

        public Task<Child?> GetByIdAsync(int id);

        public Task<IEnumerable<Child>> GetByGroupIdAsync(int groupId);

        public Task AddAsync(Child child);

        public Task UpdateAsync(Child child);

        public Task<bool> DeleteAsync(int id);
    }
}

