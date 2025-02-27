﻿// <auto-generated />
using System;
using GoFit.Infrastructure.Contexts.GoFitDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GoFit.Infrastructure.Contexts.GoFitDb.Migrations
{
    [DbContext(typeof(GoFitDbContext))]
    [Migration("20250126180028_InitialCreateGoFitDb")]
    partial class InitialCreateGoFitDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GoFit.Domain.Entities.Athlete", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Athletes", (string)null);
                });

            modelBuilder.Entity("GoFit.Domain.Entities.Exercise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Exercises", (string)null);
                });

            modelBuilder.Entity("GoFit.Domain.Entities.Workout", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<Guid>("WorkoutPlanId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("WorkoutPlanId");

                    b.ToTable("Workouts", (string)null);
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutExercise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ExerciseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<Guid>("WorkoutId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("WorkoutId");

                    b.ToTable("WorkoutExercises", (string)null);
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutPlan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AthleteId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("AthleteId");

                    b.ToTable("WorkoutsPlan", (string)null);
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutSet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MaxRepetitions")
                        .HasColumnType("int");

                    b.Property<int>("MinRepetitions")
                        .HasColumnType("int");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<float>("ResetTime")
                        .HasColumnType("real");

                    b.Property<bool>("UntilFailure")
                        .HasColumnType("bit");

                    b.Property<bool>("WarmUp")
                        .HasColumnType("bit");

                    b.Property<float?>("Weight")
                        .HasColumnType("real");

                    b.Property<Guid>("WorkoutExerciseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("WorkoutExerciseId");

                    b.ToTable("WorkoutSets", (string)null);
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutSetTracking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<int>("Repetitions")
                        .HasColumnType("int");

                    b.Property<float>("Weight")
                        .HasColumnType("real");

                    b.Property<Guid>("WorkoutTrackingId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("WorkoutTrackingId");

                    b.ToTable("WorkoutSetsTracking", (string)null);
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutTracking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("EndWorkoutDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartWorkoutDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("WorkoutId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("WorkoutId");

                    b.ToTable("WorkoutsTracking", (string)null);
                });

            modelBuilder.Entity("GoFit.Domain.Entities.Workout", b =>
                {
                    b.HasOne("GoFit.Domain.Entities.WorkoutPlan", "WorkoutPlan")
                        .WithMany("Workouts")
                        .HasForeignKey("WorkoutPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkoutPlan");
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutExercise", b =>
                {
                    b.HasOne("GoFit.Domain.Entities.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GoFit.Domain.Entities.Workout", "Workout")
                        .WithMany("WorkoutExercises")
                        .HasForeignKey("WorkoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("Workout");
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutPlan", b =>
                {
                    b.HasOne("GoFit.Domain.Entities.Athlete", "Athlete")
                        .WithMany("WorkoutPlans")
                        .HasForeignKey("AthleteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Athlete");
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutSet", b =>
                {
                    b.HasOne("GoFit.Domain.Entities.WorkoutExercise", "WorkoutExercise")
                        .WithMany("Sets")
                        .HasForeignKey("WorkoutExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkoutExercise");
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutSetTracking", b =>
                {
                    b.HasOne("GoFit.Domain.Entities.WorkoutTracking", "WorkoutTracking")
                        .WithMany("Sets")
                        .HasForeignKey("WorkoutTrackingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkoutTracking");
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutTracking", b =>
                {
                    b.HasOne("GoFit.Domain.Entities.Workout", "Workout")
                        .WithMany()
                        .HasForeignKey("WorkoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workout");
                });

            modelBuilder.Entity("GoFit.Domain.Entities.Athlete", b =>
                {
                    b.Navigation("WorkoutPlans");
                });

            modelBuilder.Entity("GoFit.Domain.Entities.Workout", b =>
                {
                    b.Navigation("WorkoutExercises");
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutExercise", b =>
                {
                    b.Navigation("Sets");
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutPlan", b =>
                {
                    b.Navigation("Workouts");
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutTracking", b =>
                {
                    b.Navigation("Sets");
                });
#pragma warning restore 612, 618
        }
    }
}
