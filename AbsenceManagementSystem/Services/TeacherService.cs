namespace AbsenceManagementSystem.Services
{
    using System;
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

        public async Task AddAsyncPupil(Child child)
        {
            if (await IsExist(child))
            {
                throw new InvalidOperationException(ErrorMessage.TwoPupilsAreNotAllowed);
            }

            _context.Children.AddAsync(child);
            _context.SaveChangesAsync();
        }

        public Task<IEnumerable<Child>> GetAsyncAllPupils()
        {
            throw new NotImplementedException();
        }

        public Task GetAsyncPupilById(int pupilId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsyncPupil(int pupilId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Private method, that checkes if a pupil already exists.
        /// If yes, it throws an exception atherwise create and add to dbcontext the pupil.
        /// </summary>
        /// <param name="child"></param>
        /// <returns>boolean</returns>
        private async Task<bool> IsExist(Child child)
        {
            return await _context.Children
                .AnyAsync(c => c.FirstName == child.FirstName
                && c.LastName == c.LastName);
        }

    }
}

