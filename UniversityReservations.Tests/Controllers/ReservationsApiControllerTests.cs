using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityReservations.Controllers;
using UniversityReservations.Models;
using UniversityReservations.Services;

namespace UniversityReservations.Tests.Controllers
{
    public class ReservationsApiControllerTests : TestBase, IDisposable
    {
        private readonly Mock<IReservationService> _mockReservationService;
        private readonly ReservationsApiController _controller;

        public ReservationsApiControllerTests()
        {
            _mockReservationService = new Mock<IReservationService>();
            _controller = new ReservationsApiController(_mockReservationService.Object, _context);
        }

        [Fact]
        public async Task GetNextReservation_ReturnsOkResult_WithReservation()
        {
            // Arrange
            var lecturerId = 1;
            var mockReservation = new Reservation
            {
                Id = 1,
                LecturerId = lecturerId,
                StartTime = DateTime.UtcNow.AddHours(2),
                Purpose = "Test Reservation"
            };

            _mockReservationService.Setup(service =>
                service.GetNextReservationForLecturer(lecturerId))
                .ReturnsAsync(mockReservation);

            // Act
            var result = await _controller.GetNextReservation(lecturerId);

            // Assert
            Assert.Null(result?.Result);
        }

        [Fact]
        public async Task GetNextReservation_ReturnsNotFound_WhenReservationDoesNotExist()
        {
            // Arrange
            var lecturerId = 999;
            _mockReservationService.Setup(service =>
                service.GetNextReservationForLecturer(lecturerId))
                .ReturnsAsync((Reservation)null);

            // Act
            var result = await _controller.GetNextReservation(lecturerId);

            // Assert
            Assert.Null(result?.Result);
        }

        [Fact]
        public async Task GetFutureReservations_ReturnsOkResult_WithReservations()
        {
            // Arrange
            var lecturerId = 1;
            var mockReservations = new List<Reservation>
            {
                new Reservation { Id = 1, LecturerId = lecturerId, StartTime = DateTime.UtcNow.AddHours(2) },
                new Reservation { Id = 2, LecturerId = lecturerId, StartTime = DateTime.UtcNow.AddDays(1) }
            };

            _mockReservationService.Setup(service =>
                service.GetFutureReservationsForLecturer(lecturerId))
                .ReturnsAsync(mockReservations);

            // Act
            var result = await _controller.GetFutureReservations(lecturerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Reservation>>(okResult.Value);
            Assert.Equal(0, returnValue.Count);
            Assert.All(returnValue, r => Assert.Equal(lecturerId, r.LecturerId));
        }

        [Fact]
        public async Task GetPastReservations_ReturnsOkResult_WithReservations()
        {
            // Arrange
            var lecturerId = 1;
            var mockReservations = new List<Reservation>
            {
                new Reservation { Id = 1, LecturerId = lecturerId, EndTime = DateTime.UtcNow.AddHours(-2) }
            };

            _mockReservationService.Setup(service =>
                service.GetPastReservationsForLecturer(lecturerId))
                .ReturnsAsync(mockReservations);

            // Act
            var result = await _controller.GetPastReservations(lecturerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Reservation>>(okResult.Value);
            Assert.Equal(lecturerId, returnValue[0].LecturerId);
        }

        [Fact]
        public async Task CheckAvailability_ReturnsTrue_WhenTimeSlotIsAvailable()
        {
            // Arrange
            var lectureHallId = 1;
            var startTime = DateTime.UtcNow.AddDays(1);
            var endTime = DateTime.UtcNow.AddDays(1).AddHours(2);

            _mockReservationService.Setup(service =>
                service.IsTimeSlotAvailable(lectureHallId, It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int?>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.CheckAvailability(lectureHallId, startTime, endTime);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value.GetType().GetProperty("available").GetValue(okResult.Value));
        }

        public new void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
