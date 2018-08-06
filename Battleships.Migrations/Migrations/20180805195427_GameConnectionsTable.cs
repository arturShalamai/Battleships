using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Battleships.Migrations.Migrations
{
    public partial class GameConnectionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameConnections",
                columns: table => new
                {
                    ConnectionId = table.Column<string>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameConnections", x => x.ConnectionId);
                    table.ForeignKey(
                        name: "FK_GameConnections_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameConnections_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameConnections_GameId",
                table: "GameConnections",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameConnections_PlayerId",
                table: "GameConnections",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameConnections");
        }
    }
}
