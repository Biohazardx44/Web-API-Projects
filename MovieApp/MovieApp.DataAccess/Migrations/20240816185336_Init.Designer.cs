﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieApp.DataAccess.Data;

#nullable disable

namespace MovieApp.DataAccess.Migrations
{
    [DbContext(typeof(MovieAppDbContext))]
    [Migration("20240816185336_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MovieApp.Domain.Models.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("Genre")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Movies");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "First Movie, Yay!",
                            Genre = 2,
                            Title = "Action Movie",
                            UserId = 1,
                            Year = 1994
                        },
                        new
                        {
                            Id = 2,
                            Description = "Second Movie, Yay!",
                            Genre = 3,
                            Title = "Thriller Movie",
                            UserId = 2,
                            Year = 1999
                        },
                        new
                        {
                            Id = 3,
                            Description = "Third Movie, Yay!",
                            Genre = 2,
                            Title = "Better Action Movie",
                            UserId = 1,
                            Year = 2021
                        },
                        new
                        {
                            Id = 4,
                            Description = "Last Movie, Yay!",
                            Genre = 1,
                            Title = "Comedy Movie",
                            UserId = 1,
                            Year = 1899
                        });
                });

            modelBuilder.Entity("MovieApp.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("FavoriteGenre")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FavoriteGenre = 2,
                            FirstName = "Eda",
                            LastName = "Nelson",
                            Password = "$??^R??|\"[u~{??",
                            Username = "user1"
                        },
                        new
                        {
                            Id = 2,
                            FavoriteGenre = 3,
                            FirstName = "John",
                            LastName = "Peterson",
                            Password = "~X?;`|?U?????7 ",
                            Username = "user2"
                        });
                });

            modelBuilder.Entity("MovieApp.Domain.Models.Movie", b =>
                {
                    b.HasOne("MovieApp.Domain.Models.User", "User")
                        .WithMany("Movies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MovieApp.Domain.Models.User", b =>
                {
                    b.Navigation("Movies");
                });
#pragma warning restore 612, 618
        }
    }
}
