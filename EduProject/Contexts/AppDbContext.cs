﻿using EduProject.Models;
using EduProject.Models.Common;
using EduProject.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduProject.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<TeacherSkill> TeacherSkill { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<EventSpeaker> EventSpeaker { get; set; } = null!;

        public DbSet<Speaker> Speakers { get; set; } = null!;
        public DbSet<Slider> Sliders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Slider>().HasQueryFilter(s=>s.IsDeleted == false);

            base.OnModelCreating(builder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
            string? userName = "Admin";
            var identity = _httpContextAccessor?.HttpContext?.User.Identity;
            if (identity is not null)
            {
                userName = identity.IsAuthenticated ? identity.Name : "Admin";

            }


            var entries = ChangeTracker.Entries<BaseSectionEntity>();
            foreach (var entry in entries)
            {
                switch(entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = userName;
						entry.Entity.CreatedDate = DateTime.UtcNow;
						entry.Entity.UpdatedDate = DateTime.UtcNow;
						entry.Entity.UpdatedBy = userName;
						break;
                    case EntityState.Modified:
						entry.Entity.UpdatedDate = DateTime.UtcNow;
						entry.Entity.UpdatedBy = userName;
                        break;
                        default: 
                        break;

				}
            }
           


			return base.SaveChangesAsync(cancellationToken);
		}



	}
}
