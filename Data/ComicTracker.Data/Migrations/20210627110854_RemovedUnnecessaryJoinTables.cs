namespace ComicTracker.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RemovedUnnecessaryJoinTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArcVolume_Arcs_ArcsId",
                table: "ArcVolume");

            migrationBuilder.DropForeignKey(
                name: "FK_ArcVolume_Volumes_VolumesId",
                table: "ArcVolume");

            migrationBuilder.DropTable(
                name: "ArcCharacter");

            migrationBuilder.DropTable(
                name: "ArcVolumes");

            migrationBuilder.DropTable(
                name: "CharacterIssue");

            migrationBuilder.DropTable(
                name: "CharacterSeries");

            migrationBuilder.DropTable(
                name: "CharacterVolume");

            migrationBuilder.DropTable(
                name: "IssuePublisher");

            migrationBuilder.DropTable(
                name: "PublisherVolume");

            migrationBuilder.RenameColumn(
                name: "VolumesId",
                table: "ArcVolume",
                newName: "VolumeId");

            migrationBuilder.RenameColumn(
                name: "ArcsId",
                table: "ArcVolume",
                newName: "ArcId");

            migrationBuilder.RenameIndex(
                name: "IX_ArcVolume_VolumesId",
                table: "ArcVolume",
                newName: "IX_ArcVolume_VolumeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArcVolume_Arcs_ArcId",
                table: "ArcVolume",
                column: "ArcId",
                principalTable: "Arcs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ArcVolume_Volumes_VolumeId",
                table: "ArcVolume",
                column: "VolumeId",
                principalTable: "Volumes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
