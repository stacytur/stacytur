using System;
using System.Collections.Generic;
using kurs.Models;
using Microsoft.EntityFrameworkCore;

namespace kurs.Context;

public partial class MagazinminContext : DbContext
{
    public MagazinminContext()
    {
    }

    public MagazinminContext(DbContextOptions<MagazinminContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dostavka> Dostavka { get; set; }

    public virtual DbSet<Pokupatel> Pokupatel { get; set; }

    public virtual DbSet<SborschikZakaza> SborschikZakaza { get; set; }

    public virtual DbSet<Tovar> Tovar { get; set; }

    public virtual DbSet<Zakaz> Zakaz { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=magazinmin;User Id=postgres;Password=Qaz1wsx2edc3");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dostavka>(entity =>
        {
            entity.HasKey(e => e.IdDostavka).HasName("dostavka_pkey");

            entity.ToTable("dostavka");

            entity.Property(e => e.IdDostavka)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_dostavka");
            entity.Property(e => e.DataPolucheniya).HasColumnName("data_polucheniya");
            entity.Property(e => e.OjdaemayaData).HasColumnName("ojdaemaya_data");
            entity.Property(e => e.StatusDostavki)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("status_dostavki");
        });

        modelBuilder.Entity<Pokupatel>(entity =>
        {
            entity.HasKey(e => e.IdPokupatel).HasName("pokupatel_pkey");

            entity.ToTable("pokupatel");

            entity.HasIndex(e => e.PokupatelTelefon, "pokupatel_pokupatel_telefon_key").IsUnique();

            entity.Property(e => e.IdPokupatel)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_pokupatel");
            entity.Property(e => e.PokupatelAdresDostavki)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("pokupatel_adres_dostavki");
            entity.Property(e => e.PokupatelFio)
                .HasMaxLength(40)
                .IsFixedLength()
                .HasColumnName("pokupatel_fio");
            entity.Property(e => e.PokupatelTelefon)
                .HasPrecision(11)
                .HasColumnName("pokupatel_telefon");
        });

        modelBuilder.Entity<SborschikZakaza>(entity =>
        {
            entity.HasKey(e => e.IdSborschikZakaza).HasName("sborschik_zakaza_pkey");

            entity.ToTable("sborschik_zakaza");

            entity.HasIndex(e => e.SborschikZakazaTelefon, "sborschik_zakaza_sborschik_zakaza_telefon_key").IsUnique();

            entity.Property(e => e.IdSborschikZakaza)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_sborschik_zakaza");
            entity.Property(e => e.SborschikZakazaNames)
                .HasMaxLength(40)
                .IsFixedLength()
                .HasColumnName("sborschik_zakaza_names");
            entity.Property(e => e.SborschikZakazaTelefon)
                .HasPrecision(11)
                .HasColumnName("sborschik_zakaza_telefon");
        });

        modelBuilder.Entity<Tovar>(entity =>
        {
            entity.HasKey(e => e.IdTovar).HasName("tovar_pkey");

            entity.ToTable("tovar");

            entity.Property(e => e.IdTovar)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_tovar");
            entity.Property(e => e.Cena)
                .HasPrecision(10)
                .HasColumnName("cena");
            entity.Property(e => e.IdZakaz).HasColumnName("id_zakaz");
            entity.Property(e => e.Kolichestvo).HasColumnName("kolichestvo");
            entity.Property(e => e.TovarNaimenovanie)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("tovar_naimenovanie");

            entity.HasOne(d => d.IdZakazNavigation).WithMany(p => p.Tovar)
                .HasForeignKey(d => d.IdZakaz)
                .HasConstraintName("zakaz_tovar");
        });

        modelBuilder.Entity<Zakaz>(entity =>
        {
            entity.HasKey(e => e.IdZakaz).HasName("zakaz_pkey");

            entity.ToTable("zakaz");

            entity.Property(e => e.IdZakaz)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_zakaz");
            entity.Property(e => e.IdDostavka).HasColumnName("id_dostavka");
            entity.Property(e => e.IdPokupatel).HasColumnName("id_pokupatel");
            entity.Property(e => e.IdSborschikZakaza).HasColumnName("id_sborschik_zakaza");
            entity.Property(e => e.ZakazData).HasColumnName("zakaz_data");
            entity.Property(e => e.ZakazSumma)
                .HasPrecision(5)
                .HasColumnName("zakaz_summa");

            entity.HasOne(d => d.IdDostavkaNavigation).WithMany(p => p.Zakaz)
                .HasForeignKey(d => d.IdDostavka)
                .HasConstraintName("zakaz_dostavka");

            entity.HasOne(d => d.IdPokupatelNavigation).WithMany(p => p.Zakaz)
                .HasForeignKey(d => d.IdPokupatel)
                .HasConstraintName("zakaz_id_pokupatel");

            entity.HasOne(d => d.IdSborschikZakazaNavigation).WithMany(p => p.Zakaz)
                .HasForeignKey(d => d.IdSborschikZakaza)
                .HasConstraintName("zakaz_id_sborschik_zakaza");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
