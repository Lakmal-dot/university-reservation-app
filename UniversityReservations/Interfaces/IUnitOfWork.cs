using UniversityReservations.Models;

namespace UniversityReservations.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Subject> Subjects { get; }
        IRepository<Lecturer> Lecturers { get; }
        IRepository<LectureHall> LectureHalls { get; }
        IRepository<Reservation> Reservations { get; }
        Task<int> CommitAsync();
    }
}
