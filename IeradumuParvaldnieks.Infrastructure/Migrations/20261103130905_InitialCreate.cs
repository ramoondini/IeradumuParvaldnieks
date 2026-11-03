using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IeradumuParvaldnieks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ieradumi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LietotajaId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nosaukums = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apraksts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Statuss = table.Column<int>(type: "int", nullable: false),
                    Izveidots = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Atjauninats = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Atkartosana_Biezums = table.Column<int>(type: "int", nullable: true),
                    Atkartosana_Intervals = table.Column<int>(type: "int", nullable: true),
                    Atkartosana_NedelasDienas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Atkartosana_BeiguDatums = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ieradumi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IeradumuIzpildes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IeradumaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Datums = table.Column<DateTime>(type: "date", nullable: false),
                    IrPabeigts = table.Column<bool>(type: "bit", nullable: false),
                    Piezimes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IeradumuIzpildes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IeradumuIzpildes_Ieradumi_IeradumaId",
                        column: x => x.IeradumaId,
                        principalTable: "Ieradumi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IeradumuIzpildes_IeradumaId_Datums",
                table: "IeradumuIzpildes",
                columns: new[] { "IeradumaId", "Datums" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IeradumuIzpildes");

            migrationBuilder.DropTable(
                name: "Ieradumi");
        }
    }
}
