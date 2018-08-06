using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Battleships.Migrations.Migrations
{
    public partial class GameConnectionsId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameConnections",
                table: "GameConnections");

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionId",
                table: "GameConnections",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GameConnections",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameConnections",
                table: "GameConnections",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameConnections",
                table: "GameConnections");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GameConnections");

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionId",
                table: "GameConnections",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameConnections",
                table: "GameConnections",
                column: "ConnectionId");
        }
    }
}
