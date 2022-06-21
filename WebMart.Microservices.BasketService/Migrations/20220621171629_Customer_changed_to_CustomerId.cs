using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMart.Microservices.BasketService.Migrations
{
    public partial class Customer_changed_to_CustomerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Customer",
                table: "Baskets");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "Baskets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Baskets");

            migrationBuilder.AddColumn<string>(
                name: "Customer",
                table: "Baskets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
