using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Template.Persistence.PostgreSql.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "log");

            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.EnsureSchema(
                name: "sample");

            migrationBuilder.CreateTable(
                name: "exceptions",
                schema: "log",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    insert_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    id_level_error = table.Column<int>(type: "integer", nullable: false),
                    level_error = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    site_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    process_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    inner_exception = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    stack_trace = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    user_agent = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    headers = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    properties = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    class_method = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    machine_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    machine_ip = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    logger = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    id_user = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    remote_address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    full_referer = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    full_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exceptions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "traces",
                schema: "log",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    insert_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    id_level_error = table.Column<int>(type: "integer", nullable: false),
                    level_error = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    site_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    process_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    inner_exception = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    stack_trace = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    user_agent = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    headers = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    properties = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    class_method = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    machine_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    machine_ip = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    logger = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    id_user = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    remote_address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    full_referer = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    full_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_traces", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roleclaims",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roleclaims", x => x.id);
                    table.ForeignKey(
                        name: "fk_roleclaims_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "auth",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "samplelists",
                schema: "sample",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_samplelists", x => x.id);
                    table.ForeignKey(
                        name: "fk_samplelists_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userclaims",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userclaims", x => x.id);
                    table.ForeignKey(
                        name: "fk_userclaims_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userlogins",
                schema: "auth",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    provider_key = table.Column<string>(type: "text", nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userlogins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_userlogins_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userroles",
                schema: "auth",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userroles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_userroles_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "auth",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_userroles_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usertokens",
                schema: "auth",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usertokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_usertokens_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sampleitems",
                schema: "sample",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    list_id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sampleitems", x => x.id);
                    table.ForeignKey(
                        name: "fk_sampleitems_sample_lists_sample_list_id",
                        column: x => x.list_id,
                        principalSchema: "sample",
                        principalTable: "samplelists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_roleclaims_role_id",
                schema: "auth",
                table: "roleclaims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "auth",
                table: "roles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sampleitems_list_id",
                schema: "sample",
                table: "sampleitems",
                column: "list_id");

            migrationBuilder.CreateIndex(
                name: "ix_samplelists_user_id_name",
                schema: "sample",
                table: "samplelists",
                columns: new[] { "user_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_userclaims_user_id",
                schema: "auth",
                table: "userclaims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_userlogins_user_id",
                schema: "auth",
                table: "userlogins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_userroles_role_id",
                schema: "auth",
                table: "userroles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "auth",
                table: "users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "auth",
                table: "users",
                column: "normalized_user_name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exceptions",
                schema: "log");

            migrationBuilder.DropTable(
                name: "roleclaims",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "sampleitems",
                schema: "sample");

            migrationBuilder.DropTable(
                name: "traces",
                schema: "log");

            migrationBuilder.DropTable(
                name: "userclaims",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "userlogins",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "userroles",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "usertokens",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "samplelists",
                schema: "sample");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "users",
                schema: "auth");
        }
    }
}
