using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnZipOnline.Migrations
{
    /// <inheritdoc />
    public partial class newFileTableStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Files",
                newName: "ExtractedFiles");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Files",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "ExtractedFiles",
                table: "Files",
                newName: "Path");
        }
    }
}
