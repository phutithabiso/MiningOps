using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiningOps.Migrations
{
    /// <inheritdoc />
    public partial class fixMaterialRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoginViewModel",
                columns: table => new
                {
                    usernameoremail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginViewModel", x => x.usernameoremail);
                });

            migrationBuilder.CreateTable(
                name: "MaterialRequestsDb",
                columns: table => new
                {
                    MaterialRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialRequestsDb", x => x.MaterialRequestId);
                });

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

            migrationBuilder.CreateTable(
                name: "PurchaseOrdersDb",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    RequestedBy = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MaterialRequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrdersDb", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrdersDb_MaterialRequestsDb_MaterialRequestId",
                        column: x => x.MaterialRequestId,
                        principalTable: "MaterialRequestsDb",
                        principalColumn: "MaterialRequestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrdersDb_RegisterMiningDb_RequestedBy",
                        column: x => x.RequestedBy,
                        principalTable: "RegisterMiningDb",
                        principalColumn: "AccId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrdersDb_SupplierProfiles_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "SupplierProfiles",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplierContractDb",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ContractTerms = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PaymentTerms = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContractValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ContractType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierContractDb", x => x.ContractId);
                    table.ForeignKey(
                        name: "FK_SupplierContractDb_SupplierProfiles_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "SupplierProfiles",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierPerformanceDb",
                columns: table => new
                {
                    PerformanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    OnTimeDeliveryRate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    QualityRating = table.Column<int>(type: "int", nullable: false),
                    ComplianceScore = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierPerformanceDb", x => x.PerformanceId);
                    table.ForeignKey(
                        name: "FK_SupplierPerformanceDb_SupplierProfiles_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "SupplierProfiles",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoicesDb",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    InvoiceReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicesDb", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_InvoicesDb_PurchaseOrdersDb_OrderId",
                        column: x => x.OrderId,
                        principalTable: "PurchaseOrdersDb",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemsDb",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemsDb", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItemsDb_PurchaseOrdersDb_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrdersDb",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentsDb",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    PaymentReference = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsDb", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_PaymentsDb_InvoicesDb_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "InvoicesDb",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentsDb_RegisterMiningDb_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "RegisterMiningDb",
                        principalColumn: "AccId");
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
                name: "IX_InvoicesDb_OrderId",
                table: "InvoicesDb",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemsDb_PurchaseOrderId",
                table: "OrderItemsDb",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsDb_ApprovedById",
                table: "PaymentsDb",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsDb_InvoiceId",
                table: "PaymentsDb",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrdersDb_MaterialRequestId",
                table: "PurchaseOrdersDb",
                column: "MaterialRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrdersDb_RequestedBy",
                table: "PurchaseOrdersDb",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrdersDb_SupplierId",
                table: "PurchaseOrdersDb",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorProfiles_AccId",
                table: "SupervisorProfiles",
                column: "AccId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplierContractDb_SupplierId",
                table: "SupplierContractDb",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierPerformanceDb_SupplierId",
                table: "SupplierPerformanceDb",
                column: "SupplierId");

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
                name: "LoginViewModel");

            migrationBuilder.DropTable(
                name: "OrderItemsDb");

            migrationBuilder.DropTable(
                name: "PaymentsDb");

            migrationBuilder.DropTable(
                name: "SupervisorProfiles");

            migrationBuilder.DropTable(
                name: "SupplierContractDb");

            migrationBuilder.DropTable(
                name: "SupplierPerformanceDb");

            migrationBuilder.DropTable(
                name: "WarehousesDb");

            migrationBuilder.DropTable(
                name: "InvoicesDb");

            migrationBuilder.DropTable(
                name: "PurchaseOrdersDb");

            migrationBuilder.DropTable(
                name: "MaterialRequestsDb");

            migrationBuilder.DropTable(
                name: "SupplierProfiles");

            migrationBuilder.DropTable(
                name: "RegisterMiningDb");
        }
    }
}
