using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Domain;

namespace Backend.DataContext
{
    public class RouletteContext : DbContext
    {
        public DbSet<Stake> Stakes { get; set; }
        public DbSet<Identities> Identities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            this.Configuration.ValidateOnSaveEnabled = false;
            base.OnModelCreating(modelBuilder);
        }
    }
}
