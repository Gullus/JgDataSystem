﻿// <auto-generated />
using JgLibDataModel.Tabellen;
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
    [Migration("20180206085554_DbErstellt")]
    partial class DbErstellt
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("JgLibDataModel.Tabellen.tabBediener", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nachname");

                    b.Property<string>("Vorname");

                    b.HasKey("Id");

                    b.ToTable("TabBedienerSet");
                });
#pragma warning restore 612, 618
        }
    }
}