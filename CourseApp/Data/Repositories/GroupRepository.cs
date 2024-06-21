using Core.Entities;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class GroupRepository:Repository<Group>, IGroupRepository
    {
        private readonly AppDbContext _appDbContext;
        public GroupRepository(AppDbContext context) : base(context)
        {

        }
    }
}
