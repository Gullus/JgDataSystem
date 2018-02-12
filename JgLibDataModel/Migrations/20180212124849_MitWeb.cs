using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JgLibDataModel.Migrations
{
    public partial class MitWeb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Vorgang",
                table: "TabMeldungSet");

            migrationBuilder.AlterColumn<int>(
                name: "Anzahl",
                table: "TabMeldungSet",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Program",
                table: "TabMeldungSet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "IdBauteilJgData",
                table: "TabBauteilSet",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<Guid>(
                name: "Bediener",
                table: "TabBauteilSet",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Program",
                table: "TabMeldungSet");

            migrationBuilder.DropColumn(
                name: "Bediener",
                table: "TabBauteilSet");

            migrationBuilder.AlterColumn<int>(
                name: "Anzahl",
                table: "TabMeldungSet",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Vorgang",
                table: "TabMeldungSet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "IdBauteilJgData",
                table: "TabBauteilSet",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
