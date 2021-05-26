using FamilyTreeBackend.Core.Application.Models.FamilyEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface ICalendarService
    {
        public Task<IEnumerable<FamilyEventOutputModel>> FindAllEventsOfTree(long treeId);

        public Task<FamilyEventOutputModel> AddEventToTree(FamilyEventInputModel model);

        public Task<FamilyEventOutputModel> RemoveEventFromTree(long eventId);

        public Task<FamilyEventOutputModel> UpdateFamilyEvent(long eventId, FamilyEventUpdateModel model);
        public Task<FamilyEventOutputModel> FindEventById(long eventId);
    }
}
