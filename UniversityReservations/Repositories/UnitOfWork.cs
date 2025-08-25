using UniversityReservations.Data;
using UniversityReservations.Interfaces;
using UniversityReservations.Models;

namespace UniversityReservations.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Subjects = new Repository<Subject>(_context);
            Lecturers = new Repository<Lecturer>(_context);
            LectureHalls = new Repository<LectureHall>(_context);
            Reservations = new Repository<Reservation>(_context);
        }

        public IRepository<Subject> Subjects { get; private set; }
        public IRepository<Lecturer> Lecturers { get; private set; }
        public IRepository<LectureHall> LectureHalls { get; private set; }
        public IRepository<Reservation> Reservations { get; private set; }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
