using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milo.Infraestructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixServiceProviderRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Providers_ProviderId1",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_ProviderId1",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ProviderId1",
                table: "Services");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId1",
                table: "Services",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_ProviderId1",
                table: "Services",
                column: "ProviderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Providers_ProviderId1",
                table: "Services",
                column: "ProviderId1",
                principalTable: "Providers",
                principalColumn: "Id");
        }
    }
}
