using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace driveX_Api.Migrations
{
    /// <inheritdoc />
    public partial class initDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "Varchar(100)", nullable: false),
                    FirstName = table.Column<string>(type: "Varchar(100)", nullable: false),
                    LastName = table.Column<string>(type: "varchar(100)", nullable: false),
                    Password = table.Column<string>(type: "varchar(100)", nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false),
                    Phone = table.Column<string>(type: "varchar(15)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "Varchar(100)", nullable: false),
                    UserId = table.Column<string>(type: "Varchar(100)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    Extension = table.Column<string>(type: "varchar(10)", nullable: false),
                    ParentId = table.Column<string>(type: "Varchar(100)", nullable: false),
                    Path = table.Column<string>(type: "varchar(max)", nullable: false),
                    Trashed = table.Column<bool>(type: "bit", nullable: false),
                    IsFile = table.Column<bool>(type: "bit", nullable: false),
                    Starred = table.Column<bool>(type: "bit", nullable: false),
                    Label = table.Column<string>(type: "varchar(255)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileDetails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FileStorages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "Varchar(100)", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileStorages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileStorages_FileDetails_Id",
                        column: x => x.Id,
                        principalTable: "FileDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedDetails",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "Varchar(100)", nullable: false),
                    DetailsId = table.Column<string>(type: "Varchar(100)", nullable: false),
                    SharedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedDetails", x => new { x.UserId, x.DetailsId });
                    table.ForeignKey(
                        name: "FK_SharedDetails_FileDetails_DetailsId",
                        column: x => x.DetailsId,
                        principalTable: "FileDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SharedDetails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileDetails_UserId",
                table: "FileDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedDetails_DetailsId",
                table: "SharedDetails",
                column: "DetailsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileStorages");

            migrationBuilder.DropTable(
                name: "SharedDetails");

            migrationBuilder.DropTable(
                name: "FileDetails");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
