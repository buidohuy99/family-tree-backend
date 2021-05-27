using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.QuartzJobs
{
    public class DailyNotificationJob : IJob
    {
        private readonly IUnitOfWork _unitOfWork;
        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }

        private async Task DistributeNotification()
        {
            var eventsNeedNotification = await _unitOfWork.Repository<FamilyEvent>().GetDbset()
                .Where(s => DoNeedNotification(s))
                .ToListAsync();
        }

        private bool DoNeedNotification(FamilyEvent familyEvent)
        {
            var dateInFuture = DateTime.Now.AddDays(familyEvent.ReminderOffest);
            switch (familyEvent.Repeat)
            {
                case RepeatEvent.WEEKLY:
                    return dateInFuture.DayOfWeek == familyEvent.StartDate.DayOfWeek;

                case RepeatEvent.MONTHLY:
                    return dateInFuture.Date == familyEvent.StartDate.Date;

                case RepeatEvent.ANNUALLY:
                    return (dateInFuture.Date == familyEvent.StartDate.Date)
                        && (dateInFuture.Month == familyEvent.StartDate.Month);

                case RepeatEvent.NEVER:
                    int reminderDays = (familyEvent.StartDate - DateTime.Now).Days;
                    return reminderDays == familyEvent.ReminderOffest;

                default:
                    return false;
            }
        }
    }
}
