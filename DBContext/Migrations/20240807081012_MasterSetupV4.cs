﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Migrations
{
    /// <inheritdoc />
    public partial class MasterSetupV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProvienceName",
                table: "Divisions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProvienceName",
                table: "Divisions");
        }
    }
}
