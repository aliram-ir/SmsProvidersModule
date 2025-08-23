using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitSmsConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmsConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EnableAutoFailoverProvider = table.Column<bool>(type: "bit", nullable: false),
                    EnableAutoFailoverLineNumber = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsProviderEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderType = table.Column<int>(type: "int", nullable: false),
                    ApiKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Token = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsProviderEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsLineEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderEntryId = table.Column<int>(type: "int", nullable: false),
                    LineNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsLineEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsLineEntries_SmsProviderEntries_ProviderEntryId",
                        column: x => x.ProviderEntryId,
                        principalTable: "SmsProviderEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmsTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderEntryId = table.Column<int>(type: "int", nullable: false),
                    TemplateType = table.Column<int>(type: "int", nullable: false),
                    TemplateBody = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsTemplates_SmsProviderEntries_ProviderEntryId",
                        column: x => x.ProviderEntryId,
                        principalTable: "SmsProviderEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SmsConfig",
                columns: new[] { "Id", "EnableAutoFailoverLineNumber", "EnableAutoFailoverProvider" },
                values: new object[] { 1, false, false });

            migrationBuilder.CreateIndex(
                name: "IX_SmsLineEntries_IsDefault",
                table: "SmsLineEntries",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_SmsLineEntries_ProviderEntryId",
                table: "SmsLineEntries",
                column: "ProviderEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsProviderEntries_IsActive",
                table: "SmsProviderEntries",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_SmsProviderEntries_IsDefault",
                table: "SmsProviderEntries",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_SmsTemplates_ProviderEntryId",
                table: "SmsTemplates",
                column: "ProviderEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmsConfig");

            migrationBuilder.DropTable(
                name: "SmsLineEntries");

            migrationBuilder.DropTable(
                name: "SmsTemplates");

            migrationBuilder.DropTable(
                name: "SmsProviderEntries");
        }
    }
}
