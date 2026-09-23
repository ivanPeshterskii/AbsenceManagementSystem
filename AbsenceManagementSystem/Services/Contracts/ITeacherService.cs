namespace AbsenceManagementSystem.Services.Contracts
{
    using System;
    using AbsenceManagementSystem.Models;

    public interface ITeacherService
    {
        public Task<IEnumerable<Teacher>> GetAllAsync();

        public Task<Teacher?> GetByIdAsync(int id);

        public Task<IEnumerable<Teacher>> GetByGroupIdAsync(int groupId);

        public Task AddAsync(Teacher teacher);

        public Task UpdateAsync(Teacher teacher);

        public Task<bool> DeleteAsync(int id);

    }
}

