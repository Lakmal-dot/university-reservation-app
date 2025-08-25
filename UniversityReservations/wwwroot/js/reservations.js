// wwwroot/js/reservations.js
function loadLecturerReservations(lecturerId) {
    // Show loading indicator
    $('#loadingIndicator').show();

    // Make AJAX calls to get all reservation types
    Promise.all([
        $.get(`/api/ReservationsApi/NextReservation/${lecturerId}`),
        $.get(`/api/ReservationsApi/FutureReservations/${lecturerId}`),
        $.get(`/api/ReservationsApi/PastReservations/${lecturerId}`)
    ]).then(function (results) {
        const nextReservation = results[0];
        const futureReservations = results[1];
        const pastReservations = results[2];

        // Update the UI with the results
        displayReservations(nextReservation, 'next-reservation');
        displayReservations(futureReservations, 'future-reservations');
        displayReservations(pastReservations, 'past-reservations');

        // Hide loading indicator
        $('#loadingIndicator').hide();
    }).catch(function (error) {
        console.error('Error loading reservations:', error);
        $('#loadingIndicator').hide();
        alert('Error loading reservations. Please try again.');
    });
}

function displayReservations(reservations, containerId) {
    const container = $(`#${containerId}`);
    container.empty();

    if (!reservations || (Array.isArray(reservations) && reservations.length === 0)) {
        container.append('<p>No reservations found.</p>');
        return;
    }

    // Handle single reservation (for next reservation)
    if (!Array.isArray(reservations)) {
        reservations = [reservations];
    }

    const table = $('<table class="table table-striped"></table>');
    const header = $('<thead><tr><th>Lecture Hall</th><th>Start Time</th><th>End Time</th><th>Purpose</th></tr></thead>');
    const body = $('<tbody></tbody>');

    table.append(header).append(body);

    reservations.forEach(reservation => {
        const row = $(`
            <tr>
                <td>${escapeHtml(reservation.lectureHallName)}</td>
                <td>${new Date(reservation.startTime).toLocaleString()}</td>
                <td>${new Date(reservation.endTime).toLocaleString()}</td>
                <td>${escapeHtml(reservation.purpose)}</td>
            </tr>
        `);
        body.append(row);
    });

    container.append(table);
}

// Helper function to prevent XSS
function escapeHtml(unsafe) {
    if (!unsafe) return '';
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

// Initialize when document is ready
$(document).ready(function () {
    // Load reservations for default lecturer (you can change this)
    $('#lecturerSelect').change(function () {
        const lecturerId = $(this).val();
        if (lecturerId) {
            loadLecturerReservations(lecturerId);
        }
    });

    // Load initial data
    if ($('#lecturerSelect').val()) {
        loadLecturerReservations($('#lecturerSelect').val());
    }
});