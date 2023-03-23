using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using thTODO.Api.Context;
using thTODO.Api.Model;

namespace thTODO.Api.Repository
{
    public class UserRepository : Repository<User>, IRepository<User>
    {
        public UserRepository(ToDoContext dbContext) : base(dbContext)
        {
        }
    }
}
