using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JgLibDataModel.Migrations
{
    public partial class AnzahlBauteil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdBauteilJgData",
                table: "TabBauteilSet",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnzahlTeile",
                table: "TabBauteilSet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TabBauteilSet_IdBauteilJgData",
                table: "TabBauteilSet",
                column: "IdBauteilJgData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TabBauteilSet_IdBauteilJgData",
                table: "TabBauteilSet");

            migrationBuilder.DropColumn(
                name: "AnzahlTeile",
                table: "TabBauteilSet");

            migrationBuilder.AlterColumn<string>(
                name: "IdBauteilJgData",
                table: "TabBauteilSet",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
