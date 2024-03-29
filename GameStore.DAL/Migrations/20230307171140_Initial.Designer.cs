﻿// <auto-generated />
using System;
using GameStore.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GameStore.DAL.Migrations
{
    [DbContext(typeof(GamestoredbContext))]
    [Migration("20230307171140_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GameStore.Domain.Models.Activation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Name" }, "name")
                        .IsUnique();

                    b.ToTable("activation", (string)null);
                });

            modelBuilder.Entity("GameStore.Domain.Models.Developer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Name" }, "name")
                        .IsUnique()
                        .HasDatabaseName("name1");

                    b.ToTable("developer", (string)null);
                });

            modelBuilder.Entity("GameStore.Domain.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AvatarPath")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)")
                        .HasColumnName("avatar_path")
                        .HasDefaultValueSql("NULL");

                    b.Property<string>("Description")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description")
                        .HasDefaultValueSql("NULL");

                    b.Property<int?>("DeveloperId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("developer_id")
                        .HasDefaultValueSql("NULL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(20,2)")
                        .HasColumnName("price");

                    b.Property<int?>("PublisherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("publisher_id")
                        .HasDefaultValueSql("NULL");

                    b.Property<DateTime>("ReleaseOn")
                        .HasColumnType("date")
                        .HasColumnName("release_on");

                    b.Property<string>("VideoUrl")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)")
                        .HasColumnName("video_url")
                        .HasDefaultValueSql("NULL");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "DeveloperId" }, "developer_id");

                    b.HasIndex(new[] { "Name" }, "name")
                        .IsUnique()
                        .HasDatabaseName("name2");

                    b.HasIndex(new[] { "PublisherId" }, "publisher_id");

                    b.ToTable("game", (string)null);
                });

            modelBuilder.Entity("GameStore.Domain.Models.GameGenre", b =>
                {
                    b.Property<int>("GameId")
                        .HasColumnType("int")
                        .HasColumnName("game_id");

                    b.Property<int>("GenreId")
                        .HasColumnType("int")
                        .HasColumnName("genre_id");

                    b.HasKey("GameId", "GenreId")
                        .HasName("Primary_Game_Genre");

                    b.HasIndex("GenreId");

                    b.ToTable("game_genre", (string)null);
                });

            modelBuilder.Entity("GameStore.Domain.Models.GameMinSpecification", b =>
                {
                    b.Property<int>("GameId")
                        .HasColumnType("int")
                        .HasColumnName("game_id");

                    b.Property<int>("MinimumSpecificationId")
                        .HasColumnType("int")
                        .HasColumnName("min_spec_id");

                    b.HasKey("GameId", "MinimumSpecificationId")
                        .HasName("Primary_Game_Min_Spec");

                    b.HasIndex("MinimumSpecificationId");

                    b.ToTable("game_min_spec", (string)null);
                });

            modelBuilder.Entity("GameStore.Domain.Models.GameOrder", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("int")
                        .HasColumnName("order_id");

                    b.Property<int>("GameId")
                        .HasColumnType("int")
                        .HasColumnName("game_id");

                    b.HasKey("OrderId", "GameId")
                        .HasName("Primary_Game_Order");

                    b.HasIndex("GameId");

                    b.ToTable("game_order", (string)null);
                });

            modelBuilder.Entity("GameStore.Domain.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Name" }, "name")
                        .IsUnique()
                        .HasDatabaseName("name3");

                    b.ToTable("genre", (string)null);
                });

            modelBuilder.Entity("GameStore.Domain.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("GameId")
                        .HasColumnType("int")
                        .HasColumnName("game_id");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("path");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "GameId" }, "game_id");

                    b.ToTable("image", (string)null);
                });

            modelBuilder.Entity("GameStore.Domain.Models.Key", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("ActivationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("activation_id")
                        .HasDefaultValueSql("NULL");

                    b.Property<int?>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("game_id")
                        .HasDefaultValueSql("NULL");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit")
                        .HasColumnName("is_used");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ActivationId" }, "activation_id");

                    b.HasIndex(new[] { "GameId" }, "game_id")
                        .HasDatabaseName("game_id1");

                    b.HasIndex(new[] { "Value" }, "value")
                        .IsUnique();

                    b.ToTable("key", (string)null);
                });

            modelBuilder.Entity("GameStore.Domain.Models.MinimumSpecification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Graphics")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("graphics");

                    b.Property<string>("Memory")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("memory");

                    b.Property<string>("OperatingSystem")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("operating_system");

                    b.Property<int>("PlatformId")
                        .HasColumnType("int")
                        .HasColumnName("platform_id");

                    b.Property<string>("Processor")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("processor");

                    b.Property<string>("Storage")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("storage");

                    b.HasKey("Id");

                    b.HasIndex("PlatformId");

                    b.ToTable("minimum_specification", (string)null);
                });

            modelBuilder.Entity("GameStore.Domain.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(20,2)")
                        .HasColumnName("amount");

                    b.Property<DateTime>("PayOn")
                        .HasColumnType("date")
                        .HasColumnName("pay_on");

                    b.Property<int?>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("user_id")
                        .HasDefaultValueSql("NULL");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "UserId" }, "user_id");

                    b.ToTable("order", (string)null);
                });

            modelBuilder.Entity("GameStore.Domain.Models.Platform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Name" }, "name")
                        .IsUnique()
                        .HasDatabaseName("name4");

                    b.ToTable("platform", (string)null);
                });

            modelBuilder.Entity("GameStore.Domain.Models.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Name" }, "name")
                        .IsUnique()
                        .HasDatabaseName("name5");

                    b.ToTable("publisher", (string)null);
                });

            modelBuilder.Entity("GameStore.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(20,2)")
                        .HasColumnName("balance");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("login");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("mail");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("password");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("GameStore.Domain.Models.Game", b =>
                {
                    b.HasOne("GameStore.Domain.Models.Developer", "Developer")
                        .WithMany("Games")
                        .HasForeignKey("DeveloperId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_game_developer_id");

                    b.HasOne("GameStore.Domain.Models.Publisher", "Publisher")
                        .WithMany("Games")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_game_publisher_id");

                    b.Navigation("Developer");

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("GameStore.Domain.Models.GameGenre", b =>
                {
                    b.HasOne("GameStore.Domain.Models.Game", "Game")
                        .WithMany("GameGenres")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_game_genre_game_id");

                    b.HasOne("GameStore.Domain.Models.Genre", "Genre")
                        .WithMany("GameGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_game_genre_genre_id");

                    b.Navigation("Game");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("GameStore.Domain.Models.GameMinSpecification", b =>
                {
                    b.HasOne("GameStore.Domain.Models.Game", "Game")
                        .WithMany("GameMinSpecifications")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_game_min_spec_game_id");

                    b.HasOne("GameStore.Domain.Models.MinimumSpecification", "MinimumSpecification")
                        .WithMany("GameMinSpecification")
                        .HasForeignKey("MinimumSpecificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_game_min_spec_min_spec_id");

                    b.Navigation("Game");

                    b.Navigation("MinimumSpecification");
                });

            modelBuilder.Entity("GameStore.Domain.Models.GameOrder", b =>
                {
                    b.HasOne("GameStore.Domain.Models.Game", "Game")
                        .WithMany("GameOrders")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_game_order_game_id");

                    b.HasOne("GameStore.Domain.Models.Order", "Order")
                        .WithMany("GameOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_game_order_order_id");

                    b.Navigation("Game");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("GameStore.Domain.Models.Image", b =>
                {
                    b.HasOne("GameStore.Domain.Models.Game", "Game")
                        .WithMany("Images")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_image_game_id");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("GameStore.Domain.Models.Key", b =>
                {
                    b.HasOne("GameStore.Domain.Models.Activation", "Activation")
                        .WithMany("Keys")
                        .HasForeignKey("ActivationId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_key_activation_id");

                    b.HasOne("GameStore.Domain.Models.Game", "Game")
                        .WithMany("Keys")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_key_game_id");

                    b.Navigation("Activation");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("GameStore.Domain.Models.MinimumSpecification", b =>
                {
                    b.HasOne("GameStore.Domain.Models.Platform", "Platform")
                        .WithMany("MinimumSpecifications")
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_minimum_specification_platform_id");

                    b.Navigation("Platform");
                });

            modelBuilder.Entity("GameStore.Domain.Models.Order", b =>
                {
                    b.HasOne("GameStore.Domain.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_order_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GameStore.Domain.Models.Activation", b =>
                {
                    b.Navigation("Keys");
                });

            modelBuilder.Entity("GameStore.Domain.Models.Developer", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("GameStore.Domain.Models.Game", b =>
                {
                    b.Navigation("GameGenres");

                    b.Navigation("GameMinSpecifications");

                    b.Navigation("GameOrders");

                    b.Navigation("Images");

                    b.Navigation("Keys");
                });

            modelBuilder.Entity("GameStore.Domain.Models.Genre", b =>
                {
                    b.Navigation("GameGenres");
                });

            modelBuilder.Entity("GameStore.Domain.Models.MinimumSpecification", b =>
                {
                    b.Navigation("GameMinSpecification");
                });

            modelBuilder.Entity("GameStore.Domain.Models.Order", b =>
                {
                    b.Navigation("GameOrders");
                });

            modelBuilder.Entity("GameStore.Domain.Models.Platform", b =>
                {
                    b.Navigation("MinimumSpecifications");
                });

            modelBuilder.Entity("GameStore.Domain.Models.Publisher", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("GameStore.Domain.Models.User", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
