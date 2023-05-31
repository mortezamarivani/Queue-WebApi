using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gx.DataLayer.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShopList",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<long>(nullable: false),
                    UpdateUserId = table.Column<long>(nullable: false),
                    fldTitle = table.Column<string>(maxLength: 400, nullable: false),
                    fldDescription = table.Column<string>(maxLength: 500, nullable: false),
                    fldTargetPrice = table.Column<double>(nullable: false),
                    fldTargetPriceCurrency = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<long>(nullable: false),
                    UpdateUserId = table.Column<long>(nullable: false),
                    fldMobile = table.Column<long>(nullable: false),
                    fldActiveCode = table.Column<string>(maxLength: 10, nullable: true),
                    fldActiveCodeExpireDate = table.Column<int>(nullable: false),
                    fldActiveCodeExpireTime = table.Column<int>(nullable: false),
                    IsActivated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopDetail",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<long>(nullable: false),
                    UpdateUserId = table.Column<long>(nullable: false),
                    fldUrl = table.Column<string>(maxLength: 1500, nullable: false),
                    fldDescription = table.Column<string>(maxLength: 500, nullable: true),
                    fldMinPrice = table.Column<double>(nullable: false),
                    fldCount = table.Column<short>(nullable: false),
                    fldRealPrice = table.Column<double>(nullable: false),
                    fldCurrency = table.Column<string>(nullable: true),
                    fldCurrenySymbol = table.Column<string>(nullable: true),
                    fldGetData = table.Column<bool>(nullable: false),
                    ShopListId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopDetail_ShopList_ShopListId",
                        column: x => x.ShopListId,
                        principalTable: "ShopList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopDetail_ShopListId",
                table: "ShopDetail",
                column: "ShopListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopDetail");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "ShopList");
        }
    }
}
