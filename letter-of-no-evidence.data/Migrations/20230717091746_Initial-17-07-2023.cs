using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace letter_of_no_evidence.data.Migrations
{
    /// <inheritdoc />
    public partial class Initial17072023 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Meaning = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SubjectFirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubjectLastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AlternativeFirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AlternativeLastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateOfBirth = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DateOfDeath = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CountryOfBirth = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactTitle = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ContactFirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactLastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactAddress1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactAddress2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactCounty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactPostCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ContactCountry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LetterToRequestor = table.Column<bool>(type: "bit", nullable: false),
                    AgentFullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AgentAddress1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AgentAddress2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AgentCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AgentCounty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AgentPostCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    AgentCountry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PaymentStatusId = table.Column<int>(type: "int", nullable: false),
                    ProcessFinished = table.Column<bool>(type: "bit", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    RequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentStatus_PaymentStatusId",
                        column: x => x.PaymentStatusId,
                        principalTable: "PaymentStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentStatusId",
                table: "Payments",
                column: "PaymentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_RequestId",
                table: "Payments",
                column: "RequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PaymentStatus");

            migrationBuilder.DropTable(
                name: "Requests");
        }
    }
}
