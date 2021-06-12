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
        public LogsController(IRequestResponseLogReaderService logReaderService)
        {
            _logReaderService = logReaderService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _logReaderService.GetRequestResponseLogs(null, null, 1, 50);
            return View(result);
        }
    }
}
