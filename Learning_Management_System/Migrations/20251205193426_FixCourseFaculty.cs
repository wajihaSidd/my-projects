using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Learning_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class FixCourseFaculty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseFaculty",
                table: "CourseFaculty");

            migrationBuilder.DropIndex(
                name: "IX_CourseFaculty_CourseId",
                table: "CourseFaculty");

            migrationBuilder.DropColumn(
                name: "CourseFacultyId",
                table: "CourseFaculty");

            migrationBuilder.AlterColumn<int>(
                name: "FacultyId",
                table: "CourseFaculty",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "CourseFaculty",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseFaculty",
                table: "CourseFaculty",
                columns: new[] { "CourseId", "FacultyId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseFaculty",
                table: "CourseFaculty");

            migrationBuilder.AlterColumn<int>(
                name: "FacultyId",
                table: "CourseFaculty",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "CourseFaculty",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseFacultyId",
                table: "CourseFaculty",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseFaculty",
                table: "CourseFaculty",
                column: "CourseFacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseFaculty_CourseId",
                table: "CourseFaculty",
                column: "CourseId");
        }
    }
}
