using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class AddProfilePictureToStudentDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "StudentDetails",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "StudentDetails");
        }
    }
}
