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

    public class ChildService : IChildService
    {
        private readonly AbsenceDbContext _context;

        public ChildService(AbsenceDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Adds child into the database.
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws if the child is null.</exception>

        public async Task AddAsync(Child child)
        {
            bool existChild = await _context.Children
                .AnyAsync(i => i.Id == child.Id);

            if (existChild)
            {
                throw new InvalidOperationException(ErrorMessage.ChildAlreadyExist);
            }

            await _context.Children.AddAsync(child);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes absence from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        /// <exception cref="InvalidOperationException">Throws if the child does not exist.</exception>

        public async Task<bool> DeleteAsync(int id)
        {
            Child? child = await _context.Children
                .FindAsync(id);

            if (child == null)
            {
                throw new InvalidOperationException(ErrorMessage.ChildDoesNotExist);
            }

            _context.Children.Remove(child);

            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Gets all children from database, asynchronously.
        /// </summary>
        /// <returns>IEnumerable<Child></returns>

        public async Task<IEnumerable<Child>> GetAllAsync()
        {
            return await _context.Children
                .AsNoTracking()
                .OrderBy(f => f.FirstName)
                .ThenBy(l => l.LastName)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all groups with id's
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>IEnumerable<Child></returns>
        /// <exception cref="InvalidOperationException">Throws if group doesn't exist</exception>

        public async Task<IEnumerable<Child>> GetByGroupIdAsync(int groupId)
        {
            Group? group = await _context.Groups
                .FindAsync(groupId);

            if (group == null)
            {
                throw new InvalidOperationException(ErrorMessage.GroupDoesNotExist);
            }

            return await _context.Children
                .AsNoTracking()
                .OrderBy(f => f.FirstName)
                .ThenBy(l => l.LastName)
                .Where(id => id.GroupId == group.Id)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the child with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Child?</returns>
        /// <exception cref="InvalidOperationException">Throws when child doesn't exist.</exception>

        public async Task<Child?> GetByIdAsync(int id)
        {
            Child? child = await _context.Children
                .FindAsync(id);

            if (child == null)
            {
                throw new InvalidOperationException(ErrorMessage.ChildDoesNotExist);
            }

            return await _context.Children
                .AsNoTracking()
                .FirstOrDefaultAsync(id => id.Id == child.Id);

        }

        /// <summary>
        /// Updates the child information in the database
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws when the child or group don't exist.</exception>

        public async Task UpdateAsync(Child child)
        {
            ArgumentNullException.ThrowIfNull(child);

            Child? existingChild = await _context.Children
                .FindAsync(child.Id);

            if (existingChild == null)
            {
                throw new InvalidOperationException(ErrorMessage.ChildDoesNotExist);
            }

            bool groupExists = await _context.Groups
                .AnyAsync(t => t.Id == child.GroupId);

            if (!groupExists)
            {
                throw new InvalidOperationException(ErrorMessage.GroupDoesNotExist);
            }

            existingChild.FirstName = child.FirstName;
            existingChild.LastName = child.LastName;
            existingChild.GroupId = child.GroupId;

            await _context.SaveChangesAsync();
        }
    }
}

