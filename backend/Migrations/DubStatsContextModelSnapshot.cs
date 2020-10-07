﻿// <auto-generated />
using System;
using Backend.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Backend.Migrations
{
    [DbContext(typeof(DubStatsContext))]
    partial class DubStatsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8");

            modelBuilder.Entity("Backend.Database.CacheEntry", b =>
                {
                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(1073741824);

                    b.Property<DateTimeOffset>("NextFetch")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.HasKey("Type");

                    b.ToTable("CacheEntries");
                });

            modelBuilder.Entity("Backend.Database.Package", b =>
                {
                    b.Property<int>("PackageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.HasKey("PackageId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("Backend.Database.PackageStats", b =>
                {
                    b.Property<int>("PackageStatsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Downloads")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Forks")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasBeenModified")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Issues")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Stars")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.Property<int>("Watchers")
                        .HasColumnType("INTEGER");

                    b.HasKey("PackageStatsId");

                    b.ToTable("PackageStats");
                });

            modelBuilder.Entity("Backend.Database.PackageWeekInfo", b =>
                {
                    b.Property<int>("PackageWeekInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PackageId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PackageStatsAtEndId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PackageStatsAtStartId")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.Property<int>("WeekId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PackageWeekInfoId");

                    b.HasIndex("PackageStatsAtEndId");

                    b.HasIndex("PackageStatsAtStartId");

                    b.HasIndex("WeekId");

                    b.HasIndex("PackageId", "WeekId")
                        .IsUnique();

                    b.ToTable("WeekInfos");
                });

            modelBuilder.Entity("Backend.Database.ScheduledPackageUpdate", b =>
                {
                    b.Property<int>("ScheduledPackageUpdateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Milestone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PackageId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WeekId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ScheduledPackageUpdateId");

                    b.HasIndex("WeekId");

                    b.HasIndex("PackageId", "WeekId", "Milestone")
                        .IsUnique();

                    b.ToTable("PackageUpdates");
                });

            modelBuilder.Entity("Backend.Database.Week", b =>
                {
                    b.Property<int>("WeekId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("WeekEnd")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("WeekStart")
                        .HasColumnType("TEXT");

                    b.HasKey("WeekId");

                    b.HasIndex("WeekEnd")
                        .IsUnique();

                    b.HasIndex("WeekStart")
                        .IsUnique();

                    b.ToTable("Weeks");
                });

            modelBuilder.Entity("Backend.Database.PackageWeekInfo", b =>
                {
                    b.HasOne("Backend.Database.Package", "Package")
                        .WithMany("WeekInfos")
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Database.PackageStats", "PackageStatsAtEnd")
                        .WithMany()
                        .HasForeignKey("PackageStatsAtEndId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Database.PackageStats", "PackageStatsAtStart")
                        .WithMany()
                        .HasForeignKey("PackageStatsAtStartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Database.Week", "Week")
                        .WithMany("WeekInfos")
                        .HasForeignKey("WeekId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.Database.ScheduledPackageUpdate", b =>
                {
                    b.HasOne("Backend.Database.Package", "Package")
                        .WithMany()
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Database.Week", "Week")
                        .WithMany()
                        .HasForeignKey("WeekId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
