﻿var AttendanceService = function () {
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