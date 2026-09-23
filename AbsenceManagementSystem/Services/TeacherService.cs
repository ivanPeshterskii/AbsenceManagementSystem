namespace AbsenceManagementSystem.Services
{
    using System;
    using System.Collections.Generic;
    using AbsenceManagementSystem.Common.ExceptionMessages;
    using AbsenceManagementSystem.Data;
    using AbsenceManagementSystem.Models;
    using AbsenceManagementSystem.Services.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class TeacherService : ITeacherService
    {
        private readonly AbsenceDbContext _context;

        public TeacherService(AbsenceDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Adds teacher into the database.
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws when group doen not exist.</exception>

        public async Task AddAsync(Teacher teacher)
        {
            ArgumentNullException.ThrowIfNull(teacher);

            bool groupExists = await _context.Groups
                .AnyAsync(g => g.Id == teacher.GroupId);

            if (!groupExists)
            {
                throw new InvalidOperationException(ErrorMessage.GroupWithThisNameDoesNotExist);
            }

            await _context.Teachers.AddAsync(teacher);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the teacher with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        /// <exception cref="InvalidOperationException">When teacher idis different, throws an exception.</exception>

        public async Task<bool> DeleteAsync(int id)
        {
            Teacher? teacher = await _context.Teachers
                .FindAsync(id);

            if (teacher == null)
            {
                throw new InvalidOperationException(ErrorMessage.TeacherWithThisIdDoesNotExist);
            }

            _context.Teachers.Remove(teacher);

            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Gets all teachers asynchronously
        /// </summary>
        /// <returns>IEnumerable<Teacher></returns>

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await _context.Teachers
                .AsNoTracking()
                .OrderBy(n => n.FirstName)
                .ThenBy(l => l.LastName)
                .ToListAsync();
        }

        /// <summary>
        /// Gets teache by its groupId param
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>IEnumerable<Teacher></returns>

        public async Task<IEnumerable<Teacher>> GetByGroupIdAsync(int groupId)
        {
            return await _context.Teachers
                .AsNoTracking()
                .OrderBy(n => n.FirstName)
                .ThenBy(l => l.LastName)
                .Where(t => t.GroupId == groupId)
                .ToListAsync();
        }

        /// <summary>
        /// Gets teacher with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Teacher?</returns>

        public async Task<Teacher?> GetByIdAsync(int id)
        {
            return await _context.Teachers
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// Updates the teacher in the database.
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If teacher or group don't exist, it throws exception.</exception>

        public async Task UpdateAsync(Teacher teacher)
        {
            ArgumentNullException.ThrowIfNull(teacher);

            Teacher? existingTeacher = await _context.Teachers
                .FindAsync(teacher.Id);

            if (existingTeacher == null)
            {
                throw new InvalidOperationException(ErrorMessage.TeacherDoesNotExist);
            }

            bool groupExists = await _context.Groups
                .AnyAsync(g => g.Id == teacher.GroupId);

            if (!groupExists)
            {
                throw new InvalidOperationException(ErrorMessage.GroupDoesNotExist);
            }

            existingTeacher.FirstName = teacher.FirstName;
            existingTeacher.LastName = teacher.LastName;
            existingTeacher.GroupId = teacher.GroupId;

            await _context.SaveChangesAsync();
        }

    }
}

