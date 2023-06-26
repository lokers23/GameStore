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
        public virtual DbSet<KeyOrder> KeyOrders { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Key> Keys { get; set; }
        public virtual DbSet<MinimumSpecification> MinimumSpecifications { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Image> Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
             if (!optionsBuilder.IsConfigured)
             {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                
                //optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=gamestoredb;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;");
                optionsBuilder.UseSqlServer("Data Source=SQL6031.site4now.net;Initial Catalog=db_a99e61_lokers;User Id=db_a99e61_lokers_admin;Password=159487159487aA"
                );
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
                    .HasColumnType("int")
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
                    .HasColumnType("int")
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

                entity.HasIndex(e => e.Name, "name");

                entity.HasIndex(e => e.PublisherId, "publisher_id");
                entity.HasIndex(e => e.ActivationId, "activation_id");
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .HasColumnName("id");

                entity.Property(e => e.AvatarName)
                    .HasMaxLength(1000)
                    .HasColumnName("avatar_name")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.DeveloperId)
                    .HasColumnType("int")
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
                    .HasColumnType("int")
                    .HasColumnName("publisher_id")
                    .HasDefaultValueSql("NULL");

                
                entity.Property(e => e.ActivationId)
                    .HasColumnType("int")
                    .HasColumnName("activation_id")
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

                
                entity.HasOne(d => d.Activation)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.ActivationId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_game_activation_id");
            });

            modelBuilder.Entity<GameGenre>(entity =>
            {
                entity.HasKey(e => new { e.GameId, e.GenreId })
                    .HasName("Primary_Game_Genre");

                entity.ToTable("game_genre");
                
                entity.Property(e => e.GameId)
                    .HasColumnType("int")
                    .HasColumnName("game_id");

                entity.Property(e => e.GenreId)
                    .HasColumnType("int")
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
            
            modelBuilder.Entity<GameMinSpecification>(entity =>
            {
                entity.HasKey(e => new { e.GameId, e.MinimumSpecificationId })
                    .HasName("Primary_Game_Min_Spec");

                entity.ToTable("game_min_spec");
                
                entity.Property(e => e.GameId)
                    .HasColumnType("int")
                    .HasColumnName("game_id");

                entity.Property(e => e.MinimumSpecificationId)
                    .HasColumnType("int")
                    .HasColumnName("min_spec_id");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameMinSpecifications)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_game_min_spec_game_id");

                entity.HasOne(d => d.MinimumSpecification)
                    .WithMany(p => p.GameMinSpecification)
                    .HasForeignKey(d => d.MinimumSpecificationId)
                    .HasConstraintName("FK_game_min_spec_min_spec_id");
            });

            modelBuilder.Entity<KeyOrder>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.KeyId })
                    .HasName("Primary_Key_Order");

                entity.ToTable("key_order");

                entity.Property(e => e.OrderId)
                    .HasColumnType("int")
                    .HasColumnName("order_id");

                entity.Property(e => e.KeyId)
                    .HasColumnType("int")
                    .HasColumnName("key_id");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.KeyOrders)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_key_order_order_id");

                entity.HasOne(d => d.Key)
                    .WithMany(p => p.KeyOrders)
                    .HasForeignKey(d => d.KeyId)
                    .HasConstraintName("FK_key_order_key_id");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genre");

                entity.HasIndex(e => e.Name, "name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int")
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

                entity.HasIndex(e => e.Value, "value")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .HasColumnName("id");

                entity.Property(e => e.GameId)
                    .HasColumnType("int")
                    .HasColumnName("game_id")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.IsUsed).HasColumnName("is_used");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("value");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Keys)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_key_game_id");
            });

            modelBuilder.Entity<MinimumSpecification>(entity =>
            {
                entity.ToTable("minimum_specification");

                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .HasColumnName("id");

                entity.Property(e => e.Graphics)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("graphics");

                entity.Property(e => e.Memory)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("memory");

                entity.Property(e => e.OperatingSystem)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("operating_system");

                entity.Property(e => e.PlatformId)
                    .HasColumnType("int")
                    .HasColumnName("platform_id");

                entity.Property(e => e.Processor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("processor");

                entity.Property(e => e.Storage)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("storage");
                
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
                    .HasColumnType("int")
                    .HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(20,2)")
                    .HasColumnName("amount");

                entity.Property(e => e.PayOn)
                    .HasColumnType("date")
                    .HasColumnName("pay_on");

                entity.Property(e => e.UserId)
                    .HasColumnType("int")
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
                    .HasColumnType("int")
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
                    .HasColumnType("int")
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
                    .HasColumnType("int")
                    .HasColumnName("id");

                entity.Property(e => e.Role)
                    .HasConversion<int>();

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
                    .HasMaxLength(255)
                    .HasColumnName("password");
            });
            
            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("image");

                entity.HasIndex(e => e.GameId, "game_id");
                
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");
                
                entity.Property(e => e.GameId)
                    .IsRequired()
                    .HasColumnType("int")
                    .HasColumnName("game_id");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_image_game_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
