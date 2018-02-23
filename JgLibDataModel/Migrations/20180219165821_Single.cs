using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JgLibDataModel.Migrations
{
    public partial class Single : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "ZeitProBiegungInSek",
                table: "TabMaschineSet",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "ZeitProBauteilInSek",
                table: "TabMaschineSet",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "VorschubProMeterInSek",
                table: "TabMaschineSet",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ZeitProBiegungInSek",
                table: "TabMaschineSet",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "ZeitProBauteilInSek",
                table: "TabMaschineSet",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "VorschubProMeterInSek",
                table: "TabMaschineSet",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
