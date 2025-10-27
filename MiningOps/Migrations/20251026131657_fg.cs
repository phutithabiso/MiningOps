using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiningOps.Migrations
{
    /// <inheritdoc />
    public partial class fg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey( 
                name: "FK_MaterialRequestsDb_PurchaseOrdersDb_PurchaseOrderId",
                table: "MaterialRequestsDb");

            migrationBuilder.DropIndex(
                name: "IX_MaterialRequestsDb_PurchaseOrderId",
                table: "MaterialRequestsDb");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                table: "MaterialRequestsDb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderId",
                table: "MaterialRequestsDb",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaterialRequestsDb_PurchaseOrderId",
                table: "MaterialRequestsDb",
                column: "PurchaseOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialRequestsDb_PurchaseOrdersDb_PurchaseOrderId",
                table: "MaterialRequestsDb",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrdersDb",
                principalColumn: "OrderId");
        }
    }
}
