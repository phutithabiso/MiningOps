using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiningOps.Migrations
{
    /// <inheritdoc />
    public partial class miningProcess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "IX_SupplierContractDb_SupplierId",
                table: "SupplierContractDb",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierPerformanceDb_SupplierId",
                table: "SupplierPerformanceDb",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItemsDb");

            migrationBuilder.DropTable(
                name: "PaymentsDb");

            migrationBuilder.DropTable(
                name: "SupplierContractDb");

            migrationBuilder.DropTable(
                name: "SupplierPerformanceDb");

            migrationBuilder.DropTable(
                name: "InvoicesDb");

            migrationBuilder.DropTable(
                name: "PurchaseOrdersDb");

            migrationBuilder.DropTable(
                name: "MaterialRequestsDb");
        }
    }
}
