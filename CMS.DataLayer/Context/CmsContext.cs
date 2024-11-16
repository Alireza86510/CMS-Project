using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.DataLayer.Models;
using System.Drawing;

namespace CMS.DataLayer.Context
{
    public class CmsContext : DbContext
    {
        public CmsContext(DbContextOptions<CmsContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        #region Post

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostReaction> PostReactions { get; set; }
        public DbSet<PostComment> PostComments { get; set; }

        #endregion

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.PostComments)
                .WithOne(e => e.Post)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.PostReactions)
                .WithOne(e => e.Post)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
