using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeonStore.Common;
using ZeonStore.WebApi.Data;

namespace ZeonStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdatesController : ControllerBase
    {
        private readonly StoreContext _storeContext;

        public UpdatesController(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        [HttpGet("GetLatest/{appId}")]
        public async Task<ActionResult<Update>> GetLatestUpdate(int appId)
        {
            var update = await _storeContext.Applications
                .Where(a => a.Id == appId)
                .Include(a => a.LatestUpdate).ThenInclude(a => a.InstallFiles)
                .Select(a => a.LatestUpdate)
                .FirstOrDefaultAsync();

            if (update is null)
                return NotFound();

            return update;
        }

        [HttpGet("GetUpdates/{appId}")]
        public async Task<ActionResult<IEnumerable<Update>>> GetUpdates(int appId, DateTime releaseDate)
        {
            var application = await _storeContext.Applications
                .Where(a => a.Id == appId)
                .Include(a => a.Updates)
                .ThenInclude(a => a.UpdateFiles)
                .FirstOrDefaultAsync();

            if (application is null)
                return NotFound();

            return application.Updates.Where(u => u.ReleaseDate > releaseDate).ToList();
        }
    }
}
