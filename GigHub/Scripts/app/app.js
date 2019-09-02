var AttendanceService = function () {
    var createAttendance = function () {
        $.post("/api/attendances", { GigId: gigId })
            .done(done)
            .fail(fail);
    };

    var deleteAttendace = function (gigId, done, fail) {
        $.ajax({
            url: "/api/attendances/" + gigId,
            method: "DELETE"
        })
            .done(done)
            .fail(fail);
    };

    return {
        createAttendance: createAttendance,
        deleteAttendance: deleteAttendace
    }
}()
// button.attr("data-gig-id")

var GigsController = function (attendanceService) {
    var button;
    var init = function () {
        $(".js-toggle-attendance").click(toggleAttendance);
    }

    var toggleAttendance = function (e) {
        button = $(e.target);
        var gigId = button.attr("data-gig-id");

        if (button.hasClass("btn-default")) {
            attendanceService.createAttendance(gigId, done, fail);
        }
        else {
            attendanceService.deleteAttendace(gigId, done, fail);
        }

        var done = function () {
            var text = (button.text == "Going?") ? "Going!" : "Going?";
            button.toggleClass("btn-info").toggleClass("btn-default").text(text);
        }

        var fail = function () {
            alert("Something failed! #Attendance");
        }

        return {
            init: init
        };
    }()
}(AttendanceService)