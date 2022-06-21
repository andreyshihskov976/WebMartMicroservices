using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMart.Microservices.BasketService.Migrations
{
    public partial class Description_field_was_added_to_Product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Product",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Product");
        }
    }
}
