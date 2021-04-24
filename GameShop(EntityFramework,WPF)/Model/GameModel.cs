using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace GameShop_EntityFramework_WPF_.Model
{
    public partial class GameModel : DbContext
    {
        public GameModel()
            : base("name=GameModel")
        {
        }

        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Style> Styles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Style>()
                .HasMany(e => e.Games)
                .WithRequired(e => e.Style)
                .HasForeignKey(e => e.Game_StyleId)
                .WillCascadeOnDelete(false);
        }
    }
}
