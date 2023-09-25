﻿// <auto-generated />
using System;
using GoFit.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GoFit.Infrastructure.Migrations
{
    [DbContext(typeof(GoFitDbContext))]
    partial class GoFitDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("GoFit.Domain.Entities.Exercise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Exercises", (string)null);
                });

            modelBuilder.Entity("GoFit.Domain.Entities.Workout", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ExerciseId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("WorkoutPlanId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("WorkoutPlanId");

                    b.ToTable("Workout", (string)null);
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutPlan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("WorkoutPlan", (string)null);
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutSet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxRepetitions")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MinRepetitions")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<float>("ResetTime")
                        .HasColumnType("REAL");

                    b.Property<bool>("UntilFailure")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("WarmUp")
                        .HasColumnType("INTEGER");

                    b.Property<float?>("Weight")
                        .HasColumnType("REAL");

                    b.Property<Guid>("WorkoutId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("WorkoutId");

                    b.ToTable("WorkoutSet", (string)null);
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutSetTracking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Repetitions")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Weight")
                        .HasColumnType("REAL");

                    b.Property<Guid>("WorkoutTrackingId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("WorkoutTrackingId");

                    b.ToTable("WorkoutSetTracking", (string)null);
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutTracking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndWorkoutDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartWorkoutDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("WorkoutId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("WorkoutId");

                    b.ToTable("WorkoutTracking", (string)null);
                });

            modelBuilder.Entity("GoFit.Domain.Entities.Workout", b =>
                {
                    b.HasOne("GoFit.Domain.Entities.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GoFit.Domain.Entities.WorkoutPlan", "WorkoutPlan")
                        .WithMany("Workouts")
                        .HasForeignKey("WorkoutPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("WorkoutPlan");
                });

            modelBuilder.Entity("GoFit.Domain.Entities.WorkoutSet", b =>
                {
                    b.HasOne("GoFit.Domain.Entities.Workout", "Workout")
                        .WithMany("Sets")
                        .HasForeignKey("WorkoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workout");
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

            modelBuilder.Entity("GoFit.Domain.Entities.Workout", b =>
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
