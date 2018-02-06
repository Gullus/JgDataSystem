using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JgLibDataModel.Migrations
{
    public partial class Daten1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ip",
                table: "TabMaschineSet",
                newName: "IpAdresse");

            migrationBuilder.AddColumn<byte[]>(
                name: "Modifikation",
                table: "TabStandortSet",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaschinenArt",
                table: "TabMaschineSet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "Modifikation",
                table: "TabMaschineSet",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Modifikation",
                table: "TabBedienerSet",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Modifikation",
                table: "TabStandortSet");

            migrationBuilder.DropColumn(
                name: "MaschinenArt",
                table: "TabMaschineSet");

            migrationBuilder.DropColumn(
                name: "Modifikation",
                table: "TabMaschineSet");

            migrationBuilder.DropColumn(
                name: "Modifikation",
                table: "TabBedienerSet");

            migrationBuilder.RenameColumn(
                name: "IpAdresse",
                table: "TabMaschineSet",
                newName: "Ip");
        }
    }
}
