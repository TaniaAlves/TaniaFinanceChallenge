using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BuyRequest.Data.Migrations
{
    public partial class UpdateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRequests_BuyRequests_BuyRequestsId",
                table: "ProductRequests");

            migrationBuilder.DropIndex(
                name: "IX_ProductRequests_BuyRequestsId",
                table: "ProductRequests");

            migrationBuilder.DropColumn(
                name: "BuyRequestsId",
                table: "ProductRequests");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                table: "ProductRequests",
                newName: "BuyRequestId");

            migrationBuilder.AlterColumn<string>(
                name: "ProductDescription",
                table: "ProductRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequests_BuyRequestId",
                table: "ProductRequests",
                column: "BuyRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRequests_BuyRequests_BuyRequestId",
                table: "ProductRequests",
                column: "BuyRequestId",
                principalTable: "BuyRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRequests_BuyRequests_BuyRequestId",
                table: "ProductRequests");

            migrationBuilder.DropIndex(
                name: "IX_ProductRequests_BuyRequestId",
                table: "ProductRequests");

            migrationBuilder.RenameColumn(
                name: "BuyRequestId",
                table: "ProductRequests",
                newName: "RequestId");

            migrationBuilder.AlterColumn<string>(
                name: "ProductDescription",
                table: "ProductRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "BuyRequestsId",
                table: "ProductRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequests_BuyRequestsId",
                table: "ProductRequests",
                column: "BuyRequestsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRequests_BuyRequests_BuyRequestsId",
                table: "ProductRequests",
                column: "BuyRequestsId",
                principalTable: "BuyRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
