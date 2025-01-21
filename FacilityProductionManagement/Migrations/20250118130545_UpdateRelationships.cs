using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FacilityProductionManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "EquipmentType",
                schema: "dbo",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Area = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentType", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Facility",
                schema: "dbo",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StandardArea = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentContract",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EquipmentTypeCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentContract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentContract_EquipmentType_EquipmentTypeCode",
                        column: x => x.EquipmentTypeCode,
                        principalSchema: "dbo",
                        principalTable: "EquipmentType",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentContract_Facility_FacilityCode",
                        column: x => x.FacilityCode,
                        principalSchema: "dbo",
                        principalTable: "Facility",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentContract_EquipmentTypeCode",
                schema: "dbo",
                table: "EquipmentContract",
                column: "EquipmentTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentContract_FacilityCode",
                schema: "dbo",
                table: "EquipmentContract",
                column: "FacilityCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentContract",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EquipmentType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Facility",
                schema: "dbo");
        }
    }
}
