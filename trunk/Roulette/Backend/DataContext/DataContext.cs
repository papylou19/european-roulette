using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Domain;
using Domain.MemberShip;

namespace Backend.DataContext
{
    public class RouletteContext : DbContext
    {
        public DbSet<Stake> Stakes { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Identities> Identities { get; set; }
        public DbSet<Cashier> Cashiers { get; set; }
        public DbSet<ASP_Membership> Memberships { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UsersInRole> UsersInRoles { get; set; }
        public DbSet<Asp_User> Users { get; set; }
        public DbSet<GameState> GameStates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            this.Configuration.ValidateOnSaveEnabled = false;
            base.OnModelCreating(modelBuilder);
        }
    }
}
