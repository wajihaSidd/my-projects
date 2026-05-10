using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Courses_CourseId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Faculty_MarkedBy",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Students_StudentId",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Faculty");

            migrationBuilder.DropColumn(
                name: "DepartmentCode",
                table: "Department");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Students",
                newName: "StudentPassword");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Students",
                newName: "StudentName");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Students",
                newName: "EmailAddress");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Students",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Faculty",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Faculty",
                newName: "FacultyPassword");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Faculty",
                newName: "FacultyName");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Faculty",
                newName: "FacultyEmail");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Faculty",
                newName: "FacultyId");

            migrationBuilder.RenameColumn(
                name: "DepartmentName",
                table: "Department",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CourseStudents",
                newName: "CourseStudentId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Courses",
                newName: "CourseId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CourseFaculty",
                newName: "CourseFacultyId");

            migrationBuilder.RenameColumn(
                name: "MarkedBy",
                table: "Attendance",
                newName: "FacultyId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Attendance",
                newName: "AttendanceId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendance_MarkedBy",
                table: "Attendance",
                newName: "IX_Attendance_FacultyId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Attendance",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "Attendance",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Courses_CourseId",
                table: "Attendance",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Faculty_FacultyId",
                table: "Attendance",
                column: "FacultyId",
                principalTable: "Faculty",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Students_StudentId",
                table: "Attendance",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Courses_CourseId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Faculty_FacultyId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Students_StudentId",
                table: "Attendance");

            migrationBuilder.RenameColumn(
                name: "StudentPassword",
                table: "Students",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "StudentName",
                table: "Students",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                table: "Students",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Students",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Faculty",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "FacultyPassword",
                table: "Faculty",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "FacultyName",
                table: "Faculty",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FacultyEmail",
                table: "Faculty",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "FacultyId",
                table: "Faculty",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Department",
                newName: "DepartmentName");

            migrationBuilder.RenameColumn(
                name: "CourseStudentId",
                table: "CourseStudents",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Courses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CourseFacultyId",
                table: "CourseFaculty",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FacultyId",
                table: "Attendance",
                newName: "MarkedBy");

            migrationBuilder.RenameColumn(
                name: "AttendanceId",
                table: "Attendance",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Attendance_FacultyId",
                table: "Attendance",
                newName: "IX_Attendance_MarkedBy");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Faculty",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentCode",
                table: "Department",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Attendance",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "Attendance",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Courses_CourseId",
                table: "Attendance",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Faculty_MarkedBy",
                table: "Attendance",
                column: "MarkedBy",
                principalTable: "Faculty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Students_StudentId",
                table: "Attendance",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
