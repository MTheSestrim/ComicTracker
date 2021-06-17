namespace ComicTracker.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using ComicTracker.Data.Common.Models;
    using ComicTracker.Data.Models;
    using ComicTracker.Web.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Arc> Arcs { get; set; }

        public virtual DbSet<ArcArtist> ArcArtists { get; set; }

        public virtual DbSet<ArcGenre> ArcGenres { get; set; }

        public virtual DbSet<ArcPublisher> ArcPublishers { get; set; }

        public virtual DbSet<ArcVolume> ArcVolumes { get; set; }

        public virtual DbSet<ArcWriter> ArcWriters { get; set; }

        public virtual DbSet<Artist> Artists { get; set; }

        public virtual DbSet<ArtistIssue> ArtistIssues { get; set; }

        public virtual DbSet<ArtistSeries> ArtistSeries { get; set; }

        public virtual DbSet<ArtistVolume> ArtistVolumes { get; set; }

        public virtual DbSet<Character> Characters { get; set; }

        public virtual DbSet<CharactersArc> CharactersArcs { get; set; }

        public virtual DbSet<CharactersIssue> CharactersIssues { get; set; }

        public virtual DbSet<CharactersSeries> CharactersSeries { get; set; }

        public virtual DbSet<CharactersVolume> CharactersVolumes { get; set; }

        public virtual DbSet<Genre> Genres { get; set; }

        public virtual DbSet<GenreIssue> GenreIssues { get; set; }

        public virtual DbSet<GenreSeries> GenreSeries { get; set; }

        public virtual DbSet<GenreVolume> GenreVolumes { get; set; }

        public virtual DbSet<Issue> Issues { get; set; }

        public virtual DbSet<IssueWriter> IssueWriters { get; set; }

        public virtual DbSet<Nationality> Nationalities { get; set; }

        public virtual DbSet<Publisher> Publishers { get; set; }

        public virtual DbSet<PublisherSeries> PublisherSeries { get; set; }

        public virtual DbSet<PublishersIssue> PublishersIssues { get; set; }

        public virtual DbSet<PublishersVolume> PublishersVolumes { get; set; }

        public virtual DbSet<Series> Series { get; set; }

        public virtual DbSet<SeriesWriter> SeriesWriters { get; set; }

        public virtual DbSet<Volume> Volumes { get; set; }

        public virtual DbSet<VolumeWriter> VolumeWriters { get; set; }

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

            builder.Entity<ArcArtist>(entity =>
            {
                entity.HasKey(e => new { e.ArcsId, e.ArtistsId });

                entity.ToTable("ArcArtist");

                entity.HasIndex(e => e.ArtistsId, "IX_ArcArtist_ArtistsId");

                entity.HasOne(d => d.Arcs)
                    .WithMany(p => p.ArcArtists)
                    .HasForeignKey(d => d.ArcsId);

                entity.HasOne(d => d.Artists)
                    .WithMany(p => p.ArcArtists)
                    .HasForeignKey(d => d.ArtistsId);
            });

            builder.Entity<ArcGenre>(entity =>
            {
                entity.HasKey(e => new { e.ArcsId, e.GenresId });

                entity.ToTable("ArcGenre");

                entity.HasIndex(e => e.GenresId, "IX_ArcGenre_GenresId");

                entity.HasOne(d => d.Arcs)
                    .WithMany(p => p.ArcGenres)
                    .HasForeignKey(d => d.ArcsId);

                entity.HasOne(d => d.Genres)
                    .WithMany(p => p.ArcGenres)
                    .HasForeignKey(d => d.GenresId);
            });

            builder.Entity<ArcPublisher>(entity =>
            {
                entity.HasKey(e => new { e.ArcsId, e.PublishersId });

                entity.ToTable("ArcPublisher");

                entity.HasIndex(e => e.PublishersId, "IX_ArcPublisher_PublishersId");

                entity.HasOne(d => d.Arcs)
                    .WithMany(p => p.ArcPublishers)
                    .HasForeignKey(d => d.ArcsId);

                entity.HasOne(d => d.Publishers)
                    .WithMany(p => p.ArcPublishers)
                    .HasForeignKey(d => d.PublishersId);
            });

            builder.Entity<ArcVolume>(entity =>
            {
                entity.HasKey(e => new { e.ArcId, e.VolumeId });

                entity.ToTable("ArcVolume");

                entity.HasIndex(e => e.VolumeId, "IX_ArcVolume_VolumeId");

                entity.HasOne(d => d.Arc)
                    .WithMany(p => p.ArcVolumes)
                    .HasForeignKey(d => d.ArcId);

                entity.HasOne(d => d.Volume)
                    .WithMany(p => p.ArcVolumes)
                    .HasForeignKey(d => d.VolumeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            builder.Entity<ArcWriter>(entity =>
            {
                entity.HasKey(e => new { e.ArcsId, e.WritersId });

                entity.ToTable("ArcWriter");

                entity.HasIndex(e => e.WritersId, "IX_ArcWriter_WritersId");

                entity.HasOne(d => d.Arcs)
                    .WithMany(p => p.ArcWriters)
                    .HasForeignKey(d => d.ArcsId);

                entity.HasOne(d => d.Writers)
                    .WithMany(p => p.ArcWriters)
                    .HasForeignKey(d => d.WritersId);
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

            builder.Entity<ArtistIssue>(entity =>
            {
                entity.HasKey(e => new { e.ArtistsId, e.IssuesId });

                entity.ToTable("ArtistIssue");

                entity.HasIndex(e => e.IssuesId, "IX_ArtistIssue_IssuesId");

                entity.HasOne(d => d.Artists)
                    .WithMany(p => p.ArtistIssues)
                    .HasForeignKey(d => d.ArtistsId);

                entity.HasOne(d => d.Issues)
                    .WithMany(p => p.ArtistIssues)
                    .HasForeignKey(d => d.IssuesId);
            });

            builder.Entity<ArtistSeries>(entity =>
            {
                entity.HasKey(e => new { e.ArtistsId, e.SeriesId });

                entity.HasIndex(e => e.SeriesId, "IX_ArtistSeries_SeriesId");

                entity.HasOne(d => d.Artists)
                    .WithMany(p => p.ArtistSeries)
                    .HasForeignKey(d => d.ArtistsId);

                entity.HasOne(d => d.Series)
                    .WithMany(p => p.ArtistSeries)
                    .HasForeignKey(d => d.SeriesId);
            });

            builder.Entity<ArtistVolume>(entity =>
            {
                entity.HasKey(e => new { e.ArtistsId, e.VolumesId });

                entity.ToTable("ArtistVolume");

                entity.HasIndex(e => e.VolumesId, "IX_ArtistVolume_VolumesId");

                entity.HasOne(d => d.Artists)
                    .WithMany(p => p.ArtistVolumes)
                    .HasForeignKey(d => d.ArtistsId);

                entity.HasOne(d => d.Volumes)
                    .WithMany(p => p.ArtistVolumes)
                    .HasForeignKey(d => d.VolumesId);
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

            builder.Entity<CharactersArc>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.ArcId });

                entity.HasIndex(e => e.ArcId, "IX_CharactersArcs_ArcId");

                entity.HasOne(d => d.Arc)
                    .WithMany(p => p.CharactersArcs)
                    .HasForeignKey(d => d.ArcId);

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharactersArcs)
                    .HasForeignKey(d => d.CharacterId);
            });

            builder.Entity<CharactersIssue>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.IssueId });

                entity.HasIndex(e => e.IssueId, "IX_CharactersIssues_IssueId");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharactersIssues)
                    .HasForeignKey(d => d.CharacterId);

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.CharactersIssues)
                    .HasForeignKey(d => d.IssueId);
            });

            builder.Entity<CharactersSeries>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.SeriesId });

                entity.HasIndex(e => e.SeriesId, "IX_CharactersSeries_SeriesId");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharactersSeries)
                    .HasForeignKey(d => d.CharacterId);

                entity.HasOne(d => d.Series)
                    .WithMany(p => p.CharactersSeries)
                    .HasForeignKey(d => d.SeriesId);
            });

            builder.Entity<CharactersVolume>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.VolumeId });

                entity.HasIndex(e => e.VolumeId, "IX_CharactersVolumes_VolumeId");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharactersVolumes)
                    .HasForeignKey(d => d.CharacterId);

                entity.HasOne(d => d.Volume)
                    .WithMany(p => p.CharactersVolumes)
                    .HasForeignKey(d => d.VolumeId);
            });

            builder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            builder.Entity<GenreIssue>(entity =>
            {
                entity.HasKey(e => new { e.GenresId, e.IssuesId });

                entity.ToTable("GenreIssue");

                entity.HasIndex(e => e.IssuesId, "IX_GenreIssue_IssuesId");

                entity.HasOne(d => d.Genres)
                    .WithMany(p => p.GenreIssues)
                    .HasForeignKey(d => d.GenresId);

                entity.HasOne(d => d.Issues)
                    .WithMany(p => p.GenreIssues)
                    .HasForeignKey(d => d.IssuesId);
            });

            builder.Entity<GenreSeries>(entity =>
            {
                entity.HasKey(e => new { e.GenresId, e.SeriesId });

                entity.HasIndex(e => e.SeriesId, "IX_GenreSeries_SeriesId");

                entity.HasOne(d => d.Genres)
                    .WithMany(p => p.GenreSeries)
                    .HasForeignKey(d => d.GenresId);

                entity.HasOne(d => d.Series)
                    .WithMany(p => p.GenreSeries)
                    .HasForeignKey(d => d.SeriesId);
            });

            builder.Entity<GenreVolume>(entity =>
            {
                entity.HasKey(e => new { e.GenresId, e.VolumesId });

                entity.ToTable("GenreVolume");

                entity.HasIndex(e => e.VolumesId, "IX_GenreVolume_VolumesId");

                entity.HasOne(d => d.Genres)
                    .WithMany(p => p.GenreVolumes)
                    .HasForeignKey(d => d.GenresId);

                entity.HasOne(d => d.Volumes)
                    .WithMany(p => p.GenreVolumes)
                    .HasForeignKey(d => d.VolumesId);
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

            builder.Entity<IssueWriter>(entity =>
            {
                entity.HasKey(e => new { e.IssuesId, e.WritersId });

                entity.ToTable("IssueWriter");

                entity.HasIndex(e => e.WritersId, "IX_IssueWriter_WritersId");

                entity.HasOne(d => d.Issues)
                    .WithMany(p => p.IssueWriters)
                    .HasForeignKey(d => d.IssuesId);

                entity.HasOne(d => d.Writers)
                    .WithMany(p => p.IssueWriters)
                    .HasForeignKey(d => d.WritersId);
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

            builder.Entity<PublisherSeries>(entity =>
            {
                entity.HasKey(e => new { e.PublishersId, e.SeriesId });

                entity.HasIndex(e => e.SeriesId, "IX_PublisherSeries_SeriesId");

                entity.HasOne(d => d.Publishers)
                    .WithMany(p => p.PublisherSeries)
                    .HasForeignKey(d => d.PublishersId);

                entity.HasOne(d => d.Series)
                    .WithMany(p => p.PublisherSeries)
                    .HasForeignKey(d => d.SeriesId);
            });

            builder.Entity<PublishersIssue>(entity =>
            {
                entity.HasKey(e => new { e.PublisherId, e.IssueId });

                entity.HasIndex(e => e.IssueId, "IX_PublishersIssues_IssueId");

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.PublishersIssues)
                    .HasForeignKey(d => d.IssueId);

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.PublishersIssues)
                    .HasForeignKey(d => d.PublisherId);
            });

            builder.Entity<PublishersVolume>(entity =>
            {
                entity.HasKey(e => new { e.PublisherId, e.VolumeId });

                entity.HasIndex(e => e.VolumeId, "IX_PublishersVolumes_VolumeId");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.PublishersVolumes)
                    .HasForeignKey(d => d.PublisherId);

                entity.HasOne(d => d.Volume)
                    .WithMany(p => p.PublishersVolumes)
                    .HasForeignKey(d => d.VolumeId);
            });

            builder.Entity<Series>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            builder.Entity<SeriesWriter>(entity =>
            {
                entity.HasKey(e => new { e.SeriesId, e.WritersId });

                entity.ToTable("SeriesWriter");

                entity.HasIndex(e => e.WritersId, "IX_SeriesWriter_WritersId");

                entity.HasOne(d => d.Series)
                    .WithMany(p => p.SeriesWriters)
                    .HasForeignKey(d => d.SeriesId);

                entity.HasOne(d => d.Writers)
                    .WithMany(p => p.SeriesWriters)
                    .HasForeignKey(d => d.WritersId);
            });

            builder.Entity<Volume>(entity =>
            {
                entity.HasIndex(e => e.SeriesId, "IX_Volumes_SeriesId");

                entity.Property(e => e.Title).HasMaxLength(150);

                entity.HasOne(d => d.Series)
                    .WithMany(p => p.Volumes)
                    .HasForeignKey(d => d.SeriesId);
            });

            builder.Entity<VolumeWriter>(entity =>
            {
                entity.HasKey(e => new { e.VolumesId, e.WritersId });

                entity.ToTable("VolumeWriter");

                entity.HasIndex(e => e.WritersId, "IX_VolumeWriter_WritersId");

                entity.HasOne(d => d.Volumes)
                    .WithMany(p => p.VolumeWriters)
                    .HasForeignKey(d => d.VolumesId);

                entity.HasOne(d => d.Writers)
                    .WithMany(p => p.VolumeWriters)
                    .HasForeignKey(d => d.WritersId);
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
