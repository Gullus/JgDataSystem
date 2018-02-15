using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JgLibDataModel.Migrations
{
    public partial class VorgangMeldung : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Program",
                table: "TabMeldungSet");

            migrationBuilder.AddColumn<int>(
                name: "Meldung",
                table: "TabMeldungSet",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Meldung",
                table: "TabMeldungSet");

            migrationBuilder.AddColumn<int>(
                name: "Program",
                table: "TabMeldungSet",
                nullable: false,
                defaultValue: 0);
        }
    }
}
