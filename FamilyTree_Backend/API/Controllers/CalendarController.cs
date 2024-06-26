﻿using FamilyTreeBackend.Core.Application.Helpers;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.FamilyEvents;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Presentation.API.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Presentation.API.Controllers
{
    [Area("calendar-management")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public partial class CalendarController : BaseController
    {
        private readonly ICalendarService _calendarService;
        private readonly ITreeAuthorizationService _authorizationService;
        public CalendarController(
            ICalendarService calendarService,
            UserManager<ApplicationUser> userManager,
            ITreeAuthorizationService authorizationService) : base(userManager)
        {
            _calendarService = calendarService;
            _authorizationService = authorizationService;
        }

        [HttpGet("events/tree/{treeId}")]
        [SwaggerOperation(Summary = "Get all events of tree (including follow up events)")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<IEnumerable<FamilyEventOutputModel>>),
            Description = "Return the list of events that belong to the tree")]
        public async Task<IActionResult> GetEventsFromTree(long treeId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                treeId,
                EventOperations.Read
                );
            if (!authorizationResult.Succeeded)
            {
                return StatusCode(403, new HttpResponse<AuthorizationFailure>(
                    authorizationResult.Failure,
                    GenericResponseStrings.Tree_NoPermissionRead));
            }

            var result = await _calendarService.FindAllEventsOfTree(treeId);
            return Ok(new HttpResponse<IEnumerable<FamilyEventOutputModel>>(result,
                GenericResponseStrings.CalendarController_FindEventsSuccessful));
        }

        [HttpPost("event")]
        [SwaggerOperation(Summary = "Add new event to tree (not a follow up event)")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<FamilyEventOutputModel>),
            Description = "Return newly created event")]
        public async Task<IActionResult> AddFamilyEvent([FromBody] FamilyEventInputModel model)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                model.FamilyTreeId,
                EventOperations.Create
            );
            if (!authorizationResult.Succeeded)
            {
                return StatusCode(403, new HttpResponse<AuthorizationFailure>(
                    authorizationResult.Failure,
                    GenericResponseStrings.Tree_NoPermissionEdit));
            }

            var result = await _calendarService.AddEventToTree(model);
            return Ok(new HttpResponse<FamilyEventOutputModel>(result,
                GenericResponseStrings.CalendarController_AddEventSuccessful));
        }

        [HttpDelete("event/{eventId}")]
        [SwaggerOperation(Summary = "Remove an event out of a tree (any event)")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<object>),
            Description = "Return a message showing that its deleted")]
        public async Task<IActionResult> RemoveFamilyEvent(long eventId)
        {
            var authorizationResult = await _authorizationService.AuthorizeWithEventAsync(
                User,
                eventId,
                EventOperations.Delete
            );
            if (!authorizationResult.Succeeded)
            {
                return StatusCode(403, new HttpResponse<AuthorizationFailure>(
                    authorizationResult.Failure,
                    GenericResponseStrings.Tree_NoPermissionEdit));
            }

            await _calendarService.RemoveEventFromTree(eventId);
            return Ok(new HttpResponse<object>(null, GenericResponseStrings.CalendarController_RemoveEventSuccessful));
        }

        [HttpPut("event/{eventId}")]
        [SwaggerOperation(Summary = "Update original event OR Create a follow up event/event exception(need to provide start&end dates)")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<FamilyEventOutputModel>),
            Description = "Return the updated event")]
        public async Task<IActionResult> UpdateFamilyEvent(long eventId, [FromBody] FamilyEventUpdateModel model)
        {
            var authorizationResult = await _authorizationService.AuthorizeWithEventAsync(
                User,
                eventId,
                EventOperations.Update
            );
            if (!authorizationResult.Succeeded)
            {
                return StatusCode(403, new HttpResponse<AuthorizationFailure>(
                    authorizationResult.Failure,
                    GenericResponseStrings.Tree_NoPermissionEdit));
            }

            var result = await _calendarService.UpdateFamilyEvent(eventId, model);
            return Ok(new HttpResponse<FamilyEventOutputModel>(
                result,
                GenericResponseStrings.CalendarController_UpdateEventSuccessful));
        }

        [HttpPut("event/{eventId}/reschedule")]
        [SwaggerOperation(Summary = "Reschedule an event")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<FamilyEventOutputModel>),
            Description = "Return the event with the newly added reschedule exception(s)")]
        public async Task<IActionResult> RescheduleFamilyEvent(long eventId, [FromBody] FamilyEventRescheduleModel model)
        {
            var authorizationResult = await _authorizationService.AuthorizeWithEventAsync(
                User,
                eventId,
                EventOperations.Reschedule
            );
            if (!authorizationResult.Succeeded)
            {
                return StatusCode(403, new HttpResponse<AuthorizationFailure>(
                    authorizationResult.Failure,
                    GenericResponseStrings.Tree_NoPermissionEdit));
            }

            var result = await _calendarService.RescheduleFamilyEvent(eventId, model);
            return Ok(new HttpResponse<FamilyEventOutputModel>(
                result,
                GenericResponseStrings.CalendarController_RescheduleEventSuccessful));
        }

        [HttpPut("event/{eventId}/cancel")]
        [SwaggerOperation(Summary = "Cancel an event")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<FamilyEventOutputModel>),
            Description = "Return the event with the newly added cancellation exception")]
        public async Task<IActionResult> CancelFamilyEvent(long eventId, [FromBody] FamilyEventCancelModel model)
        {
            var authorizationResult = await _authorizationService.AuthorizeWithEventAsync(
                User,
                eventId,
                EventOperations.Cancel
            );
            if (!authorizationResult.Succeeded)
            {
                return StatusCode(403, new HttpResponse<AuthorizationFailure>(
                    authorizationResult.Failure,
                    GenericResponseStrings.Tree_NoPermissionEdit));
            }

            var result = await _calendarService.CancelFamilyEvent(eventId, model);
            return Ok(new HttpResponse<FamilyEventOutputModel>(
                result,
                GenericResponseStrings.CalendarController_CancelEventSuccessful));
        }
    }
}
