using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelReservationManager.Migrations
{
    public partial class HotelDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    First_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Last_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    E_mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    Arrival_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Departure_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Breakfast = table.Column<bool>(type: "bit", nullable: false),
                    All_inclusive = table.Column<bool>(type: "bit", nullable: false),
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
                    Is_Available = table.Column<bool>(type: "bit", nullable: false),
                    Price_Adult = table.Column<double>(type: "float", nullable: false),
                    Price_Child = table.Column<double>(type: "float", nullable: false)
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
                    First_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Second_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_Ádministrator = table.Column<bool>(type: "bit", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    E_mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hire_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Is_active = table.Column<bool>(type: "bit", nullable: false),
                    Release_date = table.Column<DateTime>(type: "datetime2", nullable: false)
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
