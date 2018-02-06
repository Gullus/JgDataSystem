using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JgLibDataModel.Migrations
{
    public partial class Standort1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TabMaschineSet_TabStandort_FStandort",
                table: "TabMaschineSet");

            migrationBuilder.DropTable(
                name: "TabStandort");

            migrationBuilder.CreateTable(
                name: "TabStandortSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Aenderung = table.Column<DateTime>(nullable: false),
                    StandortName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabStandortSet", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_TabMaschineSet_TabStandortSet_FStandort",
                table: "TabMaschineSet",
                column: "FStandort",
                principalTable: "TabStandortSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TabMaschineSet_TabStandortSet_FStandort",
                table: "TabMaschineSet");

            migrationBuilder.DropTable(
                name: "TabStandortSet");

            migrationBuilder.CreateTable(
                name: "TabStandort",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Aenderung = table.Column<DateTime>(nullable: false),
                    StandortName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabStandort", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_TabMaschineSet_TabStandort_FStandort",
                table: "TabMaschineSet",
                column: "FStandort",
                principalTable: "TabStandort",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
