using CommentsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentsAPI.Repositories
{
    public interface ICommentsRepository
    {
        Task<IEnumerable<Comments>> Get();
        Task<Comments> Get(int id);
        Task<Comments> Create(Comments comments);
        Task Update(Comments comments);
        Task Delete(int id);
    }
}
