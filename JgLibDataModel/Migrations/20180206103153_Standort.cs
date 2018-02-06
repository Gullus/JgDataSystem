using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JgLibDataModel.Migrations
{
    public partial class Standort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FStandort",
                table: "TabMaschineSet",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.CreateIndex(
                name: "IX_TabMaschineSet_FStandort",
                table: "TabMaschineSet",
                column: "FStandort");

            migrationBuilder.AddForeignKey(
                name: "FK_TabMaschineSet_TabStandort_FStandort",
                table: "TabMaschineSet",
                column: "FStandort",
                principalTable: "TabStandort",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TabMaschineSet_TabStandort_FStandort",
                table: "TabMaschineSet");

            migrationBuilder.DropTable(
                name: "TabStandort");

            migrationBuilder.DropIndex(
                name: "IX_TabMaschineSet_FStandort",
                table: "TabMaschineSet");

            migrationBuilder.DropColumn(
                name: "FStandort",
                table: "TabMaschineSet");
        }
    }
}
