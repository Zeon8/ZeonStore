using Microsoft.EntityFrameworkCore;
using ZeonStore.WebApi.Models;

namespace ZeonStore.WebApi.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext (DbContextOptions<StoreContext> options)
            : base(options){}

        public required DbSet<ServerApplicationDetailedInfo> Applications { get; set; }
    }
}
