using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using thTODO.Api.Context;
using thTODO.Api.Model;

namespace thTODO.Api.Repository
{
    public class ToDoRepository : Repository<ToDo>, IRepository<ToDo>
    {
        public ToDoRepository(ToDoContext dbContext) : base(dbContext)
        {
        }
    }
}
