using AutoMapper;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.CalendarExceptions;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.TreeExceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.FamilyEvents;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
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

        public async Task<IEnumerable<FamilyEventModel>> FindAllEventsOfTree(long treeId)
        {
            var events = await _unitOfWork.Repository<FamilyEvent>().GetDbset()
                .Where(e => e.FamilyTreeId == treeId)
                .ToListAsync();

            List<FamilyEventModel> models = new List<FamilyEventModel>();
            foreach (var familyEvent in events)
            {
                models.Add(_mapper.Map<FamilyEventModel>(familyEvent));
            }

            return models;
        }

        public async Task<FamilyEventModel> FindEventById(long eventId)
        {
            var familyEvent = await _unitOfWork.Repository<FamilyEvent>().FindAsync(eventId);

            if (familyEvent == null)
            {
                throw new FamilyEventNotFoundException(
                    CalendarExceptionMessages.FamilyEventNotFound,
                    eventId: eventId
                    );
            }
            var model = _mapper.Map<FamilyEventModel>(familyEvent);
            return model;
        }

        public async Task<FamilyEventModel> AddEventToTree(FamilyEventInputModel model)
        {
            if (!_unitOfWork.Repository<FamilyTree>().GetDbset().Any(tr => tr.Id == model.FamilyTreeId))
            {
                throw new TreeNotFoundException(TreeExceptionMessages.TreeNotFound, model.FamilyTreeId);
            }

            var familyEvent = _mapper.Map<FamilyEvent>(model);
            var result = await _unitOfWork.Repository<FamilyEvent>().AddAsync(familyEvent);
            await _unitOfWork.SaveChangesAsync();
            var resultModel = _mapper.Map<FamilyEventModel>(result);

            return resultModel;
        }

        public async Task<FamilyEventModel> RemoveEventFromTree(long eventId)
        {
            var familyEvent = await _unitOfWork.Repository<FamilyEvent>().DeleteAsync(eventId);

            if (familyEvent == null)
            {
                throw new FamilyEventNotFoundException(
                    message: CalendarExceptionMessages.FamilyEventNotFound, 
                    eventId:eventId);
            }
            await _unitOfWork.SaveChangesAsync();
            var deletedModel = _mapper.Map<FamilyEventModel>(familyEvent);
            return deletedModel;
        }

        public async Task<FamilyEventModel> UpdateFamilyEvent(long eventId, FamilyEventInputModel model)
        {
            var familyEvent = await _unitOfWork.Repository<FamilyEvent>().DeleteAsync(eventId);

            if (familyEvent == null)
            {
                throw new FamilyEventNotFoundException(
                    message: CalendarExceptionMessages.FamilyEventNotFound,
                    eventId: eventId);
            }

            _mapper.Map(model, familyEvent);
            await _unitOfWork.SaveChangesAsync();

            var returnedModel = _mapper.Map<FamilyEventModel>(familyEvent);
            return returnedModel;
        }
    }
}
