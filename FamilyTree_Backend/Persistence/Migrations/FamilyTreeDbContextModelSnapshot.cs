﻿// <auto-generated />
using System;
using FamilyTreeBackend.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FamilyTreeBackend.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(FamilyTreeDbContext))]
    partial class FamilyTreeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ApplicationUserFamilyTree", b =>
                {
                    b.Property<long>("EditorOfFamilyTreesId")
                        .HasColumnType("bigint");

                    b.Property<string>("EditorsId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("EditorOfFamilyTreesId", "EditorsId");

                    b.HasIndex("EditorsId");

                    b.ToTable("ApplicationUserFamilyTree");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MidName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.Family", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<long>("FamilyTreeId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("LastModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<long?>("Parent1Id")
                        .HasColumnType("bigint");

                    b.Property<long?>("Parent2Id")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("FamilyTreeId");

                    b.HasIndex("Parent1Id");

                    b.HasIndex("Parent2Id");

                    b.ToTable("Family");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.FamilyEvent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("FamilyTreeId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("LastModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ParentEventId")
                        .HasColumnType("bigint");

                    b.Property<int>("ReminderOffest")
                        .HasColumnType("int");

                    b.Property<int>("Repeat")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FamilyTreeId");

                    b.HasIndex("ParentEventId")
                        .IsUnique()
                        .HasFilter("[ParentEventId] IS NOT NULL");

                    b.ToTable("FamilyEvent");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.FamilyEventExceptionCase", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("FamilyEventId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("LastModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FamilyEventId");

                    b.ToTable("FamilyEventExceptionCases");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.FamilyMemory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("FamilyTreeId")
                        .HasColumnType("bigint");

                    b.Property<string>("ImageUrls")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("MemoryDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FamilyTreeId");

                    b.ToTable("FamilyMemory");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.FamilyTree", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("PublicMode")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("FamilyTree");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.Person", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("ChildOf")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateOfDeath")
                        .HasColumnType("datetime2");

                    b.Property<long>("FamilyTreeId")
                        .HasColumnType("bigint");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("HomeAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Occupation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ChildOf");

                    b.HasIndex("FamilyTreeId");

                    b.HasIndex("UserId");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.RefreshToken", b =>
                {
                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ExpiredDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Token");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.Relationship", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime?>("LastModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int>("RelationshipType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Relationship");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.Marriage", b =>
                {
                    b.HasBaseType("FamilyTreeBackend.Core.Domain.Entities.Relationship");

                    b.Property<DateTime>("DateOfMarriage")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndOfMarriage")
                        .HasColumnType("datetime2");

                    b.ToTable("Marriage");
                });

            modelBuilder.Entity("ApplicationUserFamilyTree", b =>
                {
                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.FamilyTree", null)
                        .WithMany()
                        .HasForeignKey("EditorOfFamilyTreesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("EditorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.Family", b =>
                {
                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.FamilyTree", "FamilyTree")
                        .WithMany("Families")
                        .HasForeignKey("FamilyTreeId")
                        .HasConstraintName("FK_FamilyOfTree")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.Person", "Parent1")
                        .WithMany()
                        .HasForeignKey("Parent1Id");

                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.Person", "Parent2")
                        .WithMany()
                        .HasForeignKey("Parent2Id");

                    b.Navigation("FamilyTree");

                    b.Navigation("Parent1");

                    b.Navigation("Parent2");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.FamilyEvent", b =>
                {
                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.FamilyTree", "FamilyTree")
                        .WithMany("Calendar")
                        .HasForeignKey("FamilyTreeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.FamilyEvent", "ParentEvent")
                        .WithOne("FollowingEvent")
                        .HasForeignKey("FamilyTreeBackend.Core.Domain.Entities.FamilyEvent", "ParentEventId");

                    b.Navigation("FamilyTree");

                    b.Navigation("ParentEvent");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.FamilyEventExceptionCase", b =>
                {
                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.FamilyEvent", "BaseFamilyEvent")
                        .WithMany("EventExceptions")
                        .HasForeignKey("FamilyEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BaseFamilyEvent");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.FamilyMemory", b =>
                {
                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.FamilyTree", null)
                        .WithMany("Memories")
                        .HasForeignKey("FamilyTreeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.FamilyTree", b =>
                {
                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .HasConstraintName("FK_OwnerOfTree")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.Notification", b =>
                {
                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.ApplicationUser", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.Person", b =>
                {
                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.Family", "ChildOfFamily")
                        .WithMany("Children")
                        .HasForeignKey("ChildOf")
                        .HasConstraintName("FK_Child_OfFamily")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.FamilyTree", "FamilyTree")
                        .WithMany("People")
                        .HasForeignKey("FamilyTreeId")
                        .HasConstraintName("FK_PersonOfTree")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.ApplicationUser", "ConnectedUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_ConnectedWith_User")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("ChildOfFamily");

                    b.Navigation("ConnectedUser");

                    b.Navigation("FamilyTree");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.RefreshToken", b =>
                {
                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_BelongsTo_User")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("User");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.Relationship", b =>
                {
                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.Family", "Family")
                        .WithOne("Relationship")
                        .HasForeignKey("FamilyTreeBackend.Core.Domain.Entities.Relationship", "Id")
                        .HasConstraintName("FK_Relationship_OfFamily")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Family");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.Marriage", b =>
                {
                    b.HasOne("FamilyTreeBackend.Core.Domain.Entities.Relationship", "ParentRelationship")
                        .WithOne()
                        .HasForeignKey("FamilyTreeBackend.Core.Domain.Entities.Marriage", "Id")
                        .HasConstraintName("FK_ParentRelationship_OfMarriage")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentRelationship");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.ApplicationUser", b =>
                {
                    b.Navigation("Notifications");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.Family", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("Relationship");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.FamilyEvent", b =>
                {
                    b.Navigation("EventExceptions");

                    b.Navigation("FollowingEvent");
                });

            modelBuilder.Entity("FamilyTreeBackend.Core.Domain.Entities.FamilyTree", b =>
                {
                    b.Navigation("Calendar");

                    b.Navigation("Families");

                    b.Navigation("Memories");

                    b.Navigation("People");
                });
#pragma warning restore 612, 618
        }
    }
}
