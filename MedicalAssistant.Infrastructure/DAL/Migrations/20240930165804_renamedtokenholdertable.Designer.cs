﻿// <auto-generated />
using System;
using System.Collections.Generic;
using MedicalAssistant.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MedicalAssistant.Infrastructure.Migrations
{
    [DbContext(typeof(MedicalAssistDbContext))]
    [Migration("20240930165804_renamedtokenholdertable")]
    partial class renamedtokenholdertable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MedicalAssistant.Domain.ComplexTypes.TokenHolder", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text")
                        .HasColumnName("RefreshToken");

                    b.Property<DateTime?>("RefreshTokenExpirationUtc")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("RefreshTokenExpirationUtc");

                    b.HasKey("UserId");

                    b.ToTable("TokenHolders");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.ExternalUserLogin", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("ProvidedKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Provider")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("ExternalUserLogin");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.NotificationHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("ContentJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("DateOfRead")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("UserId");

                    b.Property<bool>("WasRead")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("NotificationHistories");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.Recommendation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ExtraNote")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("VisitId")
                        .HasColumnType("uuid");

                    b.ComplexProperty<Dictionary<string, object>>("Medicine", "MedicalAssistant.Domain.Entites.Recommendation.Medicine#Medicine", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<int>("Quantity")
                                .HasColumnType("integer");

                            b1.Property<string>("TimeOfDay")
                                .IsRequired()
                                .HasColumnType("text");
                        });

                    b.HasKey("Id");

                    b.HasIndex("VisitId");

                    b.ToTable("Recommendations");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("HasExternalLoginProvider")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("Password")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.UserSettings", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<bool>("EmailNotificationEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("NotificationLanguage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("NotificationsEnabled")
                        .HasColumnType("boolean");

                    b.Property<bool>("VisitNotificationEnabled")
                        .HasColumnType("boolean");

                    b.HasKey("UserId");

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.UserVerification", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("CodeHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("UserId");

                    b.ToTable("UserVerifications");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.Visit", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DoctorName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<DateTime>("PredictedEndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("VisitDescription")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("VisitType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.ComplexProperty<Dictionary<string, object>>("Address", "MedicalAssistant.Domain.Entites.Visit.Address#Address", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasMaxLength(6)
                                .HasColumnType("character varying(6)");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("text");
                        });

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Visits");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.VisitNotification", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ScheduledDateUtc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("SimpleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("VisitId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("VisitId");

                    b.ToTable("VisitNotifications");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.ComplexTypes.TokenHolder", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entites.User", null)
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.ExternalUserLogin", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entites.User", null)
                        .WithOne("ExternalUserProvider")
                        .HasForeignKey("MedicalAssistant.Domain.Entites.ExternalUserLogin", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.NotificationHistory", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entites.User", "User")
                        .WithMany("NotificationHistories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.Recommendation", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entites.Visit", null)
                        .WithMany("Recommendations")
                        .HasForeignKey("VisitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.UserSettings", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entites.User", "User")
                        .WithOne("UserSettings")
                        .HasForeignKey("MedicalAssistant.Domain.Entites.UserSettings", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.UserVerification", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entites.User", null)
                        .WithOne("UserVerification")
                        .HasForeignKey("MedicalAssistant.Domain.Entites.UserVerification", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.Visit", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entites.User", null)
                        .WithMany("Visits")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.VisitNotification", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entites.Visit", null)
                        .WithMany("Notifications")
                        .HasForeignKey("VisitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.User", b =>
                {
                    b.Navigation("ExternalUserProvider");

                    b.Navigation("NotificationHistories");

                    b.Navigation("RefreshTokens");

                    b.Navigation("UserSettings")
                        .IsRequired();

                    b.Navigation("UserVerification");

                    b.Navigation("Visits");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entites.Visit", b =>
                {
                    b.Navigation("Notifications");

                    b.Navigation("Recommendations");
                });
#pragma warning restore 612, 618
        }
    }
}
