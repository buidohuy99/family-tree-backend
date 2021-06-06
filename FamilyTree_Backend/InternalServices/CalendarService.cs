using AutoMapper;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.CalendarExceptions;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.TreeExceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.FamilyEvents;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    public class CalendarService : ICalendarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CalendarService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FamilyEventOutputModel>> FindAllEventsOfTree(long treeId)
        {
            FamilyTree tree = await _unitOfWork.Repository<FamilyTree>().FindAsync(treeId);
            if (tree == null)
            {
                throw new TreeNotFoundException(TreeExceptionMessages.TreeNotFound, treeId);
            }

            var events = await _unitOfWork.Repository<FamilyEvent>().GetDbset()
                .Where(e => e.FamilyTreeId == treeId)
                .Include(e => e.EventExceptions)
                .ToListAsync();

            List<FamilyEventOutputModel> models = new List<FamilyEventOutputModel>();

            foreach (var familyEvent in events)
            {
                List<FamilyEventModel> followingEvents = new List<FamilyEventModel>();
                await recursivelyFetchFollowingEvents(familyEvent, followingEvents);
                var outputEvent = new FamilyEventOutputModel()
                {
                    FollowingEvents = followingEvents
                };
                _mapper.Map(familyEvent, outputEvent);
                models.Add(outputEvent);
            }
            
            return models;
        }

        public async Task<FamilyEventOutputModel> FindEventById(long eventId)
        {
            var familyEvent = await _unitOfWork.Repository<FamilyEvent>().GetDbset()
                .Include(e => e.EventExceptions)
                .Include(e => e.FollowingEvent)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (familyEvent == null)
            {
                throw new FamilyEventNotFoundException(
                    CalendarExceptionMessages.FamilyEventNotFound,
                    eventId: eventId
                    );
            }

            // Get following events
            List<FamilyEventModel> followingEvents = new List<FamilyEventModel>();
            await recursivelyFetchFollowingEvents(familyEvent, followingEvents);
            var outputEvent = new FamilyEventOutputModel()
            {
                FollowingEvents = followingEvents
            };
            _mapper.Map(familyEvent, outputEvent);

            return outputEvent;
        }

        public async Task<FamilyEventOutputModel> AddEventToTree(FamilyEventInputModel model)
        {
            if (!_unitOfWork.Repository<FamilyTree>().GetDbset().Any(tr => tr.Id == model.FamilyTreeId))
            {
                throw new TreeNotFoundException(TreeExceptionMessages.TreeNotFound, model.FamilyTreeId);
            }

            checkTimeSpanOfInputValid(model.Repeat, model.StartDate, model.EndDate);

            var familyEvent = _mapper.Map<FamilyEvent>(model);
            await _unitOfWork.Repository<FamilyEvent>().GetDbset().AddAsync(familyEvent);

            await _unitOfWork.SaveChangesAsync();

            // Get following events
            List<FamilyEventModel> followingEvents = new List<FamilyEventModel>();
            await recursivelyFetchFollowingEvents(familyEvent, followingEvents);
            var outputEvent = new FamilyEventOutputModel()
            {
                FollowingEvents = followingEvents
            };
            _mapper.Map(familyEvent, outputEvent);

            return outputEvent;
        }

        public async Task<FamilyEventOutputModel> RemoveEventFromTree(long eventId)
        {
            var familyEvent = await _unitOfWork.Repository<FamilyEvent>().DeleteAsync(eventId);

            if (familyEvent == null)
            {
                throw new FamilyEventNotFoundException(
                    message: CalendarExceptionMessages.FamilyEventNotFound, 
                    eventId:eventId);
            }
            await _unitOfWork.SaveChangesAsync();
            var deletedModel = _mapper.Map<FamilyEventOutputModel>(familyEvent);
            return deletedModel;
        }

        public async Task<FamilyEventOutputModel> UpdateFamilyEvent(long eventId, FamilyEventUpdateModel model)
        {
            var familyEvent = await _unitOfWork.Repository<FamilyEvent>().GetDbset().Include(e => e.FollowingEvent).FirstOrDefaultAsync(e => e.Id == eventId);

            if (familyEvent == null)
            {
                throw new FamilyEventNotFoundException(
                    message: CalendarExceptionMessages.FamilyEventNotFound,
                    eventId: eventId);
            }

            if (model.StartDate != null && model.EndDate != null)
            {
                checkTimeSpanOfInputValid(model.Repeat == null ? familyEvent.Repeat : model.Repeat.Value, model.StartDate.Value, model.EndDate.Value);

                // Updating schedule
                if (!model.ApplyToFollowingEvents)
                {
                    var exceptionCase = _mapper.Map<FamilyEventExceptionCase>(model);
                    exceptionCase.BaseFamilyEvent = familyEvent;
                    await _unitOfWork.Repository<FamilyEventExceptionCase>().AddAsync(exceptionCase);
                }
                else
                {
                    if (familyEvent.FollowingEvent != null)
                    {
                        throw new InvalidOperationOnFamilyEventException(CalendarExceptionMessages.CannotAddMultipleFollowingEventsToEvent, familyEvent.Id);
                    }
                    if(familyEvent.Repeat == RepeatEvent.NONE && model.Repeat == RepeatEvent.NONE)
                    {
                        throw new InvalidOperationOnFamilyEventException(CalendarExceptionMessages.NonRepeatableEventCantHaveFollowingEvents, familyEvent.Id);
                    }
                    if(model.StartDate.Value.CompareTo(familyEvent.EndDate) < 0)
                    {
                        throw new FamilyEventDateException(CalendarExceptionMessages.FollowingEventMustBeAfterEvent, model.StartDate, model.EndDate);
                    }
                    var newEvent = _mapper.Map<FamilyEvent>(model);
                    newEvent.FamilyTreeId = familyEvent.FamilyTreeId;
                    newEvent.ParentEvent = familyEvent;
                    if (model.Repeat == null || model.Repeat == familyEvent.Repeat)
                    {
                        newEvent.Repeat = familyEvent.Repeat;
                    }
                    if (model.ReminderOffest == null)
                    {
                        newEvent.ReminderOffest = familyEvent.ReminderOffest;
                    }
                    if (model.Note == null)
                    {
                        newEvent.Note = familyEvent.Note;
                    }
                    await _unitOfWork.Repository<FamilyEvent>().AddAsync(newEvent);
                }
            } else if (!(model.StartDate == null && model.EndDate == null)) // missing input 
            {
                throw new FamilyEventDateException(CalendarExceptionMessages.MissingDateOnInput, model.StartDate, model.EndDate);    
            } else // Update without changing the start and end date
            {
                if (model.Repeat == null || model.Repeat == familyEvent.Repeat)
                {
                    if (model.ReminderOffest != null)
                    {
                        familyEvent.ReminderOffest = model.ReminderOffest.Value;
                    }
                    if (model.Note != null)
                    {
                        familyEvent.Note = model.Note;
                    }
                }
                else
                {
                    checkTimeSpanOfInputValid(model.Repeat.Value, familyEvent.StartDate, familyEvent.EndDate);

                    var newEvent = new FamilyEvent() { 
                        ParentEvent = familyEvent,
                        Note = model.Note ?? familyEvent.Note,
                        ReminderOffest = model.ReminderOffest.HasValue ? model.ReminderOffest.Value : familyEvent.ReminderOffest,
                        StartDate = familyEvent.StartDate,
                        EndDate = familyEvent.EndDate,
                        Repeat = model.Repeat.Value,
                        FamilyTreeId = familyEvent.FamilyTreeId
                    };

                    await _unitOfWork.Repository<FamilyEvent>().AddAsync(newEvent);
                }
            }
            
            await _unitOfWork.SaveChangesAsync();

            // Get following events
            var entry = _unitOfWork.Entry(familyEvent);
            if (entry != null)
            {
                await entry.Collection(e => e.EventExceptions).LoadAsync();
            }
            List<FamilyEventModel> followingEvents = new List<FamilyEventModel>();
            await recursivelyFetchFollowingEvents(familyEvent, followingEvents);
            var outputEvent = new FamilyEventOutputModel()
            {
                FollowingEvents = followingEvents
            };
            _mapper.Map(familyEvent, outputEvent);

            return outputEvent;
        }

        public async Task<FamilyEventOutputModel> RescheduleFamilyEvent(long eventId, FamilyEventRescheduleModel model)
        {
            var familyEvent = await _unitOfWork.Repository<FamilyEvent>().GetDbset().Include(e => e.FollowingEvent).FirstOrDefaultAsync(e => e.Id == eventId);

            if (familyEvent == null)
            {
                throw new FamilyEventNotFoundException(
                    message: CalendarExceptionMessages.FamilyEventNotFound,
                    eventId: eventId);
            }

            if (familyEvent.Repeat != RepeatEvent.NONE)
            {
                if(model.StartDate == null || model.EndDate == null)
                {
                    throw new FamilyEventDateException(CalendarExceptionMessages.MissingDateOnInput, model.StartDate, model.EndDate);
                }
                //Check valid timespan of edited timespan
                checkTimeSpanOfInputValid(familyEvent.Repeat, model.StartDate.Value, model.EndDate.Value);
            }
            // Check valid timespan of rescheduled to timespan
            checkTimeSpanOfInputValid(familyEvent.Repeat, model.RescheduledStartDate, model.RescheduledEndDate);

            if (familyEvent.Repeat != RepeatEvent.NONE) {
                // Cancel old event
                var cancelOld = new FamilyEventExceptionCase()
                {
                    BaseFamilyEvent = familyEvent,
                    Note = familyEvent.Note,
                    StartDate = model.StartDate.Value,
                    EndDate = model.EndDate.Value,
                    IsCancelled = true,
                    IsRescheduled = true,
                };

                await _unitOfWork.Repository<FamilyEventExceptionCase>().AddAsync(cancelOld);
            }
            else
            {
                // Cancel all previous reschedules if event is not repeated
                var getEventExceptions = _unitOfWork.Entry(familyEvent);
                if (getEventExceptions != null)
                {
                    await getEventExceptions.Collection(e => e.EventExceptions).LoadAsync();
                }

                foreach(var exception in familyEvent.EventExceptions)
                {
                    exception.IsCancelled = true;
                }
            }

            // new rescheduled time
            var reschedule = new FamilyEventExceptionCase()
            {
                BaseFamilyEvent = familyEvent,
                Note = familyEvent.Note,
                StartDate = model.RescheduledStartDate,
                EndDate = model.RescheduledEndDate,
                IsCancelled = false,
                IsRescheduled = true,
            };
            
            await _unitOfWork.Repository<FamilyEventExceptionCase>().AddAsync(reschedule);

            await _unitOfWork.SaveChangesAsync();

            // Get following events
            var entry = _unitOfWork.Entry(familyEvent);
            if (entry != null)
            {
                await entry.Collection(e => e.EventExceptions).LoadAsync();
            }
            List<FamilyEventModel> followingEvents = new List<FamilyEventModel>();
            await recursivelyFetchFollowingEvents(familyEvent, followingEvents);
            var outputEvent = new FamilyEventOutputModel()
            {
                FollowingEvents = followingEvents
            };
            _mapper.Map(familyEvent, outputEvent);

            return outputEvent;
        }

        public async Task<FamilyEventOutputModel> CancelFamilyEvent(long eventId, FamilyEventCancelModel model)
        {
            var familyEvent = await _unitOfWork.Repository<FamilyEvent>().GetDbset().Include(e => e.FollowingEvent).FirstOrDefaultAsync(e => e.Id == eventId);

            if (familyEvent == null)
            {
                throw new FamilyEventNotFoundException(
                    message: CalendarExceptionMessages.FamilyEventNotFound,
                    eventId: eventId);
            }

            if (familyEvent.Repeat != RepeatEvent.NONE)
            {
                if (model.StartDate == null || model.EndDate == null)
                {
                    throw new FamilyEventDateException(CalendarExceptionMessages.MissingDateOnInput, model.StartDate, model.EndDate);
                }
                //Check valid timespan of cancelled timespan
                checkTimeSpanOfInputValid(familyEvent.Repeat, model.StartDate.Value, model.EndDate.Value);

                // Cancel old event
                var cancelOld = new FamilyEventExceptionCase()
                {
                    BaseFamilyEvent = familyEvent,
                    Note = familyEvent.Note,
                    StartDate = model.StartDate.Value,
                    EndDate = model.EndDate.Value,
                    IsCancelled = true,
                    IsRescheduled = true,
                };

                await _unitOfWork.Repository<FamilyEventExceptionCase>().AddAsync(cancelOld);
            }
            else
            {
                // Cancel all previous reschedules if event is not repeated
                var getEventExceptions = _unitOfWork.Entry(familyEvent);
                if (getEventExceptions != null)
                {
                    await getEventExceptions.Collection(e => e.EventExceptions).LoadAsync();
                }

                foreach (var exception in familyEvent.EventExceptions)
                {
                    exception.IsCancelled = true;
                }
            }

            await _unitOfWork.SaveChangesAsync();

            // Get following events
            var entry = _unitOfWork.Entry(familyEvent);
            if (entry != null)
            {
                await entry.Collection(e => e.EventExceptions).LoadAsync();
            }
            List<FamilyEventModel> followingEvents = new List<FamilyEventModel>();
            await recursivelyFetchFollowingEvents(familyEvent, followingEvents);
            var outputEvent = new FamilyEventOutputModel()
            {
                FollowingEvents = followingEvents
            };
            _mapper.Map(familyEvent, outputEvent);

            return outputEvent;
        }

        #region Private methods
        private async Task recursivelyFetchFollowingEvents(FamilyEvent familyEvent, List<FamilyEventModel> followingEvents)
        {
            if (followingEvents == null) return;
            var entry = _unitOfWork.Entry(familyEvent);
            if (entry != null)
            {
                await entry.Reference(e => e.FollowingEvent).LoadAsync();
                await entry.Collection(e => e.EventExceptions).LoadAsync();
                if (familyEvent.FollowingEvent == null) return;
                followingEvents.Add(_mapper.Map<FamilyEventModel>(familyEvent.FollowingEvent));
                await recursivelyFetchFollowingEvents(familyEvent.FollowingEvent, followingEvents);
            }
        }

        private void checkTimeSpanOfInputValid(RepeatEvent? repeatEvent, DateTime startDate, DateTime endDate)
        {
            if (endDate.CompareTo(startDate) <= 0)
            {
                throw new FamilyEventDateException(CalendarExceptionMessages.StartDateIsAfterEndDate, startDate, endDate);
            }

            bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
            {
                var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
                var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1));
                var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2));

                return d1 == d2;
            }

            switch (repeatEvent)
            {
               case RepeatEvent.WEEKLY:
                    if(!DatesAreInTheSameWeek(startDate, endDate))
                    {
                        throw new FamilyEventRecurringDateException(CalendarExceptionMessages.StartDateAndEndDateIsNotWithinSameRepeatCycle
                            , RepeatEvent.WEEKLY, startDate, endDate);
                    }
                    break;
                case RepeatEvent.MONTHLY:
                    if(startDate.Month != endDate.Month || startDate.Year != endDate.Year)
                    {
                        throw new FamilyEventRecurringDateException(CalendarExceptionMessages.StartDateAndEndDateIsNotWithinSameRepeatCycle
                            , RepeatEvent.MONTHLY, startDate, endDate);
                    }
                    break;
                case RepeatEvent.ANNUALLY:
                    if (startDate.Year != endDate.Year)
                    {
                        throw new FamilyEventRecurringDateException(CalendarExceptionMessages.StartDateAndEndDateIsNotWithinSameRepeatCycle
                            , RepeatEvent.ANNUALLY, startDate, endDate);
                    }
                    break;
            }
        }
        
        #endregion Private methods
    }
}
