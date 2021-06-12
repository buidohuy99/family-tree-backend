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

        private const int PageSize = 50;
        public LogsController(IRequestResponseLogReaderService logReaderService)
        {
            _logReaderService = logReaderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime from, DateTime to, int currentPage)
        {
            var result = await _logReaderService.GetRequestResponseLogs(from, to, (uint)currentPage, PageSize);
            return View(result);
        }
    }
}
