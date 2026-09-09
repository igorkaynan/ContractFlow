using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContractFlow.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LinkContractsToSuppliers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_SupplierId",
                table: "Contracts",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Suppliers_SupplierId",
                table: "Contracts",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Suppliers_SupplierId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_SupplierId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Contracts");
        }
    }
}
