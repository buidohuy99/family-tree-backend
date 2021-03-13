using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
                entity.Property(e => e.Id).ValueGeneratedOnAdd();;

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
                entity.Property(e => e.Id).ValueGeneratedOnAdd();;

                entity.HasOne(e => e.ChildOfFamily)
                    .WithMany(f => f.Children)
                    .HasForeignKey(e => e.ChildOf)
                    .HasConstraintName("FK_Child_OfFamily");
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
    }
}
