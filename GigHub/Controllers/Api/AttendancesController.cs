using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Persistence;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendancesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            var currentUser = User.Identity.GetUserId();
            var attendance = _unitOfWork.Attendances.GetAttendance(dto.GigId, currentUser);
            if (attendance != null)
                return BadRequest("The attendance already exists.");

            attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = currentUser
            };
            _unitOfWork.Attendances.Add(attendance);
            _unitOfWork.Complete();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteAttendance(int id)
        {
            var currentUser = User.Identity.GetUserId();

            // Find the attendance record.
            var attendance = _unitOfWork.Attendances.GetAttendance(id, currentUser);

            if (attendance == null)
            {
                return NotFound();
            }

            // Remove the record.
            _unitOfWork.Attendances.Remove(attendance);
            _unitOfWork.Complete();


            return Ok(id);
        }
    }
}
