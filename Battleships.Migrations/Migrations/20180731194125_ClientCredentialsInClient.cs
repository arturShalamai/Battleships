using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Battleships.Migrations.Migrations
{
    public partial class ClientCredentialsInClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Credentials_Id",
                table: "Players");

            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Players",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Players");

            migrationBuilder.CreateTable(
                name: "Credentials",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credentials", x => x.PlayerId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Credentials_Id",
                table: "Players",
                column: "Id",
                principalTable: "Credentials",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
