using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityReservations.Models;

namespace UniversityReservations.Tests
{
    public static class TestDataFactory
    {
        public static List<Subject> GetTestSubjects()
        {
            return new List<Subject>
            {
                new Subject { Id = 1, Name = "Mathematics", Description = "Math course" },
                new Subject { Id = 2, Name = "Physics", Description = "Physics course" }
            };
        }

        public static List<Lecturer> GetTestLecturers()
        {
            return new List<Lecturer>
            {
                new Lecturer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@uni.edu", SubjectId = 1 },
                new Lecturer { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@uni.edu", SubjectId = 2 }
            };
        }

        public static List<LectureHall> GetTestLectureHalls()
        {
            return new List<LectureHall>
            {
                new LectureHall { Id = 1, Name = "Hall A", Capacity = 100, HasProjector = true },
                new LectureHall { Id = 2, Name = "Hall B", Capacity = 50, HasProjector = false }
            };
        }

        public static List<Reservation> GetTestReservations()
        {
            var now = DateTime.UtcNow;
            return new List<Reservation>
            {
                new Reservation
                {
                    Id = 1,
                    LecturerId = 1,
                    LectureHallId = 1,
                    StartTime = now.AddDays(-1),
                    EndTime = now.AddDays(-1).AddHours(2),
                    Purpose = "Past Math Lecture"
                },
                new Reservation
                {
                    Id = 2,
                    LecturerId = 1,
                    LectureHallId = 1,
                    StartTime = now.AddHours(2),
                    EndTime = now.AddHours(4),
                    Purpose = "Future Math Lecture"
                },
                new Reservation
                {
                    Id = 3,
                    LecturerId = 2,
                    LectureHallId = 2,
                    StartTime = now.AddDays(1),
                    EndTime = now.AddDays(1).AddHours(3),
                    Purpose = "Future Physics Lecture"
                }
            };
        }
    }
}
