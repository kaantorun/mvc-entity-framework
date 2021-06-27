using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ACuteArtInterface.Migrations
{
    public partial class CreatingIdentityScheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                 name: "ACCCopy",
                 columns: table => new
                 {
                     acccopy_id = table.Column<long>(type: "bigint", nullable: false)
                         .Annotation("SqlServer:Identity", "1, 1"),
                     acccopy_key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     acccopy_value = table.Column<string>(type: "nvarchar(50)", nullable: false)
                 },
                 constraints: table =>
                 {
                     table.PrimaryKey("PK_ACCCopy", x => x.acccopy_id);
                 });

            migrationBuilder.CreateTable(
                 name: "ACCArtist",
                 columns: table => new
                 {
                     accartist_id = table.Column<long>(type: "bigint", nullable: false)
                         .Annotation("SqlServer:Identity", "1, 1"),
                     accartist_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     accartist_creation = table.Column<DateTime>(type: "datetime", nullable: false),
                     accartist_guid = table.Column<string>(type: "nvarchar(50)", nullable: false),
                     accartist_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     accartist_img_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     accartist_fullname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     accartist_order = table.Column<int>(type: "int", nullable: false),
                     accartist_thumb_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     accartist_icon_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     accartist_active = table.Column<bool>(type: "bit", nullable: false)
                 },
                 constraints: table =>
                 {
                     table.PrimaryKey("PK_ACCArtist", x => x.accartist_id);
                 });

            migrationBuilder.CreateTable(
                name: "ACCArtwork",
                columns: table => new
                {
                    accartwork_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accartwork_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_creation = table.Column<DateTime>(type: "datetime", nullable: false),
                    accartwork_artist = table.Column<long>(type: "bigint", nullable: true),
                    accartwork_type = table.Column<int>(type: "int", nullable: false),
                    accartwork_source_meta = table.Column<string>(type: "text", nullable: true),
                    accartwork_guid = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    accartwork_object_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_icon_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_wp_id = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    accartwork_prefab_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_marshal_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_thumb_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_sign_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_size_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_year = table.Column<int>(type: "int", nullable: true),
                    accartwork_edition_size = table.Column<int>(type: "int", nullable: true),
                    accartwork_color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_trade_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_ap_size = table.Column<int>(type: "int", nullable: false),
                    accartwork_preload = table.Column<bool>(type: "bit", nullable: false),
                    accartwork_web_picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_web_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_web_intro_picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_web_carrosel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_hardware_specs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_map_icon_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accartwork_active = table.Column<bool>(type: "bit", nullable: false)

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCArtwork", x => x.accartwork_id);
                });

            migrationBuilder.CreateTable(
                name: "ACCExhibition",
                columns: table => new
                {
                    accexhibition_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accexhibition_owner = table.Column<long>(type: "bigint", nullable: true),
                    accexhibition_title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_map_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_sponsor_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_sponsor_images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_geofenced = table.Column<bool>(type: "bit", nullable: false),
                    accexhibition_latitude = table.Column<double>(type: "float", nullable: false),
                    accexhibition_longitude = table.Column<double>(type: "float", nullable: false),
                    accexhibition_radius = table.Column<double>(type: "float", nullable: false),
                    accexhibition_metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_created = table.Column<DateTime>(type: "datetime", nullable: false),
                    accexhibition_startdate = table.Column<DateTime>(type: "datetime", nullable: false),
                    accexhibition_enddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    accexhibition_active = table.Column<bool>(type: "bit", nullable: false),
                    accexhibition_guid = table.Column<string>(type: "nvarchar(80)", nullable: true),
                    accexhibition_order = table.Column<int>(type: "int", nullable: false),
                    accexhibition_icon_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_thumb_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_main_map_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_intro_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_use_gps = table.Column<bool>(type: "bit", nullable: false),
                    accexhibition_scan_radius = table.Column<double>(type: "float", nullable: false),
                    accexhibition_show_radius = table.Column<double>(type: "float", nullable: false),
                    accexhibition_view_radius = table.Column<double>(type: "float", nullable: false),
                    accexhibition_howto = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCExhibition", x => x.accexhibition_id);
                });

            migrationBuilder.CreateTable(
                name: "ACCUser",
                columns: table => new
                {
                    accuser_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accuser_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accuser_guid = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    accuser_creationtime = table.Column<DateTime>(type: "datetime", nullable: false),
                    accuser_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    accuser_device_id = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    accuser_unique_identifier = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    accuser_push_id = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    accuser_wpid = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    accuser_lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accuser_image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accuser_role = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    accuser_profile_crop = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    accuser_password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accuser_blocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCUser", x => x.accuser_id);
                });

            migrationBuilder.CreateTable(
                name: "ACCExhibitionArtwork",
                columns: table => new
                {
                    accexhibition_art_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accexhibition_art_artwork_id = table.Column<long>(type: "bigint", nullable: false),
                    accexhibition_art_order = table.Column<int>(type: "int", nullable: false),
                    accexhibition_art_active = table.Column<bool>(type: "bit", nullable: false),
                    accexhibition_art_exhibition_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCExhibitionArtwork", x => x.accexhibition_art_id);
                    table.ForeignKey(
                        name: "FK_ACCExhibitionArtwork_ACCExhibition_accexhibition_art_exhibition_id",
                        column: x => x.accexhibition_art_exhibition_id,
                        principalTable: "ACCExhibition",
                        principalColumn: "accexhibition_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ACCExhibitionRoom",
                columns: table => new
                {
                    accexhibition_room_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accexhibition_room_title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_room_va_guid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_room_icon_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_room_thumb_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_room_map_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_room_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_room_exhibition = table.Column<long>(type: "bigint", nullable: false),
                    accexhibition_room_va = table.Column<long>(type: "bigint", nullable: true),
                    accexhibition_room_order = table.Column<int>(type: "int", nullable: false),
                    accexhibition_room_scan_radius = table.Column<double>(type: "float", nullable: false),
                    accexhibition_room_show_on_map = table.Column<bool>(type: "bit", nullable: false),
                    accexhibition_room_map_icon_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accexhibition_room_active = table.Column<bool>(type: "bit", nullable: false)

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCExhibitionRoom", x => x.accexhibition_room_id);
                    table.ForeignKey(
                        name: "FK_ACCExhibitionRoom_ACCExhibition_accexhibition_room_exhibition",
                        column: x => x.accexhibition_room_exhibition,
                        principalTable: "ACCExhibition",
                        principalColumn: "accexhibition_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ACCExhibitionRoomItem",
                columns: table => new
                {
                    accexhibition_room_item_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accexhibition_room_item_artwork = table.Column<long>(type: "bigint", nullable: false),
                    accexhibition_room_item_edition_number = table.Column<int>(type: "int", nullable: false),
                    accexhibition_room_item_scale = table.Column<double>(type: "float", nullable: false),
                    accexhibition_room_item_offset = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    accexhibition_room_item_axis = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    accexhibition_room_item_rotaton = table.Column<double>(type: "float", nullable: false),
                    accexhibition_room_item_room = table.Column<long>(type: "bigint", nullable: false),
                    accexhibition_room_item_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCExhibitionRoomItem", x => x.accexhibition_room_item_id);
                    table.ForeignKey(
                        name: "FK_ACCExhibitionRoomItem_ACCArtwork_accexhibition_room_item_artwork",
                        column: x => x.accexhibition_room_item_artwork,
                        principalTable: "ACCArtwork",
                        principalColumn: "accartwork_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACCExhibitionRoomItem_ACCExhibitionRoom_accexhibition_room_item_room",
                        column: x => x.accexhibition_room_item_room,
                        principalTable: "ACCExhibitionRoom",
                        principalColumn: "accexhibition_room_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
               name: "AspNetUsers",
               columns: table => new
               {
                   Id = table.Column<string>(nullable: false),
                   UserName = table.Column<string>(maxLength: 256, nullable: true),
                   NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                   Email = table.Column<string>(maxLength: 256, nullable: true),
                   NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                   EmailConfirmed = table.Column<bool>(nullable: false),
                   PasswordHash = table.Column<string>(nullable: true),
                   SecurityStamp = table.Column<string>(nullable: true),
                   ConcurrencyStamp = table.Column<string>(nullable: true),
                   PhoneNumber = table.Column<string>(nullable: true),
                   PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                   TwoFactorEnabled = table.Column<bool>(nullable: false),
                   LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                   LockoutEnabled = table.Column<bool>(nullable: false),
                   AccessFailedCount = table.Column<int>(nullable: false),
                   FirstName = table.Column<string>(nullable: true),
                   LastName = table.Column<string>(nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_AspNetUsers", x => x.Id);
               });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ACCExhibitionArtwork_accexhibition_art_exhibition_id",
                table: "ACCExhibitionArtwork",
                column: "accexhibition_art_exhibition_id");

            migrationBuilder.CreateIndex(
                name: "IX_ACCExhibitionRoom_accexhibition_room_exhibition",
                table: "ACCExhibitionRoom",
                column: "accexhibition_room_exhibition");

            migrationBuilder.CreateIndex(
                name: "IX_ACCExhibitionRoomItem_accexhibition_room_item_artwork",
                table: "ACCExhibitionRoomItem",
                column: "accexhibition_room_item_artwork");

            migrationBuilder.CreateIndex(
                name: "IX_ACCExhibitionRoomItem_accexhibition_room_item_room",
                table: "ACCExhibitionRoomItem",
                column: "accexhibition_room_item_room");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
               name: "ACCArtist");

            migrationBuilder.DropTable(
                name: "ACCExhibitionArtwork");

            migrationBuilder.DropTable(
                name: "ACCExhibitionRoomItem");

            migrationBuilder.DropTable(
                name: "ACCUser");

            migrationBuilder.DropTable(
                name: "ACCArtwork");

            migrationBuilder.DropTable(
                name: "ACCExhibitionRoom");

            migrationBuilder.DropTable(
                name: "ACCExhibition");
        }
    }
}
