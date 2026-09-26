namespace AbsenceManagementSystem.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AbsenceManagementSystem.Common.ExceptionMessages;
    using AbsenceManagementSystem.Data;
    using AbsenceManagementSystem.Models;
    using AbsenceManagementSystem.Services.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class GroupService : IGroupService
    {
        private readonly AbsenceDbContext _context;

        public GroupService(AbsenceDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Adds group to the database.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If not found, it throws an exception.</exception>

        public async Task AddAsync(Group group)
        {
            bool existGroup = await _context.Groups
                .AnyAsync(i => i.Id == group.Id);

            if (existGroup)
            {
                throw new InvalidOperationException(ErrorMessage.GroupDoesNotExist);
            }

            await _context.Groups.AddAsync(group);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the group with its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        /// <exception cref="InvalidOperationException">If not found, it throws an exception.</exception>

        public async Task<bool> DeleteAsync(int id)
        {
            Group? group = await _context.Groups
                .FindAsync(id);

            if (group == null)
            {
                throw new InvalidOperationException(ErrorMessage.GroupDoesNotExist);
            }

            _context.Groups.Remove(group);

            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Gives a collection of groups, including teachers and children
        /// </summary>
        /// <returns>IEnumerable<Group></returns>

        public async Task<IEnumerable<Group>> GetAllAsync()
        {
            return await _context.Groups
                .AsNoTracking()
                .Include(c => c.Children)
                .Include(t => t.Teachers)
                .OrderBy(n => n.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Gets group with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Group?</returns>
        /// <exception cref="InvalidOperationException">If not found by the given id, it throws an exception.</exception>

        public async Task<Group?> GetByIdAsync(int id)
        {
            Group? group = await _context.Groups
                .FindAsync(id);

            if (group == null)
            {
                throw new InvalidOperationException(ErrorMessage.GroupDoesNotExist);
            }

            return await _context.Groups
                .AsNoTracking()
                .FirstOrDefaultAsync(id => id.Id == group.Id);
        }

        /// <summary>
        /// Updates the group information i nthe database.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If not found, it throws an exception.</exception>

        public async Task UpdateAsync(Group group)
        {
            ArgumentNullException.ThrowIfNull(group);

            Group? existingGroup = await _context.Groups
                .FindAsync(group.Id);

            if (existingGroup == null)
            {
                throw new InvalidOperationException(ErrorMessage.GroupDoesNotExist);
            }

            existingGroup.Name = group.Name;
            existingGroup.AgeGroup = group.AgeGroup;

            await _context.SaveChangesAsync();
        }
    }
}

