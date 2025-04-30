using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace kurs.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dostavka",
                columns: table => new
                {
                    iddostavka = table.Column<int>(name: "id_dostavka", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    datapolucheniya = table.Column<DateOnly>(name: "data_polucheniya", type: "date", nullable: true),
                    ojdaemayadata = table.Column<DateOnly>(name: "ojdaemaya_data", type: "date", nullable: true),
                    statusdostavki = table.Column<string>(name: "status_dostavki", type: "character(20)", fixedLength: true, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("dostavka_pkey", x => x.iddostavka);
                });

            migrationBuilder.CreateTable(
                name: "pokupatel",
                columns: table => new
                {
                    idpokupatel = table.Column<int>(name: "id_pokupatel", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    pokupatelfio = table.Column<string>(name: "pokupatel_fio", type: "character(40)", fixedLength: true, maxLength: 40, nullable: false),
                    pokupateltelefon = table.Column<decimal>(name: "pokupatel_telefon", type: "numeric(11)", precision: 11, nullable: false),
                    pokupateladresdostavki = table.Column<string>(name: "pokupatel_adres_dostavki", type: "character(50)", fixedLength: true, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pokupatel_pkey", x => x.idpokupatel);
                });

            migrationBuilder.CreateTable(
                name: "sborschik_zakaza",
                columns: table => new
                {
                    idsborschikzakaza = table.Column<int>(name: "id_sborschik_zakaza", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    sborschikzakazatelefon = table.Column<decimal>(name: "sborschik_zakaza_telefon", type: "numeric(11)", precision: 11, nullable: false),
                    sborschikzakazanames = table.Column<string>(name: "sborschik_zakaza_names", type: "character(40)", fixedLength: true, maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sborschik_zakaza_pkey", x => x.idsborschikzakaza);
                });

            migrationBuilder.CreateTable(
                name: "tovar",
                columns: table => new
                {
                    idtovar = table.Column<int>(name: "id_tovar", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    cena = table.Column<decimal>(type: "numeric(10)", precision: 10, nullable: true),
                    kolichestvo = table.Column<int>(type: "integer", nullable: true),
                    tovarnaimenovanie = table.Column<string>(name: "tovar_naimenovanie", type: "character(100)", fixedLength: true, maxLength: 100, nullable: true),
                    idzakaz = table.Column<int>(name: "id_zakaz", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tovar_pkey", x => x.idtovar);
                });

            migrationBuilder.CreateTable(
                name: "zakaz",
                columns: table => new
                {
                    idzakaz = table.Column<int>(name: "id_zakaz", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    zakazdata = table.Column<DateTime>(name: "zakaz_data", type: "timestamp with time zone", nullable: false),
                    zakazsumma = table.Column<decimal>(name: "zakaz_summa", type: "numeric(5)", precision: 5, nullable: false),
                    idpokupatel = table.Column<int>(name: "id_pokupatel", type: "integer", nullable: false),
                    idsborschikzakaza = table.Column<int>(name: "id_sborschik_zakaza", type: "integer", nullable: false),
                    iddostavka = table.Column<int>(name: "id_dostavka", type: "integer", nullable: false),
                    TovarIdTovar = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("zakaz_pkey", x => x.idzakaz);
                    table.ForeignKey(
                        name: "FK_zakaz_tovar_TovarIdTovar",
                        column: x => x.TovarIdTovar,
                        principalTable: "tovar",
                        principalColumn: "id_tovar");
                    table.ForeignKey(
                        name: "zakaz_dostavka",
                        column: x => x.iddostavka,
                        principalTable: "dostavka",
                        principalColumn: "id_dostavka",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "zakaz_id_pokupatel",
                        column: x => x.idpokupatel,
                        principalTable: "pokupatel",
                        principalColumn: "id_pokupatel",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "zakaz_id_sborschik_zakaza",
                        column: x => x.idsborschikzakaza,
                        principalTable: "sborschik_zakaza",
                        principalColumn: "id_sborschik_zakaza",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "pokupatel_pokupatel_telefon_key",
                table: "pokupatel",
                column: "pokupatel_telefon",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "sborschik_zakaza_sborschik_zakaza_telefon_key",
                table: "sborschik_zakaza",
                column: "sborschik_zakaza_telefon",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tovar_id_zakaz",
                table: "tovar",
                column: "id_zakaz");

            migrationBuilder.CreateIndex(
                name: "IX_zakaz_id_dostavka",
                table: "zakaz",
                column: "id_dostavka");

            migrationBuilder.CreateIndex(
                name: "IX_zakaz_id_pokupatel",
                table: "zakaz",
                column: "id_pokupatel");

            migrationBuilder.CreateIndex(
                name: "IX_zakaz_id_sborschik_zakaza",
                table: "zakaz",
                column: "id_sborschik_zakaza");

            migrationBuilder.CreateIndex(
                name: "IX_zakaz_TovarIdTovar",
                table: "zakaz",
                column: "TovarIdTovar");

            migrationBuilder.AddForeignKey(
                name: "zakaz_tovar",
                table: "tovar",
                column: "id_zakaz",
                principalTable: "zakaz",
                principalColumn: "id_zakaz");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "zakaz_tovar",
                table: "tovar");

            migrationBuilder.DropTable(
                name: "zakaz");

            migrationBuilder.DropTable(
                name: "tovar");

            migrationBuilder.DropTable(
                name: "dostavka");

            migrationBuilder.DropTable(
                name: "pokupatel");

            migrationBuilder.DropTable(
                name: "sborschik_zakaza");
        }
    }
}
