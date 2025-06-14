using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web6.Migrations
{
    /// <inheritdoc />
    public partial class AddSecretQuestionColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ban = table.Column<bool>(type: "bit", nullable: false),
                    auth1 = table.Column<bool>(type: "bit", nullable: false),
                    auth2 = table.Column<bool>(type: "bit", nullable: false),
                    auth3 = table.Column<bool>(type: "bit", nullable: false),
                    Q1_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Q1_answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Q2_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Q2_answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Q3_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Q3_answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

                    }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropTable(
                name: "Employees");

                    }
    }
}
