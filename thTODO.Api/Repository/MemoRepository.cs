using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using thTODO.Api.Context;
using thTODO.Api.Model;

namespace thTODO.Api.Repository
{
    public class MemoRepository : Repository<Memo>, IRepository<Memo>
    {
        public MemoRepository(ToDoContext dbContext) : base(dbContext)
        {
        }
    }
}
