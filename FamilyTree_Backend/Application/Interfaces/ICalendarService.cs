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
        public Task<IEnumerable<FamilyEventModel>> FindAllEventsOfTree(long treeId);

        public Task<FamilyEventModel> AddEventToTree(FamilyEventInputModel model);

        public Task<FamilyEventModel> RemoveEventFromTree(long eventId);

        public Task<FamilyEventModel> UpdateFamilyEvent(long eventId, FamilyEventUpdateModel model);
        public Task<FamilyEventModel> FindEventById(long eventId);

        public Task<FamilyEventModel> AddCustomHistoryToEvent(long eventId, FamilyEventHistoryInputModel model);
        public Task<FamilyEventModel> UpdateCustomHistoryOfEvent(long customHistoryId, FamilyEventHistoryInputModel model);
        public Task<FamilyEventModel> RemoveCustomHistoryFromEvent(long customHistoryId);
    }
}
