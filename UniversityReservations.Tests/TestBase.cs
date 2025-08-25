using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityReservations.Data;

namespace UniversityReservations.Tests
{
    public abstract class TestBase : IDisposable
    {
        protected readonly ApplicationDbContext _context;

        protected TestBase()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            // Seed test data
            _context.Subjects.AddRange(TestDataFactory.GetTestSubjects());
            _context.Lecturers.AddRange(TestDataFactory.GetTestLecturers());
            _context.LectureHalls.AddRange(TestDataFactory.GetTestLectureHalls());
            _context.Reservations.AddRange(TestDataFactory.GetTestReservations());
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
