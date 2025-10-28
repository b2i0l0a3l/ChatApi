using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConverstationIDColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Conversations_conversationId",
                table: "Participants");

            migrationBuilder.AlterColumn<Guid>(
                name: "conversationId",
                table: "Participants",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Conversations_conversationId",
                table: "Participants",
                column: "conversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Conversations_conversationId",
                table: "Participants");

            migrationBuilder.AlterColumn<Guid>(
                name: "conversationId",
                table: "Participants",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Conversations_conversationId",
                table: "Participants",
                column: "conversationId",
                principalTable: "Conversations",
                principalColumn: "Id");
        }
    }
}
