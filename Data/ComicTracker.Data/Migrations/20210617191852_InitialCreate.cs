namespace ComicTracker.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FirstAppearance = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nationalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CoverPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ongoing = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfDeath = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalityId = table.Column<int>(type: "int", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artists_Nationalities_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "Nationalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FoundingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NationalityId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publishers_Nationalities_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "Nationalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Writers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfDeath = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalityId = table.Column<int>(type: "int", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Writers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Writers_Nationalities_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "Nationalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Arcs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CoverPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arcs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arcs_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharactersSeries",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    IsMainCharacter = table.Column<bool>(type: "bit", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharactersSeries", x => new { x.CharacterId, x.SeriesId });
                    table.ForeignKey(
                        name: "FK_CharactersSeries_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CharactersSeries_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GenreSeries",
                columns: table => new
                {
                    GenresId = table.Column<int>(type: "int", nullable: false),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreSeries", x => new { x.GenresId, x.SeriesId });
                    table.ForeignKey(
                        name: "FK_GenreSeries_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GenreSeries_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Volumes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CoverPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volumes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Volumes_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArtistSeries",
                columns: table => new
                {
                    ArtistsId = table.Column<int>(type: "int", nullable: false),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistSeries", x => new { x.ArtistsId, x.SeriesId });
                    table.ForeignKey(
                        name: "FK_ArtistSeries_Artists_ArtistsId",
                        column: x => x.ArtistsId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArtistSeries_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublisherSeries",
                columns: table => new
                {
                    PublishersId = table.Column<int>(type: "int", nullable: false),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublisherSeries", x => new { x.PublishersId, x.SeriesId });
                    table.ForeignKey(
                        name: "FK_PublisherSeries_Publishers_PublishersId",
                        column: x => x.PublishersId,
                        principalTable: "Publishers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PublisherSeries_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SeriesWriter",
                columns: table => new
                {
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    WritersId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesWriter", x => new { x.SeriesId, x.WritersId });
                    table.ForeignKey(
                        name: "FK_SeriesWriter_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SeriesWriter_Writers_WritersId",
                        column: x => x.WritersId,
                        principalTable: "Writers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArcArtist",
                columns: table => new
                {
                    ArcsId = table.Column<int>(type: "int", nullable: false),
                    ArtistsId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArcArtist", x => new { x.ArcsId, x.ArtistsId });
                    table.ForeignKey(
                        name: "FK_ArcArtist_Arcs_ArcsId",
                        column: x => x.ArcsId,
                        principalTable: "Arcs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArcArtist_Artists_ArtistsId",
                        column: x => x.ArtistsId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArcGenre",
                columns: table => new
                {
                    ArcsId = table.Column<int>(type: "int", nullable: false),
                    GenresId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArcGenre", x => new { x.ArcsId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_ArcGenre_Arcs_ArcsId",
                        column: x => x.ArcsId,
                        principalTable: "Arcs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArcGenre_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArcPublisher",
                columns: table => new
                {
                    ArcsId = table.Column<int>(type: "int", nullable: false),
                    PublishersId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArcPublisher", x => new { x.ArcsId, x.PublishersId });
                    table.ForeignKey(
                        name: "FK_ArcPublisher_Arcs_ArcsId",
                        column: x => x.ArcsId,
                        principalTable: "Arcs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArcPublisher_Publishers_PublishersId",
                        column: x => x.PublishersId,
                        principalTable: "Publishers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArcWriter",
                columns: table => new
                {
                    ArcsId = table.Column<int>(type: "int", nullable: false),
                    WritersId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArcWriter", x => new { x.ArcsId, x.WritersId });
                    table.ForeignKey(
                        name: "FK_ArcWriter_Arcs_ArcsId",
                        column: x => x.ArcsId,
                        principalTable: "Arcs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArcWriter_Writers_WritersId",
                        column: x => x.WritersId,
                        principalTable: "Writers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharactersArcs",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    ArcId = table.Column<int>(type: "int", nullable: false),
                    IsMainCharacter = table.Column<bool>(type: "bit", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharactersArcs", x => new { x.CharacterId, x.ArcId });
                    table.ForeignKey(
                        name: "FK_CharactersArcs_Arcs_ArcId",
                        column: x => x.ArcId,
                        principalTable: "Arcs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CharactersArcs_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArcVolume",
                columns: table => new
                {
                    ArcId = table.Column<int>(type: "int", nullable: false),
                    VolumeId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArcVolume", x => new { x.ArcId, x.VolumeId });
                    table.ForeignKey(
                        name: "FK_ArcVolume_Arcs_ArcId",
                        column: x => x.ArcId,
                        principalTable: "Arcs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArcVolume_Volumes_VolumeId",
                        column: x => x.VolumeId,
                        principalTable: "Volumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArtistVolume",
                columns: table => new
                {
                    ArtistsId = table.Column<int>(type: "int", nullable: false),
                    VolumesId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistVolume", x => new { x.ArtistsId, x.VolumesId });
                    table.ForeignKey(
                        name: "FK_ArtistVolume_Artists_ArtistsId",
                        column: x => x.ArtistsId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArtistVolume_Volumes_VolumesId",
                        column: x => x.VolumesId,
                        principalTable: "Volumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharactersVolumes",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    VolumeId = table.Column<int>(type: "int", nullable: false),
                    IsMainCharacter = table.Column<bool>(type: "bit", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharactersVolumes", x => new { x.CharacterId, x.VolumeId });
                    table.ForeignKey(
                        name: "FK_CharactersVolumes_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CharactersVolumes_Volumes_VolumeId",
                        column: x => x.VolumeId,
                        principalTable: "Volumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GenreVolume",
                columns: table => new
                {
                    GenresId = table.Column<int>(type: "int", nullable: false),
                    VolumesId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreVolume", x => new { x.GenresId, x.VolumesId });
                    table.ForeignKey(
                        name: "FK_GenreVolume_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GenreVolume_Volumes_VolumesId",
                        column: x => x.VolumesId,
                        principalTable: "Volumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CoverPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    ArcId = table.Column<int>(type: "int", nullable: true),
                    VolumeId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Issues_Arcs_ArcId",
                        column: x => x.ArcId,
                        principalTable: "Arcs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Issues_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Issues_Volumes_VolumeId",
                        column: x => x.VolumeId,
                        principalTable: "Volumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublishersVolumes",
                columns: table => new
                {
                    PublisherId = table.Column<int>(type: "int", nullable: false),
                    VolumeId = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishersVolumes", x => new { x.PublisherId, x.VolumeId });
                    table.ForeignKey(
                        name: "FK_PublishersVolumes_Publishers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Publishers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PublishersVolumes_Volumes_VolumeId",
                        column: x => x.VolumeId,
                        principalTable: "Volumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VolumeWriter",
                columns: table => new
                {
                    VolumesId = table.Column<int>(type: "int", nullable: false),
                    WritersId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolumeWriter", x => new { x.VolumesId, x.WritersId });
                    table.ForeignKey(
                        name: "FK_VolumeWriter_Volumes_VolumesId",
                        column: x => x.VolumesId,
                        principalTable: "Volumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VolumeWriter_Writers_WritersId",
                        column: x => x.WritersId,
                        principalTable: "Writers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArtistIssue",
                columns: table => new
                {
                    ArtistsId = table.Column<int>(type: "int", nullable: false),
                    IssuesId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistIssue", x => new { x.ArtistsId, x.IssuesId });
                    table.ForeignKey(
                        name: "FK_ArtistIssue_Artists_ArtistsId",
                        column: x => x.ArtistsId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArtistIssue_Issues_IssuesId",
                        column: x => x.IssuesId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharactersIssues",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    IssueId = table.Column<int>(type: "int", nullable: false),
                    IsMainCharacter = table.Column<bool>(type: "bit", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharactersIssues", x => new { x.CharacterId, x.IssueId });
                    table.ForeignKey(
                        name: "FK_CharactersIssues_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CharactersIssues_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
               name: "GenreIssue",
               columns: table => new
               {
                   GenresId = table.Column<int>(type: "int", nullable: false),
                   IssuesId = table.Column<int>(type: "int", nullable: false),
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_GenreIssue", x => new { x.GenresId, x.IssuesId });
                   table.ForeignKey(
                       name: "FK_GenreIssue_Genres_GenresId",
                       column: x => x.GenresId,
                       principalTable: "Genres",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Restrict);
                   table.ForeignKey(
                       name: "FK_GenreIssue_Issues_IssuesId",
                       column: x => x.IssuesId,
                       principalTable: "Issues",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Restrict);
               });

            migrationBuilder.CreateTable(
                name: "IssueWriter",
                columns: table => new
                {
                    IssuesId = table.Column<int>(type: "int", nullable: false),
                    WritersId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueWriter", x => new { x.IssuesId, x.WritersId });
                    table.ForeignKey(
                        name: "FK_IssueWriter_Issues_IssuesId",
                        column: x => x.IssuesId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssueWriter_Writers_WritersId",
                        column: x => x.WritersId,
                        principalTable: "Writers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublishersIssues",
                columns: table => new
                {
                    PublisherId = table.Column<int>(type: "int", nullable: false),
                    IssueId = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishersIssues", x => new { x.PublisherId, x.IssueId });
                    table.ForeignKey(
                        name: "FK_PublishersIssues_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PublishersIssues_Publishers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Publishers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArcArtist_ArtistsId",
                table: "ArcArtist",
                column: "ArtistsId");

            migrationBuilder.CreateIndex(
                name: "IX_ArcGenre_GenresId",
                table: "ArcGenre",
                column: "GenresId");

            migrationBuilder.CreateIndex(
                name: "IX_ArcPublisher_PublishersId",
                table: "ArcPublisher",
                column: "PublishersId");

            migrationBuilder.CreateIndex(
                name: "IX_Arcs_SeriesId",
                table: "Arcs",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_ArcVolume_VolumeId",
                table: "ArcVolume",
                column: "VolumeId");

            migrationBuilder.CreateIndex(
                name: "IX_ArcWriter_WritersId",
                table: "ArcWriter",
                column: "WritersId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistIssue_IssuesId",
                table: "ArtistIssue",
                column: "IssuesId");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_NationalityId",
                table: "Artists",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistSeries_SeriesId",
                table: "ArtistSeries",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistVolume_VolumesId",
                table: "ArtistVolume",
                column: "VolumesId");

            migrationBuilder.CreateIndex(
                name: "IX_CharactersArcs_ArcId",
                table: "CharactersArcs",
                column: "ArcId");

            migrationBuilder.CreateIndex(
                name: "IX_CharactersIssues_IssueId",
                table: "CharactersIssues",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_CharactersSeries_SeriesId",
                table: "CharactersSeries",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_CharactersVolumes_VolumeId",
                table: "CharactersVolumes",
                column: "VolumeId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreIssue_IssuesId",
                table: "GenreIssue",
                column: "IssuesId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreSeries_SeriesId",
                table: "GenreSeries",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreVolume_VolumesId",
                table: "GenreVolume",
                column: "VolumesId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_ArcId",
                table: "Issues",
                column: "ArcId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_SeriesId",
                table: "Issues",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_VolumeId",
                table: "Issues",
                column: "VolumeId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueWriter_WritersId",
                table: "IssueWriter",
                column: "WritersId");

            migrationBuilder.CreateIndex(
                name: "IX_Publishers_NationalityId",
                table: "Publishers",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_PublisherSeries_SeriesId",
                table: "PublisherSeries",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishersIssues_IssueId",
                table: "PublishersIssues",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishersVolumes_VolumeId",
                table: "PublishersVolumes",
                column: "VolumeId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesWriter_WritersId",
                table: "SeriesWriter",
                column: "WritersId");

            migrationBuilder.CreateIndex(
                name: "IX_Volumes_SeriesId",
                table: "Volumes",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_VolumeWriter_WritersId",
                table: "VolumeWriter",
                column: "WritersId");

            migrationBuilder.CreateIndex(
                name: "IX_Writers_NationalityId",
                table: "Writers",
                column: "NationalityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArcArtist");

            migrationBuilder.DropTable(
                name: "ArcGenre");

            migrationBuilder.DropTable(
                name: "ArcPublisher");

            migrationBuilder.DropTable(
                name: "ArcVolume");

            migrationBuilder.DropTable(
                name: "ArcWriter");

            migrationBuilder.DropTable(
                name: "ArtistIssue");

            migrationBuilder.DropTable(
                name: "ArtistSeries");

            migrationBuilder.DropTable(
                name: "ArtistVolume");

            migrationBuilder.DropTable(
                name: "CharactersArcs");

            migrationBuilder.DropTable(
                name: "CharactersIssues");

            migrationBuilder.DropTable(
                name: "CharactersSeries");

            migrationBuilder.DropTable(
                name: "CharactersVolumes");

            migrationBuilder.DropTable(
                name: "GenreIssue");

            migrationBuilder.DropTable(
                name: "GenreSeries");

            migrationBuilder.DropTable(
                name: "GenreVolume");

            migrationBuilder.DropTable(
                name: "IssueWriter");

            migrationBuilder.DropTable(
                name: "PublisherSeries");

            migrationBuilder.DropTable(
                name: "PublishersIssues");

            migrationBuilder.DropTable(
                name: "PublishersVolumes");

            migrationBuilder.DropTable(
                name: "SeriesWriter");

            migrationBuilder.DropTable(
                name: "VolumeWriter");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropTable(
                name: "Publishers");

            migrationBuilder.DropTable(
                name: "Writers");

            migrationBuilder.DropTable(
                name: "Arcs");

            migrationBuilder.DropTable(
                name: "Volumes");

            migrationBuilder.DropTable(
                name: "Nationalities");

            migrationBuilder.DropTable(
                name: "Series");
        }
    }
}
