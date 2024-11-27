using System;
using System.Collections.Generic;
using GPO_BLAZOR.DBAgents.DBModels;
using Microsoft.EntityFrameworkCore;

namespace GPO_BLAZOR.DBAgents;

public partial class Gpo2Context : DbContext
{
    public Gpo2Context()
    {

    }

    public Gpo2Context(DbContextOptions<Gpo2Context> options)
        : base(options)
    {

    }

    public virtual DbSet<AskForm> Ancets { get; set; }

    public virtual DbSet<TypeandViemType> TypeandViemTypes { get; set; }

    public virtual DbSet<Groups> Groupss { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<Cafedral> Cafedrals { get; set; }

    public virtual DbSet<Direction> Directions { get; set; }

    public virtual DbSet<Organisation> Organisations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=gpo2;Username=postgres;Password=NiK!1488");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AskForm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Анкета_pkey");

            entity.ToTable("Анкета");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TypeandViemType).HasColumnName("Вид и тип");
            entity.Property(e => e.ResponsibleForFilling).HasColumnName("Ответственные за заполнение");
            entity.Property(e => e.SupervisorAndConsultant).HasColumnName("Руководитель консультант");
            entity.Property(e => e.FactoryPracticeLeader).HasColumnName("Руководитель практики орг");

            entity.HasOne(d => d.TypeandViemTypeNavigation).WithMany(p => p.AskForms)
                .HasForeignKey(d => d.TypeandViemType)
                .HasConstraintName("Анкета_Вид и тип_fkey");

            entity.HasOne(d => d.ContractNavigation).WithMany(p => p.AskForms)
                .HasForeignKey(d => d.Contract)
                .HasConstraintName("Анкета_Договор_fkey");

            entity.HasOne(d => d.ResponsibleForFillingеNavigation).WithMany(p => p.AskFormResponsibleForFillingNavigations)
                .HasForeignKey(d => d.ResponsibleForFilling)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Анкета_Ответственные за практи_fkey");

            entity.HasOne(d => d.SupervisorAndConsultantNavigation).WithMany(p => p.AskFormSupervisorAndConsultantNavigations)
                .HasForeignKey(d => d.SupervisorAndConsultant)
                .HasConstraintName("Анкета_Руководитель практики о_fkey");

            entity.HasOne(d => d.FactoryPracticeLeaderNavigation).WithMany(p => p.AskFormFactoryPracticeLeaderNavigations)
                .HasForeignKey(d => d.FactoryPracticeLeader)
                .HasConstraintName("Анкета_Руководитель практики_fkey");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.AskForms)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Анкета_Статус_fkey");
        });

        modelBuilder.Entity<TypeandViemType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Вид и тип практики_pkey");

            entity.ToTable("Вид и тип практики");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<Groups>(entity =>
        {
            entity.HasKey(e => e.Group).HasName("Группы_pkey");

            entity.ToTable("Группы");

            entity.Property(e => e.Year).HasColumnName("Год поступления");

            entity.HasOne(d => d.CafedralNavigation).WithMany(p => p.Groups)
                .HasForeignKey(d => d.Cafedral)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Группы_Кафедра_fkey");

            entity.HasOne(d => d.DirectionNavigation).WithMany(p => p.Groups)
                .HasForeignKey(d => d.Direction)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Группы_Направление_fkey");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Договор_pkey");

            entity.ToTable("Договор");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DataContract).HasColumnName("Дата договора о практике");
            entity.Property(e => e.DataPracticeStyle).HasColumnName("Дата начала практики");
            entity.Property(e => e.DataPracticeEnd).HasColumnName("Дата окончания практики");
            entity.Property(e => e.Equipment).HasColumnName("Материально-Техническое обеспече");
            entity.Property(e => e.PracticeContractNumber).HasColumnName("Номер договора о практике");

            entity.HasOne(d => d.OrganisationNavigation).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.Organisation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Договор_Организация_fkey");
        });

        modelBuilder.Entity<Cafedral>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Кафедра_pkey");

            entity.ToTable("Кафедра");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.LeaderNavigation).WithMany(p => p.Cafedrals)
                .HasForeignKey(d => d.Leader)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Кафедра_Заведующий_fkey");
        });

        modelBuilder.Entity<Direction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Направление_pkey");

            entity.ToTable("Направление");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.LeaderNavigation).WithMany(p => p.Directions)
                .HasForeignKey(d => d.Leader)
                .HasConstraintName("Руководитель_fkey");
        });

        modelBuilder.Entity<Organisation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Организация_pkey");

            entity.ToTable("Организация");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FactoryLeaderName).HasColumnName("Руководитель организации");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Пользователь_pkey");

            entity.ToTable("Пользователь");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasMany(d => d.Organisations).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "ОрганизацияПользователь",
                    r => r.HasOne<Organisation>().WithMany()
                        .HasForeignKey("Организация")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Организация_пользо_Организация_fkey"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("Пользователь")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Организация_польз_Пользователь_fkey"),
                    j =>
                    {
                        j.HasKey("Пользователь", "Организация").HasName("Организация_пользователь_pkey");
                        j.ToTable("Организация_пользователь");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Роль_pkey");

            entity.ToTable("Роль");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasMany(d => d.Users).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "РольПользователь",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("Пользователь")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Роль_Пользователь_Пользователь_fkey"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("Роль")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Роль_Пользователь_Роль_fkey"),
                    j =>
                    {
                        j.HasKey("Роль", "Пользователь").HasName("Роль_Пользователь_pkey");
                        j.ToTable("Роль_Пользователь");
                    });
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Статус_pkey");

            entity.ToTable("Статус");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.StatusFirst).HasColumnName("Статус");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => new { e.User, e.Group }).HasName("Студент_pkey");

            entity.ToTable("Студент");

            entity.Property(e => e.AdmissionYear).HasColumnName("Год поступления");

            entity.HasOne(d => d.GroupsNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Group)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Студент_Группа_fkey");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.User)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Студент_Пользователь_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
