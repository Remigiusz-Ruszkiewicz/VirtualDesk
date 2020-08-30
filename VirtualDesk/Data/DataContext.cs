using Microsoft.EntityFrameworkCore;
using VirtualDesk.Models;

namespace VirtualDesk.Data
{using VirtualDesk.Models;
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public DbSet<EmailAccountList> emailAccountLists { get; set; }
        public DbSet<AccountsList> accountsLists { get; set; }
    }
}
