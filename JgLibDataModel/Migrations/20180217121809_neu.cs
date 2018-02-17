using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JgLibDataModel.Migrations
{
    public partial class neu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TabBedienerSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Aenderung = table.Column<DateTime>(nullable: false),
                    Modifikation = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Nachname = table.Column<string>(maxLength: 30, nullable: false),
                    NummerAusweis = table.Column<string>(maxLength: 3, nullable: false),
                    Vorname = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabBedienerSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TabReportSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Aenderung = table.Column<DateTime>(nullable: false),
                    Beschreibung = table.Column<string>(nullable: true),
                    Modifikation = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ReportDaten = table.Column<byte[]>(nullable: true),
                    ReportName = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabReportSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TabStandortSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Aenderung = table.Column<DateTime>(nullable: false),
                    Modifikation = table.Column<byte[]>(rowVersion: true, nullable: true),
                    StandortName = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabStandortSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TabMaschineSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Aenderung = table.Column<DateTime>(nullable: false),
                    Bemerkung = table.Column<string>(nullable: true),
                    IdStandort = table.Column<Guid>(nullable: false),
                    IstAktiv = table.Column<bool>(nullable: false),
                    MaschineArt = table.Column<int>(nullable: false),
                    MaschineIp = table.Column<string>(nullable: true),
                    MaschineName = table.Column<string>(maxLength: 30, nullable: false),
                    MaschinePort = table.Column<int>(nullable: false),
                    Modifikation = table.Column<byte[]>(rowVersion: true, nullable: true),
                    NummerScanner = table.Column<string>(maxLength: 30, nullable: false),
                    SammelScannung = table.Column<bool>(nullable: false),
                    ScannerMitDisplay = table.Column<bool>(nullable: false),
                    StatusMaschine = table.Column<byte[]>(nullable: true),
                    VorschubProMeterInSek = table.Column<int>(nullable: false),
                    ZeitProBauteilInSek = table.Column<int>(nullable: false),
                    ZeitProBiegungInSek = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabMaschineSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TabMaschineSet_TabStandortSet_IdStandort",
                        column: x => x.IdStandort,
                        principalTable: "TabStandortSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TabBauteilSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Aenderung = table.Column<DateTime>(nullable: false),
                    AnzahlBiegungen = table.Column<int>(nullable: false),
                    AnzahlHelfer = table.Column<int>(nullable: false),
                    DuchmesserInMm = table.Column<int>(nullable: false),
                    EndeFertigung = table.Column<DateTime>(nullable: true),
                    GewichtInKg = table.Column<double>(nullable: false),
                    IdBauteilJgData = table.Column<string>(nullable: true),
                    IdBediener = table.Column<Guid>(nullable: false),
                    IdMaschine = table.Column<Guid>(nullable: false),
                    LaengeInCm = table.Column<int>(nullable: false),
                    Modifikation = table.Column<byte[]>(rowVersion: true, nullable: true),
                    StartFertigung = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabBauteilSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TabBauteilSet_TabBedienerSet_IdBediener",
                        column: x => x.IdBediener,
                        principalTable: "TabBedienerSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Anzahl = table.Column<int>(nullable: true),
                    Bemerkung = table.Column<string>(nullable: true),
                    IdBediener = table.Column<Guid>(nullable: false),
                    IdMaschine = table.Column<Guid>(nullable: false),
                    Meldung = table.Column<int>(nullable: false),
                    Modifikation = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ZeitAbmeldung = table.Column<DateTime>(nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_TabBauteilSet_IdBediener",
                table: "TabBauteilSet",
                column: "IdBediener");

            migrationBuilder.CreateIndex(
                name: "IX_TabBauteilSet_IdMaschine",
                table: "TabBauteilSet",
                column: "IdMaschine");

            migrationBuilder.CreateIndex(
                name: "IX_TabMaschineSet_IdStandort",
                table: "TabMaschineSet",
                column: "IdStandort");

            migrationBuilder.CreateIndex(
                name: "IX_TabMeldungSet_IdBediener",
                table: "TabMeldungSet",
                column: "IdBediener");

            migrationBuilder.CreateIndex(
                name: "IX_TabMeldungSet_IdMaschine",
                table: "TabMeldungSet",
                column: "IdMaschine");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TabBauteilSet");

            migrationBuilder.DropTable(
                name: "TabMeldungSet");

            migrationBuilder.DropTable(
                name: "TabReportSet");

            migrationBuilder.DropTable(
                name: "TabBedienerSet");

            migrationBuilder.DropTable(
                name: "TabMaschineSet");

            migrationBuilder.DropTable(
                name: "TabStandortSet");
        }
    }
}
