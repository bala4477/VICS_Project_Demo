using Microsoft.EntityFrameworkCore;
using Project1.Domain.Contracts;
using Project1.Domain.Models;
using Project1.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Infrastructure.Repos
{
    public class UsersRepository : GenericRepository<Users>, IUsersRepository
    {

        public UsersRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<IEnumerable<Users>> GetByNameAsync(string name)
        {
            return await _dbContext.Set<Users>().Where(x => x.Name == name).ToListAsync();
        }

        public async Task UpdateAsync(Users user)
        {
            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
