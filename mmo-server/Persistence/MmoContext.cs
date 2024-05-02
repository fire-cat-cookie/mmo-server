using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using mmo_server.Gamestate;

namespace mmo_server.Persistence;

public partial class MmoContext : DbContext
{
    public MmoContext()
    {
    }

    public MmoContext(DbContextOptions<MmoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Character> Characters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Persist Security Info = False; database = mmo; server = localhost; user id = root; Password = kylie");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("accounts");

            entity.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Username, "username_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(36)
                .IsFixedLength()
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(45)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("characters");

            entity.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Name, "name_UNIQUE").IsUnique();

            entity.HasIndex(e => new { e.AccountId, e.Slot }, "slot_assignment").IsUnique();

            entity.HasIndex(e => new { e.AccountId, e.Slot }, "slot_assignment_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Class).HasColumnName("class");
            entity.Property(e => e.Level)
                .HasDefaultValueSql("'1'")
                .HasColumnName("level");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.PositionX)
                .IsRequired()
                .HasDefaultValueSql("'0'")
                .HasColumnName("positionX");
            entity.Property(e => e.PositionY)
                .IsRequired()
                .HasDefaultValueSql("'0'")
                .HasColumnName("positionY");
            entity.Property(e => e.Slot).HasColumnName("slot");
            entity.Property(e => e.ZoneId)
                .IsRequired()
                .HasDefaultValueSql("'0'")
                .HasColumnName("zone_id");

            entity.HasOne(d => d.Account).WithMany(p => p.Characters)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("account_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
