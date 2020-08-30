using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtualDesk.Migrations
{
    public partial class start : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accountsLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Pass = table.Column<string>(nullable: true),
                    EmailAdress = table.Column<string>(nullable: true),
                    SmtpServer = table.Column<string>(nullable: true),
                    SmtpPort = table.Column<int>(nullable: false),
                    PopServer = table.Column<string>(nullable: true),
                    PopPort = table.Column<int>(nullable: false),
                    IsPrivate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountsLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "emailAccountLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emailAccountLists", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accountsLists");

            migrationBuilder.DropTable(
                name: "emailAccountLists");
        }
    }
}
