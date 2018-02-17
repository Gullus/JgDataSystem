﻿// <auto-generated />
using JgLibDataModel;
using JgLibHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace JgLibDataModel.Migrations
{
    [DbContext(typeof(JgMaschineDb))]
    partial class JgMaschineDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("JgLibDataModel.TabBauteil", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Aenderung");

                    b.Property<int>("AnzahlBiegungen");

                    b.Property<int>("AnzahlHelfer");

                    b.Property<int>("DuchmesserInMm");

                    b.Property<DateTime?>("EndeFertigung");

                    b.Property<double>("GewichtInKg");

                    b.Property<string>("IdBauteilJgData");

                    b.Property<Guid>("IdBediener");

                    b.Property<Guid>("IdMaschine");

                    b.Property<int>("LaengeInCm");

                    b.Property<byte[]>("Modifikation")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<DateTime>("StartFertigung");

                    b.HasKey("Id");

                    b.HasIndex("IdBediener");

                    b.HasIndex("IdMaschine");

                    b.ToTable("TabBauteilSet");
                });

            modelBuilder.Entity("JgLibDataModel.TabBediener", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Aenderung");

                    b.Property<byte[]>("Modifikation")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Nachname")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("NummerAusweis")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<string>("Vorname")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("TabBedienerSet");
                });

            modelBuilder.Entity("JgLibDataModel.TabMaschine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Aenderung");

                    b.Property<string>("Bemerkung");

                    b.Property<Guid>("IdStandort");

                    b.Property<bool>("IstAktiv");

                    b.Property<int>("MaschineArt");

                    b.Property<string>("MaschineIp");

                    b.Property<string>("MaschineName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<int>("MaschinePort");

                    b.Property<byte[]>("Modifikation")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("NummerScanner")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<bool>("SammelScannung");

                    b.Property<bool>("ScannerMitDisplay");

                    b.Property<byte[]>("StatusMaschine");

                    b.Property<int>("VorschubProMeterInSek");

                    b.Property<int>("ZeitProBauteilInSek");

                    b.Property<int>("ZeitProBiegungInSek");

                    b.HasKey("Id");

                    b.HasIndex("IdStandort");

                    b.ToTable("TabMaschineSet");
                });

            modelBuilder.Entity("JgLibDataModel.TabMeldung", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Aenderung");

                    b.Property<int?>("Anzahl");

                    b.Property<string>("Bemerkung");

                    b.Property<Guid>("IdBediener");

                    b.Property<Guid>("IdMaschine");

                    b.Property<int>("Meldung");

                    b.Property<byte[]>("Modifikation")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("Status");

                    b.Property<DateTime?>("ZeitAbmeldung");

                    b.Property<DateTime>("ZeitMeldung");

                    b.HasKey("Id");

                    b.HasIndex("IdBediener");

                    b.HasIndex("IdMaschine");

                    b.ToTable("TabMeldungSet");
                });

            modelBuilder.Entity("JgLibDataModel.TabReport", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Aenderung");

                    b.Property<string>("Beschreibung");

                    b.Property<byte[]>("Modifikation")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<byte[]>("ReportDaten");

                    b.Property<string>("ReportName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("TabReportSet");
                });

            modelBuilder.Entity("JgLibDataModel.TabStandort", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Aenderung");

                    b.Property<byte[]>("Modifikation")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("StandortName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("TabStandortSet");
                });

            modelBuilder.Entity("JgLibDataModel.TabBauteil", b =>
                {
                    b.HasOne("JgLibDataModel.TabBediener", "EBediener")
                        .WithMany("SBauteile")
                        .HasForeignKey("IdBediener")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("JgLibDataModel.TabMaschine", "EMaschine")
                        .WithMany()
                        .HasForeignKey("IdMaschine")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("JgLibDataModel.TabMaschine", b =>
                {
                    b.HasOne("JgLibDataModel.TabStandort", "EStandort")
                        .WithMany("SMaschinen")
                        .HasForeignKey("IdStandort")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("JgLibDataModel.TabMeldung", b =>
                {
                    b.HasOne("JgLibDataModel.TabBediener", "EBediener")
                        .WithMany("SMeldungen")
                        .HasForeignKey("IdBediener")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("JgLibDataModel.TabMaschine", "EMaschine")
                        .WithMany("SMeldungen")
                        .HasForeignKey("IdMaschine")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
