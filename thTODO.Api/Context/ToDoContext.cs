using Microsoft.EntityFrameworkCore;
using thTODO.Api.Model;

namespace thTODO.Api.Context
{
    public class ToDoContext : DbContext
    {
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Memo> Memo { get; set; }
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {

        }
    }
}
