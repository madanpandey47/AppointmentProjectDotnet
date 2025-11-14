using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateGetAllAppointmentsSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"
                IF OBJECT_ID('sp_GetAllAppointments', 'P') IS NOT NULL
                    DROP PROCEDURE sp_GetAllAppointments;
            ";
            migrationBuilder.Sql(sp);

            var sp2 = @"
                CREATE PROCEDURE sp_GetAllAppointments
                AS
                BEGIN
                    SELECT * FROM Appointments;
                END";
            migrationBuilder.Sql(sp2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"DROP PROCEDURE sp_GetAllAppointments";
            migrationBuilder.Sql(sp);
        }
    }
}
