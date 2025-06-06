﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TimeManage.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<TimeTable> TimeTables { get; set; }
        public virtual ICollection<ToDo> ToDos { get; set; }

        public string DisplayName { get; set; }

        // 🔧 Add this method
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // The authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here if needed
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }

        public DbSet<ToDo> ToDos { get; set; }

        public string DisplayName { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TimeTable>()
                .HasRequired(t => t.User)
                .WithMany(u => u.TimeTables)
                .HasForeignKey(t => t.UserId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TimeSlot>()
                .HasRequired(s => s.TimeTable)
                .WithMany(t => t.TimeSlots)
                .HasForeignKey(s => s.TimeTableId)
                .WillCascadeOnDelete(true);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
