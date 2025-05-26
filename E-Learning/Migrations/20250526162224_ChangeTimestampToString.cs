using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTimestampToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LessonNotes",
                keyColumn: "Timestamp",
                keyValue: null,
                column: "Timestamp",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Timestamp",
                table: "LessonNotes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Timestamp",
                table: "LessonNotes",
                type: "time(6)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
