using System;
using AbsenceManagementSystem.Common.ExceptionMessages;
using AbsenceManagementSystem.Data;
using AbsenceManagementSystem.Models;
using AbsenceManagementSystem.Models.Enum;
using AbsenceManagementSystem.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AbsenceManagementSystem.Services
{
    public class AbsenceService : IAbsenceService
    {
        private readonly AbsenceDbContext _context;

        public AbsenceService(AbsenceDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Gets all absences, asynchronously and returns it in IEnumerable collection.
        /// </summary>
        /// <returns>IEnumerable<Absence></returns>

        public async Task<IEnumerable<Absence>> GetAllAsync()
        {
            return await _context.Absences
                .AsNoTracking()
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        /// <summary>
        /// Gets an absence with its id, asynchronously.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Absence</returns>

        public async Task<Absence?> GetByIdAsync(int id)
        {
            return await _context.Absences
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        /// <summary>
        /// Gets absences by child's id, asynchronously.
        /// </summary>
        /// <param name="childId"></param>
        /// <returns>IEnumerable<Absence></returns>

        public async Task<IEnumerable<Absence>> GetByChildIdAsync(int childId)
        {
            return await _context.Absences
                .AsNoTracking()
                .Where(a => a.ChildId == childId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the absences, that have been added in an exact date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>IEnumerable<Absence></returns>

        public async Task<IEnumerable<Absence>> GetByDateAsync(DateTime date)
        {
            DateTime start = date.Date;
            DateTime end = start.AddDays(1);

            return await _context.Absences
                .AsNoTracking()
                .Where(a => a.Date >= start &&
                            a.Date < end)
                .OrderBy(a => a.ChildId)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the absences, that have been added in an exact date and group.
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="date"></param>
        /// <returns>IEnumerable<Absence></returns>

        public async Task<IEnumerable<Absence>> GetByGroupAndDateAsync(int groupId, DateTime date)
        {
            DateTime start = date.Date;
            DateTime end = start.AddDays(1);

            return await _context.Absences
                .AsNoTracking()
                .Where(a => a.Child.GroupId == groupId &&
                            a.Date >= start &&
                            a.Date < end)
                .OrderBy(a => a.Child.FirstName)
                .ThenBy(a => a.Child.LastName)
                .ToListAsync();
        }

        /// <summary>
        /// Adds an absence asynchronously into the database.
        /// </summary>
        /// <param name="absence"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws if child doesn't exist</exception>
        /// <exception cref="ArgumentException">Throws if type of absence is invalid.</exception>

        public async Task AddAsync(Absence absence)
        {
            ArgumentNullException.ThrowIfNull(absence);

            bool childExists = await _context.Children
                .AnyAsync(c => c.Id == absence.ChildId);

            if (!childExists)
            {
                throw new InvalidOperationException(ErrorMessage.ChildDoesNotExist);
            }

            if (!Enum.IsDefined(typeof(AbsenceType), absence.Type))
            {
                throw new ArgumentException(ErrorMessage.InvalidAbsenceType);
            }

            await _context.Absences.AddAsync(absence);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the status of the selected absence.
        /// </summary>
        /// <param name="absence"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Throws when absence doesn't exist.</exception>
        /// <exception cref="InvalidOperationException">Throws when child doesn't exist.</exception>
        /// <exception cref="ArgumentException">Throws when absence type is invalid.</exception>

        public async Task UpdateAsync(Absence absence)
        {
            ArgumentNullException.ThrowIfNull(absence);

            Absence? existingAbsence = await _context.Absences
                .FindAsync(absence.Id);

            if (existingAbsence == null)
            {
                throw new KeyNotFoundException(ErrorMessage.NotFoundAbsence);
            }

            bool childExists = await _context.Children
                .AnyAsync(c => c.Id == absence.ChildId);

            if (!childExists)
            {
                throw new InvalidOperationException(ErrorMessage.ChildDoesNotExist);
            }

            if (!Enum.IsDefined(
                typeof(AbsenceType), absence.Type))
            {
                throw new ArgumentException(ErrorMessage.InvalidAbsenceType);
            }

            existingAbsence.Date = absence.Date;
            existingAbsence.Type = absence.Type;
            existingAbsence.Note = absence.Note;
            existingAbsence.ChildId = absence.ChildId;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the exact absence with the same id, that has been given form the user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>

        public async Task<bool> DeleteAsync(int id)
        {
            Absence? absence = await _context.Absences
                .FindAsync(id);

            if (absence == null)
            {
                return false;
            }

            _context.Absences.Remove(absence);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}

