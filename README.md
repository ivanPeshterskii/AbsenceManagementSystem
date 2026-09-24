# AbsenceManagementSystem

## A quick overview

This project will be used to help the teachers in the Draginovo's kindergarden ["ДГ - Пролет с. Драгиново"](https://prolet-dg.eu). Here teachers can add their froups and manage pupils' absences easily. The system is quite simple, they can do all the CRUD operations and manage their work. 

## Tech Stack

| Layer     |   Technology  |
| ----------| :----------- |
| Backend   | C# / ASP.NET Core MVC |
| ORM       | Entity Framework 6 / Core |
| Database  | SQL Server (Docker Host) |
| Frontend  | Razor views |
| IDE       | Visual Studio for Mac / Visual Studio Code |
| Version Control | Git & GitHub |

## Database Schema

The application uses **Entity Framework Core** with **SQL Server**

The database consists of four main entities:

- `Group`
- `Child`
- `Teacher`
- `Absence`

### Group
Represents a kindergarten group.

| Property | Type | Required | Description |
| --------- | ---- | -------- | ---------- |
| Id        | int | Yes       | PK        |
| Name      | string | Yes    | Name of group |
| AgeGroup  | int  | Yes      | Age group of the children |

### Child
Represents a child, attending the kindergarten.

| Property | Type | Required | Description |
| --------- | ---- | -------- | ---------- |
| Id        | int  |   Yes    | PK         |
| FirstName | string | Yes    | Child's first name|
| LastName  | string | Yes    | Child's last name |
| GroupId   | int     | Yes    | Fk to Group      |

### Teacher
Represents a teacher, assigned to the kindergarten group.

| Property | Type | Required | Description |
| --------- | ---- | -------- | ---------- |
| Id        | int  |   Yes    | PK         |
| FirstName | string | Yes    | Teacher's first name|
| LastName  | string | Yes    | Teacher's last name |
| GroupId   | int     | Yes    | Fk to Group      |

### Absence
Represents an absence, registered for a child.

| Property | Type | Required | Description |
| --------- | ---- | -------- | ---------- |
| Id        | int  |   Yes    | PK         |
| Date      | DateTime | Yes  | Date of the absence |
| Type      | AbsenceType | Yes | Type of absence |
| Note      | string?     | No  | Optional additional information |

### AbsenceType

```csharp
public enum AbsenceType 
{
    Unexcused = 1,
    Excused = 2,
    Medical = 3
}
```

### Business logic

All the business logic will be separated into `services` that implements a specific interface. 

A piece of the project worktree:

```text
AbsenceManagementSystem/
│
├── Services/
│   │
│   ├── Contracts/
│   │   ├── IChildService.cs
│   │   ├── IGroupService.cs
│   │   ├── ITeacherService.cs
│   │   └── IAbsenceService.cs
│   │
│   ├── ChildService.cs
│   ├── GroupService.cs
│   ├── TeacherService.cs
│   └── AbsenceService.cs
│
└── Program.cs
```
