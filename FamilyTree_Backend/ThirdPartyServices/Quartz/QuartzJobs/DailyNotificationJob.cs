using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Core.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.Quartz.QuartzJobs
{
    [DisallowConcurrentExecution]
    public class DailyNotificationJob : IJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DailyNotificationJob> _logger;
        public DailyNotificationJob(IUnitOfWork unitOfWork, ILogger<DailyNotificationJob> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            return DistributeNotification();
        }

        private async Task DistributeNotification()
        {
            var param = new SqlParameter("Today", DateTime.Now);
            var eventsNeedNotification = await _unitOfWork.Repository<FamilyEvent>().GetDbset()
                .FromSqlRaw(Sql_GetNotificationNeededEvents, param)
                .AsNoTracking()
                .ToListAsync();

            foreach(var familyEvent in eventsNeedNotification)
            {
                NotifyUsers(familyEvent).Start();
            }
        }

        private async Task NotifyUsers(FamilyEvent familyEvent)
        {
            var tree =  await _unitOfWork.Repository<FamilyTree>().GetDbset()
                .Include(tr => tr.Owner)
                .Include(tr => tr.Editors)
                .Where(tr => tr.Id == familyEvent.FamilyTreeId)
                .SingleOrDefaultAsync();

            AddNotificationToUser($"Reminder for event: {familyEvent.Note} on {familyEvent.StartDate:yyyy/MM/dd HH:mm}", tree.Owner).Start();

            foreach(var editors in tree.Editors)
            {
                AddNotificationToUser($"Reminder for event: {familyEvent.Note}", editors).Start();
            }
        }

        private async Task AddNotificationToUser(string message, ApplicationUser user)
        {
            var notification = new Notification()
            {
                Message = message,
                UserId = user.Id,
            };
            
            await _unitOfWork.Repository<Notification>().AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        private const string Sql_GetNotificationNeededEvents = @"
            WITH
            _needNotifyEvents AS (
	        SELECT e1.*
	        FROM FamilyEvent e1 LEFT JOIN FamilyEvent e2 ON (e1.Id = e2.ParentEventId AND e1.Id != e2.Id)
	        WHERE
		        --have not been replaced yet
		        e2.ParentEventId IS NULL
		        --got replaced, but the next one is far away and have not comes yet
		        OR (e2.ParentEventId IS NOT NULL AND CAST(e2.StartDate AS DATE) > DATEADD(day, e2.ReminderOffest, CAST(@Today AS DATE)))
            ),

            SELECT *
            FROM _needNotifyEvents e
            WHERE
        	--non repeat
        	(e.Repeat = 0 AND CAST(e.StartDate AS DATE) = DATEADD(day, e.ReminderOffest, CAST(@Today AS DATE)))
        	--repeat weekly
        	OR (e.Repeat = 1 AND DATEPART(WEEKDAY, e.StartDate)  = DATEPART(WEEKDAY, DATEADD(day, e.ReminderOffest, @Today)))
        	--repeat monthly
        	OR (e.Repeat = 2 AND DATEPART(DAY, e.StartDate)  = DATEPART(DAY, DATEADD(day, e.ReminderOffest, @Today)))
        	--repeat annually
        	OR (e.Repeat = 3 
		        AND DATEPART(day, e.StartDate)  = DATEPART(DAY, DATEADD(day, e.ReminderOffest, @Today)))
		        AND DATEPART(MONTH, e.StartDate)  = DATEPART(DAY, DATEADD(MONTH, e.ReminderOffest, @Today))";
    }
}
