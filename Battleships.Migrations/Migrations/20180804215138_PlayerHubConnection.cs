using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Battleships.Migrations.Migrations
{
    public partial class PlayerHubConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerConnections",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Connected = table.Column<bool>(nullable: false),
                    ConnectionId = table.Column<string>(nullable: true),
                    PlayerId = table.Column<Guid>(nullable: false),
                    UserAgent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerConnections_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerConnections_PlayerId",
                table: "PlayerConnections",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerConnections");
        }
    }
}
