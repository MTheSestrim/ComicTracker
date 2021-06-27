namespace ComicTracker.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class InheritedBaseDeletableModelInEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Writers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Writers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Writers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Writers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Volumes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Volumes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Volumes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Volumes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Series",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Series",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Series",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Series",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Publishers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Publishers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Publishers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Publishers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Nationalities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Nationalities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Nationalities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Nationalities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Issues",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Issues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Issues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Issues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Genres",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Genres",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Genres",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Genres",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Characters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Characters",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Characters",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Artists",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Artists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Artists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Artists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Arcs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Arcs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Arcs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Arcs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Writers_IsDeleted",
                table: "Writers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Volumes_IsDeleted",
                table: "Volumes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Series_IsDeleted",
                table: "Series",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Publishers_IsDeleted",
                table: "Publishers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_IsDeleted",
                table: "Nationalities",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_IsDeleted",
                table: "Issues",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_IsDeleted",
                table: "Genres",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_IsDeleted",
                table: "Characters",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_IsDeleted",
                table: "Artists",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Arcs_IsDeleted",
                table: "Arcs",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Writers_IsDeleted",
                table: "Writers");

            migrationBuilder.DropIndex(
                name: "IX_Volumes_IsDeleted",
                table: "Volumes");

            migrationBuilder.DropIndex(
                name: "IX_Series_IsDeleted",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Publishers_IsDeleted",
                table: "Publishers");

            migrationBuilder.DropIndex(
                name: "IX_Nationalities_IsDeleted",
                table: "Nationalities");

            migrationBuilder.DropIndex(
                name: "IX_Issues_IsDeleted",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Genres_IsDeleted",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Characters_IsDeleted",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Artists_IsDeleted",
                table: "Artists");

            migrationBuilder.DropIndex(
                name: "IX_Arcs_IsDeleted",
                table: "Arcs");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Writers");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Writers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Writers");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Writers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Volumes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Volumes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Volumes");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Volumes");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Nationalities");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Nationalities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Nationalities");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Nationalities");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Arcs");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Arcs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Arcs");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Arcs");
        }
    }
}
