﻿using FamilyTreeBackend.Core.Application.Operation.Models;
using System;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IRequestResponseLogReaderService
    {
        public Task<RequestResponsePageModel> GetRequestResponseLogs(DateTime? from, DateTime? to, string userId, string path, uint page, uint pageSize = 50);
        public Task<RequestResponseLogDetailsModel> GetRequestResponseLogById(string id);
    }
}
