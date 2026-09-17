namespace AbsenceManagementSystem.Data
{
    using System;
    using AbsenceManagementSystem.Models;
    using Microsoft.EntityFrameworkCore;

    public class AbsenceDbContext : DbContext
    {
        public AbsenceDbContext(DbContextOptions<AbsenceDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Absence> Absences { get; set; } = null!;

        public virtual DbSet<Child> Children { get; set; } = null!;

        public virtual DbSet<Group> Groups { get; set; } = null!;

        public virtual DbSet<Teacher> Teachers { get; set; } = null!;
    }
}

