namespace ComicTracker.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using ComicTracker.Data.Common.Models;
    using ComicTracker.Data.Models;
    using ComicTracker.Data.Models.Entities;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ComicTrackerDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ComicTrackerDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ComicTrackerDbContext(DbContextOptions<ComicTrackerDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Arc> Arcs { get; set; }

        public virtual DbSet<ArcVolume> ArcVolumes { get; set; }

        public virtual DbSet<Artist> Artists { get; set; }

        public virtual DbSet<Character> Characters { get; set; }

        public virtual DbSet<CharacterArc> CharactersArcs { get; set; }

        public virtual DbSet<CharacterIssue> CharactersIssues { get; set; }

        public virtual DbSet<CharacterSeries> CharactersSeries { get; set; }

        public virtual DbSet<CharacterVolume> CharactersVolumes { get; set; }

        public virtual DbSet<Genre> Genres { get; set; }

        public virtual DbSet<Issue> Issues { get; set; }

        public virtual DbSet<Nationality> Nationalities { get; set; }

        public virtual DbSet<Publisher> Publishers { get; set; }

        public virtual DbSet<PublisherIssue> PublishersIssues { get; set; }

        public virtual DbSet<PublisherVolume> PublishersVolumes { get; set; }

        public virtual DbSet<Series> Series { get; set; }

        public virtual DbSet<Volume> Volumes { get; set; }

        public virtual DbSet<Writer> Writers { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            this.ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            builder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            builder.Entity<Arc>(entity =>
            {
                entity.HasIndex(e => e.SeriesId, "IX_Arcs_SeriesId");

                entity.Property(e => e.Title).HasMaxLength(150);

                entity.HasOne(d => d.Series)
                    .WithMany(p => p.Arcs)
                    .HasForeignKey(d => d.SeriesId);
            });

            builder.Entity<ArcVolume>(entity =>
            {
                entity.HasKey(e => new { e.ArcId, e.VolumeId });

                entity.ToTable("ArcVolumes");

                entity.HasIndex(e => e.VolumeId, "IX_ArcVolume_VolumeId");

                entity.HasOne(av => av.Arc)
                    .WithMany(a => a.ArcsVolumes)
                    .HasForeignKey(av => av.ArcId);

                entity.HasOne(av => av.Volume)
                    .WithMany(v => v.ArcsVolumes)
                    .HasForeignKey(av => av.VolumeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            builder.Entity<Artist>(entity =>
            {
                entity.HasIndex(e => e.NationalityId, "IX_Artists_NationalityId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Nationality)
                    .WithMany(p => p.Artists)
                    .HasForeignKey(d => d.NationalityId);
            });

            builder.Entity<Character>(entity =>
            {
                entity.Property(e => e.FirstAppearance)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            builder.Entity<CharacterArc>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.ArcId });

                entity.ToTable("CharactersArcs");

                entity.HasOne(ca => ca.Character)
                    .WithMany(c => c.CharactersArcs)
                    .HasForeignKey(ca => ca.CharacterId);

                entity.HasOne(ca => ca.Arc)
                    .WithMany(a => a.CharactersArcs)
                    .HasForeignKey(ca => ca.ArcId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            builder.Entity<CharacterIssue>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.IssueId });

                entity.ToTable("CharactersIssues");

                entity.HasOne(ci => ci.Character)
                    .WithMany(c => c.CharactersIssues)
                    .HasForeignKey(ci => ci.CharacterId);

                entity.HasOne(ci => ci.Issue)
                    .WithMany(i => i.CharactersIssues)
                    .HasForeignKey(ci => ci.IssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            builder.Entity<CharacterSeries>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.SeriesId });

                entity.ToTable("CharactersSeries");

                entity.HasOne(cs => cs.Character)
                    .WithMany(c => c.CharactersSeries)
                    .HasForeignKey(cs => cs.CharacterId);

                entity.HasOne(cs => cs.Series)
                    .WithMany(s => s.CharactersSeries)
                    .HasForeignKey(cs => cs.SeriesId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            builder.Entity<CharacterVolume>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.VolumeId });

                entity.ToTable("CharactersVolumes");

                entity.HasOne(cv => cv.Character)
                    .WithMany(c => c.CharactersVolumes)
                    .HasForeignKey(cv => cv.CharacterId);

                entity.HasOne(cv => cv.Volume)
                    .WithMany(v => v.CharactersVolumes)
                    .HasForeignKey(cv => cv.VolumeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            builder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            builder.Entity<Issue>(entity =>
            {
                entity.HasIndex(e => e.ArcId, "IX_Issues_ArcId");

                entity.HasIndex(e => e.SeriesId, "IX_Issues_SeriesId");

                entity.HasIndex(e => e.VolumeId, "IX_Issues_VolumeId");

                entity.Property(e => e.Title).HasMaxLength(150);

                entity.HasOne(d => d.Arc)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.ArcId);

                entity.HasOne(d => d.Series)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.SeriesId);

                entity.HasOne(d => d.Volume)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.VolumeId);
            });

            builder.Entity<Nationality>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            builder.Entity<Publisher>(entity =>
            {
                entity.HasIndex(e => e.NationalityId, "IX_Publishers_NationalityId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Nationality)
                    .WithMany(p => p.Publishers)
                    .HasForeignKey(d => d.NationalityId);
            });

            builder.Entity<PublisherIssue>(entity =>
            {
                entity.HasKey(e => new { e.PublisherId, e.IssueId });

                entity.ToTable("PublishersIssues");

                entity.HasOne(pi => pi.Publisher)
                    .WithMany(p => p.PublishersIssues)
                    .HasForeignKey(pi => pi.PublisherId);

                entity.HasOne(pi => pi.Issue)
                    .WithMany(i => i.PublishersIssues)
                    .HasForeignKey(pi => pi.IssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            builder.Entity<PublisherVolume>(entity =>
            {
                entity.HasKey(e => new { e.PublisherId, e.VolumeId });

                entity.ToTable("PublishersVolumes");

                entity.HasOne(pv => pv.Publisher)
                    .WithMany(p => p.PublishersVolumes)
                    .HasForeignKey(pv => pv.PublisherId);

                entity.HasOne(pv => pv.Volume)
                    .WithMany(v => v.PublishersVolumes)
                    .HasForeignKey(pv => pv.VolumeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            builder.Entity<Series>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            builder.Entity<Volume>(entity =>
            {
                entity.HasIndex(e => e.SeriesId, "IX_Volumes_SeriesId");

                entity.Property(e => e.Title).HasMaxLength(150);

                entity.HasOne(d => d.Series)
                    .WithMany(p => p.Volumes)
                    .HasForeignKey(d => d.SeriesId);
            });

            builder.Entity<Writer>(entity =>
            {
                entity.HasIndex(e => e.NationalityId, "IX_Writers_NationalityId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Nationality)
                    .WithMany(p => p.Writers)
                    .HasForeignKey(d => d.NationalityId);
            });
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        // Applies configurations
        private void ConfigureUserIdentityRelations(ModelBuilder builder)
             => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
