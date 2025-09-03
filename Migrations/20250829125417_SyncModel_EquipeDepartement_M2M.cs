using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackMania.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel_EquipeDepartement_M2M : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departements_Users_AdminId",
                table: "Departements");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipeIngenieur_Equipes_EquipeId",
                table: "EquipeIngenieur");

            migrationBuilder.DropIndex(
                name: "IX_Departements_AdminId",
                table: "Departements");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Departements");

            migrationBuilder.RenameColumn(
                name: "EquipeId",
                table: "EquipeIngenieur",
                newName: "EquipesId");

            migrationBuilder.AddColumn<int>(
                name: "DepartementId1",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "KPIs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateMesure",
                table: "KPIs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "DepartementId",
                table: "Equipes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartementId1",
                table: "Users",
                column: "DepartementId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipeIngenieur_Equipes_EquipesId",
                table: "EquipeIngenieur",
                column: "EquipesId",
                principalTable: "Equipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Departements_DepartementId1",
                table: "Users",
                column: "DepartementId1",
                principalTable: "Departements",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipeIngenieur_Equipes_EquipesId",
                table: "EquipeIngenieur");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departements_DepartementId1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DepartementId1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DepartementId1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DateMesure",
                table: "KPIs");

            migrationBuilder.RenameColumn(
                name: "EquipesId",
                table: "EquipeIngenieur",
                newName: "EquipeId");

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "KPIs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DepartementId",
                table: "Equipes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Departements",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departements_AdminId",
                table: "Departements",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departements_Users_AdminId",
                table: "Departements",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipeIngenieur_Equipes_EquipeId",
                table: "EquipeIngenieur",
                column: "EquipeId",
                principalTable: "Equipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
