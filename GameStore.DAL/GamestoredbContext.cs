using System;
using GameStore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace GameStore.DAL
{
    public partial class GamestoredbContext : DbContext
    {
        public GamestoredbContext()
        {
        }

        public GamestoredbContext(DbContextOptions<GamestoredbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Activation> Activations { get; set; }
        public virtual DbSet<Developer> Developers { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<GameGenre> GameGenres { get; set; }
        public virtual DbSet<GameOrder> GameOrders { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Key> Keys { get; set; }
        public virtual DbSet<MinimumSpecification> MinimumSpecifications { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
             if (!optionsBuilder.IsConfigured)
             {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("server=localhost;user=root;password=123123123;database=gamestoredb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activation>(entity =>
            {
                entity.ToTable("activation");

                entity.HasIndex(e => e.Name, "name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Developer>(entity =>
            {
                entity.ToTable("developer");

                entity.HasIndex(e => e.Name, "name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("game");

                entity.HasIndex(e => e.DeveloperId, "developer_id");

                entity.HasIndex(e => e.Name, "name")
                    .IsUnique();

                entity.HasIndex(e => e.PublisherId, "publisher_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AvatarPath)
                    .HasMaxLength(1000)
                    .HasColumnName("avatar_path")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.DeveloperId)
                    .HasColumnType("int(11)")
                    .HasColumnName("developer_id")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(20,2)")
                    .HasColumnName("price");

                entity.Property(e => e.PublisherId)
                    .HasColumnType("int(11)")
                    .HasColumnName("publisher_id")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.ReleaseOn)
                    .HasColumnType("date")
                    .HasColumnName("release_on");

                entity.Property(e => e.VideoUrl)
                    .HasMaxLength(1000)
                    .HasColumnName("video_url")
                    .HasDefaultValueSql("NULL");

                entity.HasOne(d => d.Developer)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.DeveloperId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_game_developer_id");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_game_publisher_id");
            });

            modelBuilder.Entity<GameGenre>(entity =>
            {
                entity.HasKey(e => new { e.GameId, e.GenreId })
                    .HasName("PRIMARY");

                entity.ToTable("game_genre");
                
                entity.Property(e => e.GameId)
                    .HasColumnType("int(11)")
                    .HasColumnName("game_id");

                entity.Property(e => e.GenreId)
                    .HasColumnType("int(11)")
                    .HasColumnName("genre_id");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameGenres)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_game_genre_game_id");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.GameGenres)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK_game_genre_genre_id");
            });

            modelBuilder.Entity<GameOrder>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.GameId })
                    .HasName("PRIMARY");

                entity.ToTable("game_order");
                
                entity.Property(e => e.OrderId)
                    .HasColumnType("int(11)")
                    .HasColumnName("order_id");

                entity.Property(e => e.GameId)
                    .HasColumnType("int(11)")
                    .HasColumnName("game_id");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.GameOrders)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_game_order_order_id");
                
                //тут добавлял своё
                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameOrders)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_game_order_game_id");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genre");

                entity.HasIndex(e => e.Name, "name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Key>(entity =>
            {
                entity.ToTable("key");

                entity.HasIndex(e => e.GameId, "game_id");

                entity.HasIndex(e => e.ActivationId, "activation_id");

                entity.HasIndex(e => e.Value, "value")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.ActivationId)
                    .HasColumnType("int(11)")
                    .HasColumnName("activation_id")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.GameId)
                    .HasColumnType("int(11)")
                    .HasColumnName("game_id")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.IsUsed).HasColumnName("is_used");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("value");

                entity.HasOne(d => d.Activation)
                    .WithMany(p => p.Keys)
                    .HasForeignKey(d => d.ActivationId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_key_activation_id");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Keys)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_key_game_id");
            });

            modelBuilder.Entity<MinimumSpecification>(entity =>
            {
                entity.ToTable("minimum_specification");

                entity.HasIndex(e => e.GameId, "game_id");

                entity.HasIndex(e => new { e.PlatformId, e.GameId }, "platform_id_game_id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.GameId)
                    .HasColumnType("int(11)")
                    .HasColumnName("game_id");

                entity.Property(e => e.Graphics)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("graphics");

                entity.Property(e => e.Memory)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("memory");

                entity.Property(e => e.Os)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("os");

                entity.Property(e => e.PlatformId)
                    .HasColumnType("int(11)")
                    .HasColumnName("platform_id");

                entity.Property(e => e.Processor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("processor");

                entity.Property(e => e.Storage)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("storage");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.MinimumSpecifications)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_minimum_specification_game_id");

                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.MinimumSpecifications)
                    .HasForeignKey(d => d.PlatformId)
                    .HasConstraintName("FK_minimum_specification_platform_id");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("order");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(20,2)")
                    .HasColumnName("amount");

                entity.Property(e => e.PayOn)
                    .HasColumnType("date")
                    .HasColumnName("pay_on");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id")
                    .HasDefaultValueSql("NULL");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_order_user_id");
            });

            modelBuilder.Entity<Platform>(entity =>
            {
                entity.ToTable("platform");

                entity.HasIndex(e => e.Name, "name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.ToTable("publisher");

                entity.HasIndex(e => e.Name, "name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Balance)
                    .HasColumnType("decimal(20,2)")
                    .HasColumnName("balance");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("login");

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("mail");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("password");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
