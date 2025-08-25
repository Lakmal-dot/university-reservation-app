using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityReservations.Models;
using UniversityReservations.Repositories;

namespace UniversityReservations.Tests.Repositories
{
    public class RepositoryTests : TestBase, IDisposable
    {
        private readonly Repository<Reservation> _repository;

        public RepositoryTests()
        {
            _repository = new Repository<Reservation>(_context);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsEntity_WhenEntityExists()
        {
            // Arrange
            var reservationId = 1;

            // Act
            var result = await _repository.GetByIdAsync(reservationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reservationId, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenEntityDoesNotExist()
        {
            // Arrange
            var reservationId = 999;

            // Act
            var result = await _repository.GetByIdAsync(reservationId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllEntities()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count()); // Should return all 3 test reservations
        }

        [Fact]
        public async Task AddAsync_AddsEntityToDatabase()
        {
            // Arrange
            var newReservation = new Reservation
            {
                LecturerId = 1,
                LectureHallId = 1,
                StartTime = DateTime.UtcNow.AddDays(3),
                EndTime = DateTime.UtcNow.AddDays(3).AddHours(2),
                Purpose = "New Test Reservation"
            };

            // Act
            await _repository.AddAsync(newReservation);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _context.Reservations.CountAsync();
            Assert.Equal(4, result); // Should have 4 reservations now
        }

        [Fact]
        public async Task ExistsAsync_ReturnsTrue_WhenEntityExists()
        {
            // Arrange
            var reservationId = 1;

            // Act
            var result = await _repository.ExistsAsync(reservationId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsAsync_ReturnsFalse_WhenEntityDoesNotExist()
        {
            // Arrange
            var reservationId = 999;

            // Act
            var result = await _repository.ExistsAsync(reservationId);

            // Assert
            Assert.False(result);
        }

        public new void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
