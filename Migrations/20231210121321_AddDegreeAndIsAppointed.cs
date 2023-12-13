using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalAppointmentProject1.Migrations
{
    public partial class AddDegreeAndIsAppointed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAppointed",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "degree",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAppointed",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "degree",
                table: "Doctors");
        }
    }
}
