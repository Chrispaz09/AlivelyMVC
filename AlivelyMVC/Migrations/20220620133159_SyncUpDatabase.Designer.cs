﻿// <auto-generated />
using System;
using AlivelyMVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AlivelyMVC.Migrations
{
    [DbContext(typeof(AlivelyDbContext))]
    [Migration("20220620133159_SyncUpDatabase")]
    partial class SyncUpDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AlivelyMVC.Models.SMARTGoal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("AchieveDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Completed")
                        .HasColumnType("bit");

                    b.Property<string>("Execution")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Measure")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Plan")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Target")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserUuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("SMARTGoals");
                });

            modelBuilder.Entity("AlivelyMVC.Models.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Completed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("Objective")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Relevance")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SMARTGoalId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SMARTGoalId");

                    b.ToTable("Task");
                });

            modelBuilder.Entity("AlivelyMVC.Models.Task", b =>
                {
                    b.HasOne("AlivelyMVC.Models.SMARTGoal", "SMARTGoal")
                        .WithMany("Tasks")
                        .HasForeignKey("SMARTGoalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SMARTGoal");
                });

            modelBuilder.Entity("AlivelyMVC.Models.SMARTGoal", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
