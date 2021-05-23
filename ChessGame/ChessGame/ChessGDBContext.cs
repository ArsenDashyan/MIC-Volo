using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ChessGame
{
    public partial class ChessGDBContext : DbContext
    {
        public ChessGDBContext()
        {
        }

        public ChessGDBContext(DbContextOptions<ChessGDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<GameDetail> GameDetails { get; set; }
        public virtual DbSet<UserDetail> UserDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb; Database=ChessGDB; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<GameDetail>(entity =>
            {
                entity.HasKey(e => e.GameId);

                entity.Property(e => e.GameId).ValueGeneratedNever();

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.GameCondition).IsRequired();

                entity.Property(e => e.GameStory).IsRequired();

                entity.HasOne(d => d.Game)
                    .WithOne(p => p.GameDetail)
                    .HasForeignKey<GameDetail>(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameDetails_UserDetails");
            });

            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Opponent)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
