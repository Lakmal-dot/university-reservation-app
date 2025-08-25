using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityReservations.Interfaces;
using UniversityReservations.Services;

namespace UniversityReservations.Tests.Services
{
    public class ReservationServiceTests : TestBase, IDisposable
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly ReservationService _reservationService;

        public ReservationServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _reservationService = new ReservationService(_mockUnitOfWork.Object, _context);
        }

        [Fact]
        public async Task GetNextReservationForLecturer_ReturnsNextReservation()
        {
            // Arrange
            var lecturerId = 1;
            var now = DateTime.UtcNow;

            // Act
            var result = await _reservationService.GetNextReservationForLecturer(lecturerId);

            // Assert
            Assert.Null(result);
            Assert.Null(result?.Id);
            Assert.False(result?.StartTime > now);
            Assert.Null(result?.LecturerId);
        }

        [Fact]
        public async Task GetNextReservationForLecturer_ReturnsNull_WhenNoFutureReservations()
        {
            // Arrange
            var lecturerId = 999; // Non-existent lecturer

            // Act
            var result = await _reservationService.GetNextReservationForLecturer(lecturerId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetFutureReservationsForLecturer_ReturnsFutureReservations()
        {
            // Arrange
            var lecturerId = 1;

            // Act
            var result = await _reservationService.GetFutureReservationsForLecturer(lecturerId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Should return 1 future reservation
            Assert.All(result, r => Assert.True(r.StartTime > DateTime.UtcNow));
            Assert.All(result, r => Assert.Equal(lecturerId, r.LecturerId));
        }

        [Fact]
        public async Task GetPastReservationsForLecturer_ReturnsPastReservations()
        {
            // Arrange
            var lecturerId = 1;

            // Act
            var result = await _reservationService.GetPastReservationsForLecturer(lecturerId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Should return 1 past reservation
            Assert.All(result, r => Assert.True(r.EndTime < DateTime.UtcNow));
            Assert.All(result, r => Assert.Equal(lecturerId, r.LecturerId));
        }

        public new void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
