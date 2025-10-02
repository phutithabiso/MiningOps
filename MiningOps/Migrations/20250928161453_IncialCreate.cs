using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiningOps.Migrations
{
    /// <inheritdoc />
    public partial class IncialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegisterMiningDb",
                columns: table => new
                {
                    AccId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterMiningDb", x => x.AccId);
                });

            migrationBuilder.CreateTable(
                name: "WarehousesDb",
                columns: table => new
                {
                    WarehouseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehousesDb", x => x.WarehouseId);
                });

            migrationBuilder.CreateTable(
                name: "AdminProfiles",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccId = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CanManageUsers = table.Column<bool>(type: "bit", nullable: false),
                    CanApproveRequests = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminProfiles", x => x.AdminId);
                    table.ForeignKey(
                        name: "FK_AdminProfiles_RegisterMiningDb_AccId",
                        column: x => x.AccId,
                        principalTable: "RegisterMiningDb",
                        principalColumn: "AccId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupervisorProfiles",
                columns: table => new
                {
                    SupervisorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccId = table.Column<int>(type: "int", nullable: false),
                    Team = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MineLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Shift = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CanViewReports = table.Column<bool>(type: "bit", nullable: false),
                    CanManageTasks = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupervisorProfiles", x => x.SupervisorId);
                    table.ForeignKey(
                        name: "FK_SupervisorProfiles_RegisterMiningDb_AccId",
                        column: x => x.AccId,
                        principalTable: "RegisterMiningDb",
                        principalColumn: "AccId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierProfiles",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CanViewOrders = table.Column<bool>(type: "bit", nullable: false),
                    CanManageInventory = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierProfiles", x => x.SupplierId);
                    table.ForeignKey(
                        name: "FK_SupplierProfiles_RegisterMiningDb_AccId",
                        column: x => x.AccId,
                        principalTable: "RegisterMiningDb",
                        principalColumn: "AccId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryDb",
                columns: table => new
                {
                    InventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ReorderLevel = table.Column<int>(type: "int", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryDb", x => x.InventoryId);
                    table.ForeignKey(
                        name: "FK_InventoryDb_WarehousesDb_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "WarehousesDb",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminProfiles_AccId",
                table: "AdminProfiles",
                column: "AccId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDb_WarehouseId",
                table: "InventoryDb",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorProfiles_AccId",
                table: "SupervisorProfiles",
                column: "AccId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplierProfiles_AccId",
                table: "SupplierProfiles",
                column: "AccId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminProfiles");

            migrationBuilder.DropTable(
                name: "InventoryDb");

            migrationBuilder.DropTable(
                name: "SupervisorProfiles");

            migrationBuilder.DropTable(
                name: "SupplierProfiles");

            migrationBuilder.DropTable(
                name: "WarehousesDb");

            migrationBuilder.DropTable(
                name: "RegisterMiningDb");
        }
    }
}
