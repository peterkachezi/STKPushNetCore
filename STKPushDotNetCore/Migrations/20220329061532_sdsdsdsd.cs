using Microsoft.EntityFrameworkCore.Migrations;

namespace STKPushDotNetCore.Migrations
{
    public partial class sdsdsdsd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckoutRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantRequestID = table.Column<string>(nullable: true),
                    CheckoutRequestID = table.Column<string>(nullable: true),
                    ResponseCode = table.Column<string>(nullable: true),
                    ResponseDescription = table.Column<string>(nullable: true),
                    CustomerMessage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckoutRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MpesaPayments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantRequestID = table.Column<string>(nullable: true),
                    CheckoutRequestID = table.Column<string>(nullable: true),
                    ResultCode = table.Column<int>(nullable: true),
                    ResultDesc = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    TransactionNumber = table.Column<string>(nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    TransactionDate = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MpesaPayments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckoutRequests");

            migrationBuilder.DropTable(
                name: "MpesaPayments");
        }
    }
}
