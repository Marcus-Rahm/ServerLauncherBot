using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServerConfigs",
                columns: table => new
                {
                    ServerInfoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConfigName = table.Column<string>(type: "TEXT", nullable: true),
                    GameVersion = table.Column<string>(type: "TEXT", nullable: true),
                    ThumbnailURL = table.Column<string>(type: "TEXT", nullable: true),
                    ServerFileURL = table.Column<string>(type: "TEXT", nullable: true),
                    GameType = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerConfigs", x => x.ServerInfoId);
                });

            migrationBuilder.CreateTable(
                name: "ServerInfos",
                columns: table => new
                {
                    ServerInfoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServerName = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ArchiveFolder = table.Column<ulong>(type: "datetime", nullable: false),
                    ServerIP = table.Column<string>(type: "TEXT", nullable: true),
                    IsRunning = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsInitialized = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastActive = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ArchiveFolder1 = table.Column<string>(type: "TEXT", nullable: true),
                    ServerConfigId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerInfos", x => x.ServerInfoId);
                    table.ForeignKey(
                        name: "FK_ServerInfos_ServerConfigs_ServerConfigId",
                        column: x => x.ServerConfigId,
                        principalTable: "ServerConfigs",
                        principalColumn: "ServerInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServerInfos_ServerConfigId",
                table: "ServerInfos",
                column: "ServerConfigId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServerInfos");

            migrationBuilder.DropTable(
                name: "ServerConfigs");
        }
    }
}
