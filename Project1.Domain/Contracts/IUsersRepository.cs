using Project1.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Domain.Contracts
{
    public interface IUsersRepository : IGenericRepository<Users>
    {
        Task UpdateAsync(Users user);
        Task<IEnumerable<Users>> GetByNameAsync(string name);
    }
}
