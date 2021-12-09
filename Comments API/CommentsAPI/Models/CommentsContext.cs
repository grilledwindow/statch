using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentsAPI.Models
{
    public class CommentsContext : DbContext
    {
        public CommentsContext(DbContextOptions<CommentsContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Comments> Comments { get; set; }
    }
}
