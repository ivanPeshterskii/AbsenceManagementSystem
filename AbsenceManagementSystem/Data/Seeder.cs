
using AbsenceManagementSystem.Models;
using AbsenceManagementSystem.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace AbsenceManagementSystem.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AbsenceDbContext context)
        {
            await context.Database.MigrateAsync();

            // Seed only an empty database.
            if (await context.Groups.AnyAsync()
                || await context.Teachers.AnyAsync()
                || await context.Children.AnyAsync()
                || await context.Absences.AnyAsync())
            {
                return;
            }

            // 1. Groups
            var groups = new List<Group>
            {
                new Group
                {
                    Name = "Butterflies",
                    AgeGroup = 3
                },
                new Group
                {
                    Name = "Ladybugs",
                    AgeGroup = 4
                },
                new Group
                {
                    Name = "Rainbows",
                    AgeGroup = 5
                },
                new Group
                {
                    Name = "Sunflowers",
                    AgeGroup = 6
                }
            };

            // 2. Teachers
            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    FirstName = "Emma",
                    LastName = "Johnson",
                    Group = groups[0]
                },
                new Teacher
                {
                    FirstName = "Olivia",
                    LastName = "Brown",
                    Group = groups[0]
                },
                new Teacher
                {
                    FirstName = "Sophia",
                    LastName = "Williams",
                    Group = groups[1]
                },
                new Teacher
                {
                    FirstName = "Charlotte",
                    LastName = "Davis",
                    Group = groups[1]
                },
                new Teacher
                {
                    FirstName = "Amelia",
                    LastName = "Wilson",
                    Group = groups[2]
                },
                new Teacher
                {
                    FirstName = "Isabella",
                    LastName = "Taylor",
                    Group = groups[2]
                },
                new Teacher
                {
                    FirstName = "Emily",
                    LastName = "Anderson",
                    Group = groups[3]
                },
                new Teacher
                {
                    FirstName = "Grace",
                    LastName = "Thomas",
                    Group = groups[3]
                }
            };

            // 3. Children
            var children = new List<Child>
            {
                // Butterflies
                new Child
                {
                    FirstName = "Liam",
                    LastName = "Smith",
                    Group = groups[0]
                },
                new Child
                {
                    FirstName = "Noah",
                    LastName = "Brown",
                    Group = groups[0]
                },
                new Child
                {
                    FirstName = "Mia",
                    LastName = "Wilson",
                    Group = groups[0]
                },
                new Child
                {
                    FirstName = "Ella",
                    LastName = "Davis",
                    Group = groups[0]
                },

                // Ladybugs
                new Child
                {
                    FirstName = "Oliver",
                    LastName = "Taylor",
                    Group = groups[1]
                },
                new Child
                {
                    FirstName = "Lucas",
                    LastName = "Martin",
                    Group = groups[1]
                },
                new Child
                {
                    FirstName = "Ava",
                    LastName = "White",
                    Group = groups[1]
                },
                new Child
                {
                    FirstName = "Chloe",
                    LastName = "Harris",
                    Group = groups[1]
                },

                // Rainbows
                new Child
                {
                    FirstName = "Ethan",
                    LastName = "Clark",
                    Group = groups[2]
                },
                new Child
                {
                    FirstName = "James",
                    LastName = "Lewis",
                    Group = groups[2]
                },
                new Child
                {
                    FirstName = "Lily",
                    LastName = "Walker",
                    Group = groups[2]
                },
                new Child
                {
                    FirstName = "Sophie",
                    LastName = "Hall",
                    Group = groups[2]
                },

                // Sunflowers
                new Child
                {
                    FirstName = "Henry",
                    LastName = "Allen",
                    Group = groups[3]
                },
                new Child
                {
                    FirstName = "Jack",
                    LastName = "Young",
                    Group = groups[3]
                },
                new Child
                {
                    FirstName = "Ruby",
                    LastName = "King",
                    Group = groups[3]
                },
                new Child
                {
                    FirstName = "Alice",
                    LastName = "Scott",
                    Group = groups[3]
                }
            };

            // 4. Absences
            var absences = new List<Absence>
            {
                new Absence
                {
                    Child = children[0],
                    Date = new DateTime(2026, 9, 16),
                    Type = AbsenceType.Medical,
                    Note = "Medical certificate provided."
                },
                new Absence
                {
                    Child = children[1],
                    Date = new DateTime(2026, 9, 17),
                    Type = AbsenceType.Excused,
                    Note = "Family reasons."
                },
                new Absence
                {
                    Child = children[4],
                    Date = new DateTime(2026, 9, 18),
                    Type = AbsenceType.Unexcused,
                    Note = "No reason provided."
                },
                new Absence
                {
                    Child = children[6],
                    Date = new DateTime(2026, 9, 21),
                    Type = AbsenceType.Medical,
                    Note = "Child was ill."
                },
                new Absence
                {
                    Child = children[9],
                    Date = new DateTime(2026, 9, 22),
                    Type = AbsenceType.Excused,
                    Note = "Parent request."
                },
                new Absence
                {
                    Child = children[12],
                    Date = new DateTime(2026, 9, 23),
                    Type = AbsenceType.Medical,
                    Note = "Medical certificate provided."
                },
                new Absence
                {
                    Child = children[15],
                    Date = new DateTime(2026, 9, 24),
                    Type = AbsenceType.Unexcused,
                    Note = "No reason provided."
                }
            };

            // Save all entities and their relationships.
            await context.Groups.AddRangeAsync(groups);
            await context.Teachers.AddRangeAsync(teachers);
            await context.Children.AddRangeAsync(children);
            await context.Absences.AddRangeAsync(absences);

            await context.SaveChangesAsync();
        }
    }
}
