using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace letter_of_no_evidence.data.Migrations
{
    /// <inheritdoc />
    public partial class AddTrackingDeliveryCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryZoneCost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ZoneNo = table.Column<int>(type: "int", nullable: false),
                    CostWithTracking = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryZoneCost", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryZoneCost");
        }
    }
}
