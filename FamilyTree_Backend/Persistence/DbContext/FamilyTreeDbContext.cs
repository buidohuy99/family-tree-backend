using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Core.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FamilyTreeBackend.Infrastructure.Persistence.DbContext
{
    public partial class FamilyTreeDbContext : IdentityDbContext<ApplicationUser>
    {
        public FamilyTreeDbContext(DbContextOptions<FamilyTreeDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Family> Families { get; set; }
        public virtual DbSet<FamilyTree> FamilyTrees { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Relationship> Relationships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Family>((entity) => {
                entity.ToTable("Family");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.DateCreated)
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd();

                //entity.Property(e => e.LastModified)
                //    .HasColumnName("updatedAt")
                //    .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                //    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(e => e.Parent1)
                    .WithMany()
                    .HasForeignKey(e => e.Parent1Id);

                entity.HasOne(e => e.Parent2)
                    .WithMany()
                    .HasForeignKey(e => e.Parent2Id);
            });

            modelBuilder.Entity<Person>((entity) => {
                entity.ToTable("Person");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.ChildOfFamily)
                    .WithMany(f => f.Children)
                    .HasForeignKey(e => e.ChildOf)
                    .HasConstraintName("FK_Child_OfFamily")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Relationship>((entity) => {
                entity.ToTable("Relationship");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id);

                entity.HasOne(e => e.Family)
                    .WithOne(f => f.Relationship)
                    .HasForeignKey<Relationship>(e => e.Id)
                    .HasConstraintName("FK_Relationship_OfFamily");
            });

            modelBuilder.Entity<Marriage>((entity) =>
            {
                entity.ToTable("Marriage");
            });

            modelBuilder.Entity<FamilyTree>((entity) => {
                entity.ToTable("FamilyTree");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();;

                entity.HasMany(e => e.People)
                    .WithOne()
                    .HasConstraintName("Constraints_PeopleOfTree");

                entity.HasMany(e => e.Families)
                    .WithOne()
                    .HasConstraintName("Constraints_FamiliesOfTree");
            });


            // Set asp net table to be compatible with mysql 8
            //modelBuilder.Entity<ApplicationUser>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            //modelBuilder.Entity<ApplicationUser>(entity => entity.Property(m => m.NormalizedEmail).HasMaxLength(85));
            //modelBuilder.Entity<ApplicationUser>(entity => entity.Property(m => m.NormalizedUserName).HasMaxLength(85));

            //modelBuilder.Entity<IdentityRole>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            //modelBuilder.Entity<IdentityRole>(entity => entity.Property(m => m.NormalizedName).HasMaxLength(85));

            //modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(85));
            //modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.ProviderKey).HasMaxLength(85));
            //modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            //modelBuilder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));

            //modelBuilder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(85));

            //modelBuilder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            //modelBuilder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(85));
            //modelBuilder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.Name).HasMaxLength(85));

            //modelBuilder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            //modelBuilder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            //modelBuilder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            //modelBuilder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(85));
        }

        //public void SeedData(ModelBuilder modelBuilder)
        //{
            
        //    FamilyTree familyTree = new FamilyTree
        //    {
        //        Id = 1,
        //        Name = "Hung Thi Family Tree",
        //        Description = "Hung Thi Test Family Tree",
        //    };
            
        //    //modelBuilder.Entity<FamilyTree>().HasData(familyTree);

        //    //Person person = new Person
        //    //{
        //    //    Id = 1,
        //    //    FirstName = "Hung",
        //    //    LastName = "Thi",
        //    //    DateOfBirth = DateTime.Today,
        //    //    Gender = Gender.MALE,
        //    //    FamilyTreeId = 1,
        //    //    ChildOf = 1,

        //    //};
        //    //Person father = new Person
        //    //{
        //    //    Id = 2,
        //    //    FirstName = "Senh",
        //    //    LastName = "Thi",
        //    //    DateOfBirth = new DateTime(1945, 07, 20),
        //    //    Gender = Gender.MALE,
        //    //    FamilyTreeId = 1,

        //    //};
        //    //Person mother = new Person
        //    //{
        //    //    Id = 3,
        //    //    FirstName = "Phan",
        //    //    LastName = "Luan",
        //    //    DateOfBirth = new DateTime(1954, 10, 20),
        //    //    Gender = Gender.MALE,
        //    //    FamilyTreeId = 1,

        //    //};

        //    //Family family = new Family
        //    //{
        //    //    Id = 1,
        //    //    Parent1Id = 2,
        //    //    Parent2Id = 3,
                
        //    //};

        //    //Marriage relationship = new Marriage
        //    //{
        //    //    Id = 1,
        //    //    RelationshipType = RelationshipType.MARRIED,
        //    //    DateOfMarriage = new DateTime(1985, 10, 11),
        //    //};

        //    //family.Relationship = relationship;
        //    //modelBuilder.Entity<Person>().HasData(person, father, mother );
        //    //modelBuilder.Entity<Family>().HasData(family);
        //    //modelBuilder.Entity<Marriage>().HasData(relationship);
        //    //modelBuilder.Entity<Relationship>().HasData(relationship as Relationship);
        //}

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
                Gender = Gender.MALE,
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

            Marriage relationship = new Marriage
            {
                RelationshipType = RelationshipType.MARRIED,
                DateOfMarriage = new DateTime(1985, 10, 11),
            };

            family.Relationship = relationship;

            familyTree.Families.Add(family);


            SaveChanges();
        }
    }
}
