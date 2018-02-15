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
    [Migration("20180214154107_VorgangMeldung")]
    partial class VorgangMeldung
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<Guid>("Bediener");

                    b.Property<int>("DuchmesserInMm");

                    b.Property<DateTime?>("EndeFertigung");

                    b.Property<double>("GewichtInKg");

                    b.Property<string>("IdBauteilJgData");

                    b.Property<Guid>("IdMaschine");

                    b.Property<int>("LaengeInCm");

                    b.Property<byte[]>("Modifikation")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<DateTime>("StartFertigung");

                    b.HasKey("Id");

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

                    b.Property<string>("Nachname");

                    b.Property<string>("NummerAusweis");

                    b.Property<string>("Vorname");

                    b.HasKey("Id");

                    b.ToTable("TabBedienerSet");
                });

            modelBuilder.Entity("JgLibDataModel.TabBedienerBauteil", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Aenderung");

                    b.Property<Guid>("IdBauteil");

                    b.Property<Guid>("IdBediener");

                    b.Property<byte[]>("Modifikation")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("IdBauteil");

                    b.HasIndex("IdBediener");

                    b.ToTable("TabBedienerBauteilSet");
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

                    b.Property<string>("MaschineName");

                    b.Property<int>("MaschinePort");

                    b.Property<byte[]>("Modifikation")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("NummerScanner");

                    b.Property<bool>("SammelScannung");

                    b.Property<bool>("ScannerMitDisplay");

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

                    b.Property<DateTime>("ZeitMeldung");

                    b.HasKey("Id");

                    b.HasIndex("IdBediener");

                    b.HasIndex("IdMaschine");

                    b.ToTable("TabMeldungSet");
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
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("TabStandortSet");
                });

            modelBuilder.Entity("JgLibDataModel.TabBauteil", b =>
                {
                    b.HasOne("JgLibDataModel.TabMaschine", "EMaschine")
                        .WithMany()
                        .HasForeignKey("IdMaschine")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("JgLibDataModel.TabBedienerBauteil", b =>
                {
                    b.HasOne("JgLibDataModel.TabBauteil", "EBauteil")
                        .WithMany("SBedienerBauteil")
                        .HasForeignKey("IdBauteil")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("JgLibDataModel.TabBediener", "EBediener")
                        .WithMany("SBauteilBediener")
                        .HasForeignKey("IdBediener")
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
