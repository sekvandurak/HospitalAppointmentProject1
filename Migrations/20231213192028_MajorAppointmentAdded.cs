using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalAppointmentProject1.Migrations
{
    public partial class MajorAppointmentAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Major",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Major",
                table: "Appointments");
        }
    }
}
