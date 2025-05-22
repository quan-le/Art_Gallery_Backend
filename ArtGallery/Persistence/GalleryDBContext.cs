using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ArtGallery.Models;

namespace ArtGallery.Persistence
{
    public partial class GalleryDBContext : DbContext
    {
        public virtual DbSet<Artifact> Artifacts { get; set; } = null!;
        public virtual DbSet<Artist> Artists { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UsersRole> UsersRoles { get; set; } = null!;

        public GalleryDBContext()
        {
        }

        public GalleryDBContext(DbContextOptions<GalleryDBContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("data source=LAPTOP-BB93VNEB\\SQLEXPRESS;initial catalog=ArtGallery;trusted_connection=true;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artifact>(entity =>
            {
                entity.HasKey(e => e.artifact_id)
                    .HasName("PK__Artifact__A074A76F3489080E");

                entity.HasIndex(e => e.date_display, "IX_Artifacts_Active")
                    .HasFilter("([date_display] IS NOT NULL)");

                entity.HasIndex(e => e.date_start, "IX_Artifacts_DateStart");

                entity.HasIndex(e => new { e.location, e.longitude, e.latitude }, "IX_Artifacts_Geo");

                entity.HasIndex(e => new { e.title, e.place_of_origin, e.date_display }, "IX_Artifacts_QuickDisplay");

                entity.HasIndex(e => e.title, "IX_Artifacts_Title");

                entity.Property(e => e.artifact_id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.created_date)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.date_display).HasMaxLength(100);

                entity.Property(e => e.dimension).HasMaxLength(100);

                entity.Property(e => e.image_url).HasMaxLength(2048);

                entity.Property(e => e.location).HasMaxLength(150);

                entity.Property(e => e.material).HasMaxLength(100);

                entity.Property(e => e.modified_date)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.place_of_origin).HasMaxLength(150);

                entity.Property(e => e.title).HasMaxLength(255);

                entity.HasMany(d => d.tags)
                    .WithMany(p => p.artifacts)
                    .UsingEntity<Dictionary<string, object>>(
                        "ArtifactsTag",
                        l => l.HasOne<Tag>().WithMany().HasForeignKey("tag_id").HasConstraintName("FK_ArtifactsTags_Tag"),
                        r => r.HasOne<Artifact>().WithMany().HasForeignKey("artifact_id").HasConstraintName("FK_ArtifactsTags_Artifact"),
                        j =>
                        {
                            j.HasKey("artifact_id", "tag_id");

                            j.ToTable("ArtifactsTags");

                            j.HasIndex(new[] { "artifact_id", "tag_id" }, "IX_ArtifactsTags_Artifact");

                            j.HasIndex(new[] { "tag_id" }, "IX_ArtifactsTags_Tag");
                        });
            });

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.HasKey(e => e.artist_id)
                    .HasName("PK__Artists__6CD040012C327FB8");

                entity.HasIndex(e => e.birth_date, "IX_Artists_Birth");

                entity.HasIndex(e => new { e.last_name, e.first_name }, "IX_Artists_LastFirstName");

                entity.Property(e => e.artist_id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.created_date)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.first_name).HasMaxLength(100);

                entity.Property(e => e.gender).HasMaxLength(20);

                entity.Property(e => e.last_name).HasMaxLength(100);

                entity.Property(e => e.modified_date)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.nationality).HasMaxLength(100);

                entity.HasMany(d => d.artifacts)
                    .WithMany(p => p.artists)
                    .UsingEntity<Dictionary<string, object>>(
                        "ArtifactsArtist",
                        l => l.HasOne<Artifact>().WithMany().HasForeignKey("artifact_id").HasConstraintName("FK_ArtifactsArtists_Artifact"),
                        r => r.HasOne<Artist>().WithMany().HasForeignKey("artist_id").HasConstraintName("FK_ArtifactsArtists_Artist"),
                        j =>
                        {
                            j.HasKey("artist_id", "artifact_id");

                            j.ToTable("ArtifactsArtists");

                            j.HasIndex(new[] { "artifact_id", "artist_id" }, "IX_ArtifactsArtists_Artifact");

                            j.HasIndex(new[] { "artist_id" }, "IX_ArtifactsArtists_Artist");
                        });
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.role_id)
                    .HasName("PK__Roles__760965CC611C36A1");

                entity.HasIndex(e => e.role_id, "IX_Roles_Id");

                entity.HasIndex(e => e.role_name, "IX_Roles_Name");

                entity.HasIndex(e => e.role_name, "UQ__Roles__783254B11B119647")
                    .IsUnique();

                entity.Property(e => e.role_id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.description).HasMaxLength(512);

                entity.Property(e => e.role_name).HasMaxLength(100);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.tag_id)
                    .HasName("PK__Tags__4296A2B6F37111A1");

                entity.HasIndex(e => e.tag_id, "IX_Tags_Id");

                entity.HasIndex(e => e.tag_name, "IX_Tags_Name");

                entity.HasIndex(e => e.tag_name, "UQ__Tags__E298655C4E27715B")
                    .IsUnique();

                entity.Property(e => e.tag_id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.tag_description).HasMaxLength(512);

                entity.Property(e => e.tag_name).HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.user_id)
                    .HasName("PK__Users__B9BE370F91E7449C");

                entity.HasIndex(e => e.email, "IX_Users_Email");

                entity.HasIndex(e => e.user_id, "IX_Users_Id");

                entity.HasIndex(e => e.email, "UQ__Users__AB6E61641F378F07")
                    .IsUnique();

                entity.Property(e => e.user_id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.created_date)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.email).HasMaxLength(256);

                entity.Property(e => e.first_name).HasMaxLength(100);

                entity.Property(e => e.last_name).HasMaxLength(100);

                entity.Property(e => e.modified_date)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.password_hash).HasMaxLength(512);
            });

            modelBuilder.Entity<UsersRole>(entity =>
            {
                entity.HasKey(e => new { e.user_id, e.role_id });

                entity.HasIndex(e => new { e.role_id, e.user_id }, "IX_UsersRoles_RoleUser");

                entity.HasIndex(e => e.user_id, "IX_UsersRoles_UserId");

                entity.Property(e => e.assigned_date)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.role)
                    .WithMany(p => p.UsersRoles)
                    .HasForeignKey(d => d.role_id)
                    .HasConstraintName("FK_UsersRoles_Role");

                entity.HasOne(d => d.user)
                    .WithMany(p => p.UsersRoles)
                    .HasForeignKey(d => d.user_id)
                    .HasConstraintName("FK_UsersRoles_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
