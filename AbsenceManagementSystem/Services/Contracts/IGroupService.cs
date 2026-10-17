namespace AbsenceManagementSystem.Services.Contracts
{
    using System;
    using AbsenceManagementSystem.DTOs.Groups;
    using AbsenceManagementSystem.Models;

    public interface IGroupService
    {
        public Task<IEnumerable<Group>> GetAllAsync();

        public Task<Group?> GetByIdAsync(int id);

        public Task AddAsync(CreateDto group);

        public Task UpdateAsync(Group group);

        public Task<bool> DeleteAsync(int id);
    }
}

