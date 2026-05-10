using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Learning_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class createReecap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reecaps",
                columns: table => new
                {
                    ReecapId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CourseStudentId = table.Column<int>(type: "integer", nullable: false),
                    Ass1 = table.Column<int>(type: "integer", nullable: true),
                    Ass2 = table.Column<int>(type: "integer", nullable: true),
                    Quiz1 = table.Column<int>(type: "integer", nullable: true),
                    Quiz2 = table.Column<int>(type: "integer", nullable: true),
                    Mid = table.Column<int>(type: "integer", nullable: true),
                    Final = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reecaps", x => x.ReecapId);
                    table.ForeignKey(
                        name: "FK_Reecaps_CourseStudents_CourseStudentId",
                        column: x => x.CourseStudentId,
                        principalTable: "CourseStudents",
                        principalColumn: "CourseStudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reecaps_CourseStudentId",
                table: "Reecaps",
                column: "CourseStudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reecaps");
        }
    }
}
