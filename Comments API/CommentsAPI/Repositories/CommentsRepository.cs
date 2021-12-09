using CommentsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentsAPI.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly CommentsContext _context;
        public CommentsRepository(CommentsContext context)
        {
            _context = context;
        }

        public async Task<Comments> Create(Comments comments)
        {
            _context.Comments.Add(comments);
            await _context.SaveChangesAsync();

            return comments;
        }

        public async Task Delete(int id)
        {
            var commentsToDelete = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(commentsToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comments>> Get()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comments> Get(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task Update(Comments comments)
        {
            _context.Entry(comments).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
