using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Battleships.Migrations.Migrations
{
    public partial class GameConnections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerConnections",
                table: "PlayerConnections");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "PlayerConnections");

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionId",
                table: "PlayerConnections",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerConnections",
                table: "PlayerConnections",
                column: "ConnectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerConnections",
                table: "PlayerConnections");

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionId",
                table: "PlayerConnections",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "PlayerConnections",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerConnections",
                table: "PlayerConnections",
                column: "Id");
        }
    }
}
