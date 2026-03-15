using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POC.API.Common;
using POC.Application.Journal.Interfaces;

namespace POC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [SkipJournal]
    public class JournalController : ControllerBase
    {
        private readonly IJournalRepository _repository;

        public JournalController(IJournalRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Search(
            Guid? userId,
            string? userName,
            string? email,
            string? endpoint,
            DateTime? from,
            DateTime? to)
        {
            var result = await _repository.SearchAsync(
                userId,
                userName,
                email,
                endpoint,
                from,
                to);

            return Ok(result);
        }
    }
}
