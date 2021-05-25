using FamilyTreeBackend.Core.Application.Helpers;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.FamilyEvents;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Presentation.API.Controllers
{
    [Area("calendar-management")]
    [ApiController]
    public partial class CalendarController : BaseController
    {
        private readonly ICalendarService _calendarService;
        public CalendarController(
            ICalendarService calendarService,
            UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _calendarService = calendarService;
        }

        [HttpGet("tree/{treeId}")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<IEnumerable<FamilyEventModel>>),
            Description = "Return the list of events that belong to the tree")]
        public async Task<IActionResult> GetEventsFromTree(long treeId)
        {
            var result = await _calendarService.FindAllEventsOfTree(treeId);
            return Ok(new HttpResponse<IEnumerable<FamilyEventModel>>(result,
                GenericResponseStrings.CalendarController_FindEventsSuccessful));
        }

        [HttpPost("calendar")]
        public async Task<IActionResult> AddFamilyEvent([FromBody] FamilyEventInputModel model)
        {
            var result = await _calendarService.AddEventToTree(model);
            return Ok(new HttpResponse<FamilyEventModel>(result,
                GenericResponseStrings.CalendarController_AddEventSuccessful));
        }

        [HttpDelete("calendar/{eventId}")]
        public async Task<IActionResult> RemoveFamilyEvent(long eventId)
        {
            var result = await _calendarService.RemoveEventFromTree(eventId);
            return Ok(GenericResponseStrings.CalendarController_RemoveEventSuccessful);
        }

        [HttpPut("calendar/{eventId}")]
        public async Task<IActionResult> UpdateFamilyEvent(long eventId, [FromBody] FamilyEventInputModel model)
        {
            var result = await _calendarService.UpdateFamilyEvent(eventId, model);
            return Ok(new HttpResponse<FamilyEventModel>(
                result,
                GenericResponseStrings.CalendarController_UpdateEventSuccessful));
        }
    }
}
