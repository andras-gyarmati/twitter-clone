﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwitterClone;

#nullable disable

namespace TwitterClone.Migrations
{
    [DbContext(typeof(TwitterCloneDbContext))]
    partial class TwitterCloneDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("TwitterClone.Models.Tweet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasMaxLength(280)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int?>("InReplyToId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("InReplyToId");

                    b.ToTable("Tweet");
                });

            modelBuilder.Entity("TwitterClone.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bio")
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProfilePicture")
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("TwitterClone.Models.UserUser", b =>
                {
                    b.Property<int>("FollowedId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FollowerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("FollowDate")
                        .HasColumnType("TEXT");

                    b.HasKey("FollowedId", "FollowerId");

                    b.HasIndex("FollowerId");

                    b.ToTable("UserUser");
                });

            modelBuilder.Entity("TwitterClone.Models.Tweet", b =>
                {
                    b.HasOne("TwitterClone.Models.User", "Author")
                        .WithMany("Tweets")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TwitterClone.Models.Tweet", "InReplyTo")
                        .WithMany("Replies")
                        .HasForeignKey("InReplyToId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Author");

                    b.Navigation("InReplyTo");
                });

            modelBuilder.Entity("TwitterClone.Models.UserUser", b =>
                {
                    b.HasOne("TwitterClone.Models.User", "Followed")
                        .WithMany("Followers")
                        .HasForeignKey("FollowedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TwitterClone.Models.User", "Follower")
                        .WithMany("Following")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Followed");

                    b.Navigation("Follower");
                });

            modelBuilder.Entity("TwitterClone.Models.Tweet", b =>
                {
                    b.Navigation("Replies");
                });

            modelBuilder.Entity("TwitterClone.Models.User", b =>
                {
                    b.Navigation("Followers");

                    b.Navigation("Following");

                    b.Navigation("Tweets");
                });
#pragma warning restore 612, 618
        }
    }
}
