using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using ZeonStore.Common;
using ZeonStore.WebApi.Data;
using ZeonStore.WebApi.Models;

namespace ZeonStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly StoreContext _context;

        public ApplicationsController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Applications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationInfo>>> GetApplications()
        {
            return await ToApplicationInfos(_context.Applications)
                .ToListAsync();
        }

        [HttpGet("Limited")]
        public async Task<ActionResult<IEnumerable<ApplicationInfo>>> GetApplicationsLimited(int count, int page = 0)
        {
            return await ToApplicationInfos(_context.Applications.Skip(page * count).Take(count))
                .ToListAsync();
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<ApplicationInfo>>> Search(string name, int count, int page = 0)
        {
            var query = _context.Applications
                .Where(a => a.Name.StartsWith(name))
                .Skip(page * count).Take(count);

            return await ToApplicationInfos(query).ToListAsync();
        }

        private static IQueryable<ApplicationInfo> ToApplicationInfos(IQueryable<ServerApplicationDetailedInfo> applications)
        {
            return applications
                .Include(a => a.Publisher)
                .Select(a => new ApplicationInfo()
                {
                    Id = a.Id,
                    Name = a.Name,
                    IconUrl = a.IconUrl,
                    Categories = a.Categories,
                    ShortDescription = a.ShortDescription,
                    PublisherName = a.Publisher.Name
                });
        }

        // GET: api/Applications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationDetailedInfo>> GetApplications(int id)
        {
            var application = await _context.Applications
                .Include(a => a.Publisher)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application is null)
                return NotFound();

            return application with {
                PublisherName = application.Publisher.Name, 
            };
        }

        // PUT: api/Applications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplications(int id, ApplicationDetailedInfo application)
        {
            if (id != application.Id)
            {
                return BadRequest();
            }

            _context.Entry(application).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Applications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApplicationInfo>> PostApplications(ServerApplicationDetailedInfo application)
        {
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApplication", new { id = application.Id }, application);
        }

        // DELETE: api/Applications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplications(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.Id == id);
        }
    }
}
