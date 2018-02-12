using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JgLibDataModel.Migrations
{
    public partial class Schnittstellen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TabMaschineSet_TabStandortSet_FStandort",
                table: "TabMaschineSet");

            migrationBuilder.RenameColumn(
                name: "Port",
                table: "TabMaschineSet",
                newName: "ZeitProBiegungInSek");

            migrationBuilder.RenameColumn(
                name: "MaschinenArt",
                table: "TabMaschineSet",
                newName: "ZeitProBauteilInSek");

            migrationBuilder.RenameColumn(
                name: "IpAdresse",
                table: "TabMaschineSet",
                newName: "NummerScanner");

            migrationBuilder.RenameColumn(
                name: "FStandort",
                table: "TabMaschineSet",
                newName: "IdStandort");

            migrationBuilder.RenameIndex(
                name: "IX_TabMaschineSet_FStandort",
                table: "TabMaschineSet",
                newName: "IX_TabMaschineSet_IdStandort");

            migrationBuilder.AddColumn<string>(
                name: "Bemerkung",
                table: "TabMaschineSet",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IstAktiv",
                table: "TabMaschineSet",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaschineArt",
                table: "TabMaschineSet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MaschineIp",
                table: "TabMaschineSet",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaschinePort",
                table: "TabMaschineSet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SammelScannung",
                table: "TabMaschineSet",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ScannerMitDisplay",
                table: "TabMaschineSet",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VorschubProMeterInSek",
                table: "TabMaschineSet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TabBauteilSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Aenderung = table.Column<DateTime>(nullable: false),
                    AnzahlBiegungen = table.Column<int>(nullable: false),
                    DuchmesserInMm = table.Column<int>(nullable: false),
                    EndeFertigung = table.Column<DateTime>(nullable: true),
                    GewichtInKg = table.Column<double>(nullable: false),
                    IdBauteilJgData = table.Column<int>(nullable: false),
                    IdMaschine = table.Column<Guid>(nullable: false),
                    LaengeInCm = table.Column<int>(nullable: false),
                    Modifikation = table.Column<byte[]>(rowVersion: true, nullable: true),
                    StartFertigung = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabBauteilSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TabBauteilSet_TabMaschineSet_IdMaschine",
                        column: x => x.IdMaschine,
                        principalTable: "TabMaschineSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TabMeldungSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Aenderung = table.Column<DateTime>(nullable: false),
                    Anzahl = table.Column<int>(nullable: false),
                    Bemerkung = table.Column<string>(nullable: true),
                    IdBediener = table.Column<Guid>(nullable: false),
                    IdMaschine = table.Column<Guid>(nullable: false),
                    Modifikation = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Vorgang = table.Column<int>(nullable: false),
                    ZeitMeldung = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabMeldungSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TabMeldungSet_TabBedienerSet_IdBediener",
                        column: x => x.IdBediener,
                        principalTable: "TabBedienerSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TabMeldungSet_TabMaschineSet_IdMaschine",
                        column: x => x.IdMaschine,
                        principalTable: "TabMaschineSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TabBedienerBauteilSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Aenderung = table.Column<DateTime>(nullable: false),
                    IdBauteil = table.Column<Guid>(nullable: false),
                    IdBediener = table.Column<Guid>(nullable: false),
                    Modifikation = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabBedienerBauteilSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TabBedienerBauteilSet_TabBauteilSet_IdBauteil",
                        column: x => x.IdBauteil,
                        principalTable: "TabBauteilSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TabBedienerBauteilSet_TabBedienerSet_IdBediener",
                        column: x => x.IdBediener,
                        principalTable: "TabBedienerSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TabBauteilSet_IdMaschine",
                table: "TabBauteilSet",
                column: "IdMaschine");

            migrationBuilder.CreateIndex(
                name: "IX_TabBedienerBauteilSet_IdBauteil",
                table: "TabBedienerBauteilSet",
                column: "IdBauteil");

            migrationBuilder.CreateIndex(
                name: "IX_TabBedienerBauteilSet_IdBediener",
                table: "TabBedienerBauteilSet",
                column: "IdBediener");

            migrationBuilder.CreateIndex(
                name: "IX_TabMeldungSet_IdBediener",
                table: "TabMeldungSet",
                column: "IdBediener");

            migrationBuilder.CreateIndex(
                name: "IX_TabMeldungSet_IdMaschine",
                table: "TabMeldungSet",
                column: "IdMaschine");

            migrationBuilder.AddForeignKey(
                name: "FK_TabMaschineSet_TabStandortSet_IdStandort",
                table: "TabMaschineSet",
                column: "IdStandort",
                principalTable: "TabStandortSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TabMaschineSet_TabStandortSet_IdStandort",
                table: "TabMaschineSet");

            migrationBuilder.DropTable(
                name: "TabBedienerBauteilSet");

            migrationBuilder.DropTable(
                name: "TabMeldungSet");

            migrationBuilder.DropTable(
                name: "TabBauteilSet");

            migrationBuilder.DropColumn(
                name: "Bemerkung",
                table: "TabMaschineSet");

            migrationBuilder.DropColumn(
                name: "IstAktiv",
                table: "TabMaschineSet");

            migrationBuilder.DropColumn(
                name: "MaschineArt",
                table: "TabMaschineSet");

            migrationBuilder.DropColumn(
                name: "MaschineIp",
                table: "TabMaschineSet");

            migrationBuilder.DropColumn(
                name: "MaschinePort",
                table: "TabMaschineSet");

            migrationBuilder.DropColumn(
                name: "SammelScannung",
                table: "TabMaschineSet");

            migrationBuilder.DropColumn(
                name: "ScannerMitDisplay",
                table: "TabMaschineSet");

            migrationBuilder.DropColumn(
                name: "VorschubProMeterInSek",
                table: "TabMaschineSet");

            migrationBuilder.RenameColumn(
                name: "ZeitProBiegungInSek",
                table: "TabMaschineSet",
                newName: "Port");

            migrationBuilder.RenameColumn(
                name: "ZeitProBauteilInSek",
                table: "TabMaschineSet",
                newName: "MaschinenArt");

            migrationBuilder.RenameColumn(
                name: "NummerScanner",
                table: "TabMaschineSet",
                newName: "IpAdresse");

            migrationBuilder.RenameColumn(
                name: "IdStandort",
                table: "TabMaschineSet",
                newName: "FStandort");

            migrationBuilder.RenameIndex(
                name: "IX_TabMaschineSet_IdStandort",
                table: "TabMaschineSet",
                newName: "IX_TabMaschineSet_FStandort");

            migrationBuilder.AddForeignKey(
                name: "FK_TabMaschineSet_TabStandortSet_FStandort",
                table: "TabMaschineSet",
                column: "FStandort",
                principalTable: "TabStandortSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
