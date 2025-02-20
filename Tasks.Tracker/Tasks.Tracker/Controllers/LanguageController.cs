using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Tasks.Tracker.Application.Interfaces;

namespace Tasks.Tracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageController : ControllerBase
    {
        private readonly static string nameAssembly = Assembly.GetExecutingAssembly().GetName().Name!;
        private readonly ILogger<LanguageController> _logger;
        private readonly ILanguageRepository _languageRepository;

        public LanguageController(ILogger<LanguageController> logger, ILanguageRepository languageRepository)
        {
            _logger = logger;
            _languageRepository = languageRepository;
        }

        [HttpGet]
        [Route("v1/GetLiterals")]
        public async Task<IActionResult> GetLiterals()
        {
            try
            {
                return Ok(await _languageRepository.GetLiterals());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameAssembly} - {MethodBase.GetCurrentMethod()} - {ex.Message}");
                return NotFound(ex);
            }
        }
    }
}
