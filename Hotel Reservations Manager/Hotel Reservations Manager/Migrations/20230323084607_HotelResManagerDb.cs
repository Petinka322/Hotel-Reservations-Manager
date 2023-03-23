using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelReservationsManager.Migrations
{
    /// <inheritdoc />
    public partial class HotelResManagerDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(name: "First_Name", type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(name: "Last_Name", type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(name: "E_mail", type: "nvarchar(max)", nullable: false),
                    Adult = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ResId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomsId = table.Column<int>(type: "int", nullable: false),
                    Usename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Arrivaldate = table.Column<DateTime>(name: "Arrival_date", type: "datetime2", nullable: false),
                    Departuredate = table.Column<DateTime>(name: "Departure_date", type: "datetime2", nullable: false),
                    Breakfast = table.Column<bool>(type: "bit", nullable: false),
                    Allinclusive = table.Column<bool>(name: "All_inclusive", type: "bit", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ResId);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomsCapacity = table.Column<int>(type: "int", nullable: false),
                    RoomsType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAvailable = table.Column<bool>(name: "Is_Available", type: "bit", nullable: false),
                    PriceAdult = table.Column<double>(name: "Price_Adult", type: "float", nullable: false),
                    PriceChild = table.Column<double>(name: "Price_Child", type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomsId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    EGN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Firstname = table.Column<string>(name: "First_name", type: "nvarchar(max)", nullable: false),
                    Secondname = table.Column<string>(name: "Second_name", type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(name: "Last_name", type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(name: "E_mail", type: "nvarchar(max)", nullable: false),
                    Hiredate = table.Column<DateTime>(name: "Hire_date", type: "datetime2", nullable: false),
                    Isactive = table.Column<bool>(name: "Is_active", type: "bit", nullable: false),
                    Releasedate = table.Column<DateTime>(name: "Release_date", type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.EGN);
                });

            migrationBuilder.CreateTable(
                name: "ClientsReservation",
                columns: table => new
                {
                    ClientsClientId = table.Column<int>(type: "int", nullable: false),
                    ReservationsResId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientsReservation", x => new { x.ClientsClientId, x.ReservationsResId });
                    table.ForeignKey(
                        name: "FK_ClientsReservation_Clients_ClientsClientId",
                        column: x => x.ClientsClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientsReservation_Reservations_ReservationsResId",
                        column: x => x.ReservationsResId,
                        principalTable: "Reservations",
                        principalColumn: "ResId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientsReservation_ReservationsResId",
                table: "ClientsReservation",
                column: "ReservationsResId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientsReservation");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Reservations");
        }
    }
}
