using System;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using HireMeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HireMeApp.Data
{
    public partial class HireMeContext : DbContext
    {
        public HireMeContext()
        {
        }

        public HireMeContext(DbContextOptions<HireMeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccessLogs> AccessLogs { get; set; }
        public virtual DbSet<InfoName> InfoName { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<Picture> Picture { get; set; }
        public virtual DbSet<TextBlock> TextBlock { get; set; }

        private static string Secrets()
        {
            string kvUri = "https://HireMeVault.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            KeyVaultSecret secret = client.GetSecret("ConnectStr");
            string key = secret.Value;
            return key;
        }

        private readonly string _conStr = Secrets();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(_conStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessLogs>(entity =>
            {
                entity.HasKey(e => e.IdColumn)
                    .HasName("PK__accessLo__1E5A0B8E124F0E74");

                entity.ToTable("accessLogs");

                entity.Property(e => e.IdColumn).HasColumnName("ID_column");

                entity.Property(e => e.AccessDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.PageName)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<InfoName>(entity =>
            {
                entity.Property(e => e.Info_Name)
                    .HasColumnName("infoname")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LanguageId).HasColumnName("languageId");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(e => e.LanguageName)
                    .IsRequired()
                    .HasColumnName("language")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Picture>(entity =>
            {
                entity.Property(e => e.ImageMimeType)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.InfoId).HasColumnName("infoId");

                entity.Property(e => e.Pic)
                    .IsRequired()
                    .HasColumnName("pic");

                entity.HasOne(d => d.Info)
                    .WithMany(p => p.Picture)
                    .HasForeignKey(d => d.InfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Picture_InfoName");
            });

            modelBuilder.Entity<TextBlock>(entity =>
            {
                entity.Property(e => e.InfoId).HasColumnName("infoId");

                entity.Property(e => e.Textblock1)
                    .IsRequired()
                    .HasColumnName("textblock1")
                    .HasColumnType("text");

                entity.HasOne(d => d.Info)
                    .WithMany(p => p.TextBlock)
                    .HasForeignKey(d => d.InfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_textinfo_info");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
