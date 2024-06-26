﻿using FamilyTreeBackend.Core.Application.Helpers;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Core.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Persistence.Context
{
    public partial class FamilyTreeDbContext : IdentityDbContext<ApplicationUser>
    {
        public FamilyTreeDbContext(DbContextOptions<FamilyTreeDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Family> Families { get; set; }
        public virtual DbSet<FamilyTree> FamilyTrees { get; set; }
        public virtual DbSet<FamilyEvent> FamilyEvents { get; set; }
        public virtual DbSet<FamilyMemory> FamilyMemories { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Relationship> Relationships { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<UserConnection> UserConnections { get; set; }
        public virtual DbSet<RequestResponseLog> RequestResponseLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>((entity) =>
            {
                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.UpdatedDate)
                    .HasDefaultValueSql("GETUTCDATE()")
                   .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<RefreshToken>((entity) =>
            {
                entity.ToTable("RefreshToken");

                entity.HasKey(e => e.Token);

                entity.Property(e => e.Token).ValueGeneratedNever();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .HasConstraintName("FK_BelongsTo_User")
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Family>((entity) =>
            {
                entity.ToTable("Family");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.DateCreated)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastModified)
                    .HasDefaultValueSql("GETUTCDATE()")
                   .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(e => e.Parent1)
                    .WithMany()
                    .HasForeignKey(e => e.Parent1Id);

                entity.HasOne(e => e.Parent2)
                    .WithMany()
                    .HasForeignKey(e => e.Parent2Id);

                entity.HasOne(e => e.Relationship)
                    .WithOne(e => e.Family);

                entity.HasOne(e => e.FamilyTree)
                    .WithMany(f => f.Families)
                    .HasForeignKey(e => e.FamilyTreeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Person>((entity) =>
            {
                entity.ToTable("Person");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.DateCreated)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastModified)
                    .HasDefaultValueSql("GETUTCDATE()")
                   .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(e => e.ChildOfFamily)
                    .WithMany(f => f.Children)
                    .HasForeignKey(e => e.ChildOf)
                    .HasConstraintName("FK_Child_OfFamily")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ConnectedUser)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .HasConstraintName("FK_ConnectedWith_User")
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.FamilyTree)
                    .WithMany(f => f.People)
                    .HasForeignKey(e => e.FamilyTreeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Relationship>((entity) =>
            {
                entity.ToTable("Relationship");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id);

                entity.Property(e => e.DateCreated)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastModified)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(e => e.Family)
                    .WithOne(f => f.Relationship)
                    .HasForeignKey<Relationship>(e => e.Id)
                    .HasConstraintName("FK_Relationship_OfFamily")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<FamilyTree>((entity) => {
                entity.ToTable("FamilyTree");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.DateCreated)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastModified)
                    .HasDefaultValueSql("GETUTCDATE()")
                   .ValueGeneratedOnAddOrUpdate();

                entity.HasMany(e => e.People)
                    .WithOne(e => e.FamilyTree)
                    .HasForeignKey(e => e.FamilyTreeId)
                    .HasConstraintName("FK_PersonOfTree");

                entity.HasMany(e => e.Families)
                    .WithOne(e => e.FamilyTree)
                    .HasForeignKey(e => e.FamilyTreeId)
                    .HasConstraintName("FK_FamilyOfTree");

                entity.HasMany(e => e.Editors)
                    .WithMany(e => e.EditorOfFamilyTrees);

                entity.HasOne(e => e.Owner)
                    .WithMany()
                    .HasForeignKey(e => e.OwnerId)
                    .HasConstraintName("FK_OwnerOfTree")
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(e => e.Calendar)
                    .WithOne(e => e.FamilyTree)
                    .HasForeignKey(e => e.FamilyTreeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Memories)
                    .WithOne()
                    .HasForeignKey(e => e.FamilyTreeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<FamilyEvent>((entity) =>
            {
                entity.ToTable("FamilyEvent");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Repeat).HasDefaultValue(RepeatEvent.NONE);

                entity.HasOne(e => e.ParentEvent)
                    .WithOne(e => e.FollowingEvent)
                    .HasForeignKey<FamilyEvent>(e => e.ParentEventId);

                entity.HasMany(e => e.EventExceptions)
                    .WithOne(e => e.BaseFamilyEvent)
                    .HasForeignKey(e => e.FamilyEventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.DateCreated)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastModified)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<FamilyEventExceptionCase>((entity) =>
            {
                entity.ToTable("FamilyEventExceptionCases");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.IsRescheduled).HasDefaultValue(false);
                entity.Property(e => e.IsCancelled).HasDefaultValue(false);

                entity.Property(e => e.DateCreated)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastModified)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<FamilyMemory>((entity) =>
            {
                entity.ToTable("FamilyMemory");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                var valueComparer = new ValueComparer<ICollection<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => (ICollection<string>)c.ToHashSet());

                entity.Property(p => p.ImageUrls)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<ICollection<string>>(v, null))
                .Metadata.SetValueComparer(valueComparer);

                entity.Property(e => e.DateCreated)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastModified)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(e => e.CreatedBy)
                    .WithMany()
                    .HasForeignKey(entity => entity.CreatedByUserID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.FamilyTree)
                    .WithMany(e => e.Memories)
                    .HasForeignKey(e => e.FamilyTreeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Notification>((entity) =>
            {
                entity.ToTable("Notifications");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.DateCreated)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastModified)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<RequestResponseLog>((entity) => {
                entity.ToTable("RequestResponseLogs");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.DateCreated)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UserConnection>(e => {
                e.HasNoKey().ToView(null);
                e.ToTable("UserConnections");
            });
        }

        public void SeedData()
        {

            var exists = FamilyTrees.Any();

            if (exists)
            {
                return;
            }

            FamilyTree familyTree = new FamilyTree()
            {
                Name = "Hung Thi Family Tree",
                Description = "Hung Thi Test Family Tree",
                Families = new List<Family>(),
                People = new List<Person>(),
            };

            FamilyTrees.Add(familyTree);

            Person person = new Person
            {
                FirstName = "Hung",
                LastName = "Thi",
                DateOfBirth = DateTime.Today,
                Gender = Gender.MALE,
            };

            Person father = new Person
            {
                FirstName = "Senh",
                LastName = "Thi",
                DateOfBirth = new DateTime(1945, 07, 20),
                Gender = Gender.MALE,
            };

            Person mother = new Person
            {
                FirstName = "Phan",
                LastName = "Luan",
                DateOfBirth = new DateTime(1954, 10, 20),
                Gender = Gender.FEMALE,
            };

            familyTree.People.Add(person);
            familyTree.People.Add(mother);
            familyTree.People.Add(father);

            People.AddRange(person, father, mother);

            SaveChanges();

            Family family = new Family
            {
                Parent1 = father,
                Parent2 = mother,
                Children = new List<Person>(),
            };

            family.Children.Add(person);

            Relationship relationship = new Relationship
            {
                RelationshipType = RelationshipType.MARRIED,
                StartDate = new DateTime(1985, 10, 11),
            };

            family.Relationship = relationship;

            familyTree.Families.Add(family);


            SaveChanges();
        }

        public async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var logger = Log.Logger;

            if (!roleManager.Roles.Any())
            {
                logger.Information("Seeding user roles...");
                await roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.Admin));
                await roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.User));
                logger.Information("Seeding complete for user roles...");
            }

            if (!userManager.Users.Any())
            {
                logger.Information("Seeding test user...");
                //Seed Default User
                var defaultUser = new ApplicationUser
                {
                    UserName = "test",
                    FirstName = "test",
                    LastName = "user",
                    Email = "test@family-tree.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0123434552",
                    PhoneNumberConfirmed = true,
                    Status = true,
                };

                await userManager.CreateAsync(defaultUser, "test@123");
                await userManager.AddToRoleAsync(defaultUser, ApplicationUserRoles.Admin);

                logger.Information("Seeding complete for test user...");
            }
        }
    }
}
