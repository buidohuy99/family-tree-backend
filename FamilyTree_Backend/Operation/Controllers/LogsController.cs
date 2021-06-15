using FamilyTreeBackend.Core.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Operation.Controllers
{
    public class LogsController : Controller
    {
        private readonly IRequestResponseLogReaderService _logReaderService;

        private const int PageSize = 20;
        public LogsController(IRequestResponseLogReaderService logReaderService)
        {
            _logReaderService = logReaderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime from, DateTime to, int currentPage, string searchUserId, string path)
        {
            var result = await _logReaderService.GetRequestResponseLogs(from, to, searchUserId, path, (uint)currentPage, PageSize);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var result = await _logReaderService.GetRequestResponseLogById(id);
            return View(result);
        }
    }
}
