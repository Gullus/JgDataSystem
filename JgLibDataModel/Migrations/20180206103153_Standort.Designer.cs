﻿// <auto-generated />
using JgLibDataModel;
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
    [Migration("20180206103153_Standort")]
    partial class Standort
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("JgLibDataModel.Tabellen.TabBediener", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Aenderung");

                    b.Property<string>("Nachname");

                    b.Property<string>("Vorname");

                    b.HasKey("Id");

                    b.ToTable("TabBedienerSet");
                });

            modelBuilder.Entity("JgLibDataModel.Tabellen.TabMaschine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Aenderung");

                    b.Property<Guid>("FStandort");

                    b.Property<string>("Ip");

                    b.Property<string>("MaschineName");

                    b.Property<int>("Port");

                    b.HasKey("Id");

                    b.HasIndex("FStandort");

                    b.ToTable("TabMaschineSet");
                });

            modelBuilder.Entity("JgLibDataModel.Tabellen.TabStandort", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Aenderung");

                    b.Property<string>("StandortName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("TabStandort");
                });

            modelBuilder.Entity("JgLibDataModel.Tabellen.TabMaschine", b =>
                {
                    b.HasOne("JgLibDataModel.Tabellen.TabStandort", "EStandort")
                        .WithMany("SKfzEinsatz")
                        .HasForeignKey("FStandort")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}