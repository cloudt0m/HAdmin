using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HAdmin.Models;

namespace HAdmin.Data
{
    public interface IDataContext
    {
        DbSet<User> Users { get; init; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}