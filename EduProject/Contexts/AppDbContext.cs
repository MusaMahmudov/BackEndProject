using EduProject.Models;
using EduProject.Models.Common;
using EduProject.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduProject.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<TeacherSkill> TeacherSkill { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<EventSpeaker> EventSpeaker { get; set; } = null!;

        public DbSet<Speaker> Speakers { get; set; } = null!;
        public DbSet<Slider> Sliders { get; set; } = null!;


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
            var entries = ChangeTracker.Entries<BaseSectionEntity>();
            foreach (var entry in entries)
            {
                switch(entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = "Admin";
						entry.Entity.CreatedDate = DateTime.UtcNow;
						entry.Entity.UpdatedDate = DateTime.UtcNow;
						entry.Entity.UpdatedBy = "Admin";
						break;
                    case EntityState.Modified:
						entry.Entity.UpdatedDate = DateTime.UtcNow;
						entry.Entity.UpdatedBy = "Admin";
                        break;
                        default: 
                        break;

				}
            }



			return base.SaveChangesAsync(cancellationToken);
		}



	}
}
