using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JgLibDataModel.Migrations
{
    public partial class Maschine1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Aenderung",
                table: "TabBedienerSet",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "TabMaschineSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Aenderung = table.Column<DateTime>(nullable: false),
                    Ip = table.Column<string>(nullable: true),
                    MaschineName = table.Column<string>(nullable: true),
                    Port = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabMaschineSet", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TabMaschineSet");

            migrationBuilder.DropColumn(
                name: "Aenderung",
                table: "TabBedienerSet");
        }
    }
}
