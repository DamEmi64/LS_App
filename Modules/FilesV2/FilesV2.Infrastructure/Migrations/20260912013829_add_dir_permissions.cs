using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilesV2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_dir_permissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilesV2_FileUser_OwnerId",
                table: "FilesV2");

            migrationBuilder.RenameTable(
                name: "FileUser",
                newName: "CatalogUser");

            migrationBuilder.AddColumn<Guid>(
                name: "DirectoryId",
                table: "CatalogUser",
                type: "uniqueidentifier",
                nullable: true);

            var ownerId = Guid.NewGuid();

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Directories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql($"""
            INSERT INTO [AppContext].[dbo].[CatalogUser]
                ([ID], [UserId], [Login], [Privilage], [InsBy], [UpdBy], [InsDate], [UpdDate])
            VALUES
                ('{ownerId}',
                 'f9beb204-3bb1-4531-84fc-96762bc26520',
                 'DamEmi64',
                 0,
                 'SYSTEM',
                 'SYSTEM',
                 GETDATE(),
                 GETDATE());

            UPDATE [Directories]
            SET [OwnerId] = '{ownerId}';
            """);

            migrationBuilder.AlterColumn<Guid>(
            name: "OwnerId",
            table: "Directories",
            type: "uniqueidentifier",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Public",
                table: "Directories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Directories_OwnerId",
                table: "Directories",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogUser_DirectoryId",
                table: "CatalogUser",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogUser_FileId",
                table: "CatalogUser",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Directories_CatalogUser_OwnerId",
                table: "Directories",
                column: "OwnerId",
                principalTable: "CatalogUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_FilesV2_CatalogUser_OwnerId",
                table: "FilesV2",
                column: "OwnerId",
                principalTable: "CatalogUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Directories_CatalogUser_OwnerId",
                table: "Directories");

            migrationBuilder.DropForeignKey(
                name: "FK_FilesV2_CatalogUser_OwnerId",
                table: "FilesV2");

            migrationBuilder.DropTable(
                name: "CatalogUser");

            migrationBuilder.DropIndex(
                name: "IX_Directories_OwnerId",
                table: "Directories");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Directories");

            migrationBuilder.DropColumn(
                name: "Public",
                table: "Directories");

            migrationBuilder.CreateTable(
                name: "FileUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InsBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Privilage = table.Column<int>(type: "int", nullable: false),
                    UpdBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileUser_FilesV2_FileId",
                        column: x => x.FileId,
                        principalTable: "FilesV2",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileUser_FileId",
                table: "FileUser",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilesV2_FileUser_OwnerId",
                table: "FilesV2",
                column: "OwnerId",
                principalTable: "FileUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
