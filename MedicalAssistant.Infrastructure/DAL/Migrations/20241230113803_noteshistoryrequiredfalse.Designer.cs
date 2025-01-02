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

namespace MedicalAssistant.Infrastructure.DAL.Migrations
{
    [DbContext(typeof(MedicalAssistantDbContext))]
    [Migration("20241230113803_noteshistoryrequiredfalse")]
    partial class noteshistoryrequiredfalse
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
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text")
                        .HasColumnName("RefreshToken");

                    b.Property<DateTime?>("RefreshTokenExpirationUtc")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("RefreshTokenExpirationUtc");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TokenHolders");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.Attachment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasMaxLength(10485760)
                        .HasColumnType("bytea");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("VisitId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("VisitId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.DiseaseStage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("MedicalHistoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("character varying(35)");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<Guid?>("VisitId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("MedicalHistoryId");

                    b.HasIndex("VisitId");

                    b.ToTable("DiseaseStages");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.ExternalUserLogin", b =>
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

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.MedicalHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DiseaseEndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DiseaseName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("DiseaseStartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<string>("SymptomDescription")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("VisitId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VisitId");

                    b.HasIndex("DiseaseName", "SymptomDescription")
                        .HasAnnotation("Npgsql:TsVectorConfig", "english");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("DiseaseName", "SymptomDescription"), "GIN");

                    b.ToTable("MedicalHistories");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.MedicalNote", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string[]>("Tags")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Note")
                        .HasAnnotation("Npgsql:TsVectorConfig", "english");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("Note"), "GIN");

                    b.HasIndex("UserId");

                    b.ToTable("MedicalNotes");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.MedicationRecommendation", b =>
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

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("VisitId")
                        .HasColumnType("uuid");

                    b.ComplexProperty<Dictionary<string, object>>("Medicine", "MedicalAssistant.Domain.Entities.MedicationRecommendation.Medicine#Medicine", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<int>("Quantity")
                                .HasColumnType("integer");

                            b1.Property<string[]>("TimeOfDay")
                                .IsRequired()
                                .HasColumnType("text[]");
                        });

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VisitId");

                    b.ToTable("MedicationRecommendation", (string)null);
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.MedicationRecommendationNotification", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("End")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("JobId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("MedicationRecommendationId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp without time zone");

                    b.Property<TimeSpan>("TriggerTimeUtc")
                        .HasColumnType("TIME");

                    b.HasKey("Id");

                    b.HasIndex("MedicationRecommendationId");

                    b.ToTable("MedicationRecommendationsNotifications");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.NotificationHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("ContentJson")
                        .IsRequired()
                        .HasColumnType("jsonb");

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

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.RecommendationUsage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("MedicationRecommendationId")
                        .HasColumnType("uuid");

                    b.Property<string>("TimeOfDay")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.HasKey("Id");

                    b.HasIndex("MedicationRecommendationId");

                    b.ToTable("RecommendationUsages");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DateOfDeactivationUtc")
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

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<bool>("IsVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.UserSettings", b =>
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

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.UserVerification", b =>
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

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.Visit", b =>
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

                    b.ComplexProperty<Dictionary<string, object>>("Address", "MedicalAssistant.Domain.Entities.Visit.Address#Address", b1 =>
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

                    b.HasIndex("VisitType", "VisitDescription", "DoctorName")
                        .HasAnnotation("Npgsql:TsVectorConfig", "english");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("VisitType", "VisitDescription", "DoctorName"), "GIN");

                    b.ToTable("Visits");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.VisitNotification", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("JobId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ScheduledDateUtc")
                        .HasColumnType("timestamp without time zone");

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
                    b.HasOne("MedicalAssistant.Domain.Entities.User", null)
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.Attachment", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entities.Visit", null)
                        .WithMany("Attachments")
                        .HasForeignKey("VisitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.DiseaseStage", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entities.MedicalHistory", null)
                        .WithMany("DiseaseStages")
                        .HasForeignKey("MedicalHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicalAssistant.Domain.Entities.Visit", "Visit")
                        .WithMany("DiseaseStages")
                        .HasForeignKey("VisitId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Visit");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.ExternalUserLogin", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entities.User", null)
                        .WithOne("ExternalUserProvider")
                        .HasForeignKey("MedicalAssistant.Domain.Entities.ExternalUserLogin", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.MedicalHistory", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entities.User", "User")
                        .WithMany("MedicalHistories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicalAssistant.Domain.Entities.Visit", "Visit")
                        .WithMany("MedicalHistories")
                        .HasForeignKey("VisitId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("User");

                    b.Navigation("Visit");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.MedicalNote", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entities.User", "User")
                        .WithMany("MedicalNotes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.MedicationRecommendation", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entities.User", "User")
                        .WithMany("MedicationRecommendations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicalAssistant.Domain.Entities.Visit", "Visit")
                        .WithMany("Recommendations")
                        .HasForeignKey("VisitId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");

                    b.Navigation("Visit");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.MedicationRecommendationNotification", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entities.MedicationRecommendation", null)
                        .WithMany("Notifications")
                        .HasForeignKey("MedicationRecommendationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.NotificationHistory", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entities.User", "User")
                        .WithMany("NotificationHistories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.RecommendationUsage", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entities.MedicationRecommendation", "MedicationRecommendation")
                        .WithMany("RecommendationUsages")
                        .HasForeignKey("MedicationRecommendationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicationRecommendation");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.UserSettings", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entities.User", "User")
                        .WithOne("UserSettings")
                        .HasForeignKey("MedicalAssistant.Domain.Entities.UserSettings", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.UserVerification", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entities.User", null)
                        .WithOne("UserVerification")
                        .HasForeignKey("MedicalAssistant.Domain.Entities.UserVerification", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.Visit", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entities.User", null)
                        .WithMany("Visits")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.VisitNotification", b =>
                {
                    b.HasOne("MedicalAssistant.Domain.Entities.Visit", null)
                        .WithMany("Notifications")
                        .HasForeignKey("VisitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.MedicalHistory", b =>
                {
                    b.Navigation("DiseaseStages");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.MedicationRecommendation", b =>
                {
                    b.Navigation("Notifications");

                    b.Navigation("RecommendationUsages");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.User", b =>
                {
                    b.Navigation("ExternalUserProvider");

                    b.Navigation("MedicalHistories");

                    b.Navigation("MedicalNotes");

                    b.Navigation("MedicationRecommendations");

                    b.Navigation("NotificationHistories");

                    b.Navigation("RefreshTokens");

                    b.Navigation("UserSettings")
                        .IsRequired();

                    b.Navigation("UserVerification");

                    b.Navigation("Visits");
                });

            modelBuilder.Entity("MedicalAssistant.Domain.Entities.Visit", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("DiseaseStages");

                    b.Navigation("MedicalHistories");

                    b.Navigation("Notifications");

                    b.Navigation("Recommendations");
                });
#pragma warning restore 612, 618
        }
    }
}