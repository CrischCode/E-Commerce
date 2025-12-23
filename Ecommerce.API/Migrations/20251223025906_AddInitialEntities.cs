using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.API.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "persona",
                columns: table => new
                {
                    id_persona = table.Column<Guid>(type: "uuid", nullable: false),
                    primer_nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    segundo_nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    primer_apellido = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    segundo_apellido = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    fecha_nacimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    fecha_registro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persona", x => x.id_persona);
                });

            migrationBuilder.CreateTable(
                name: "rol",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_persona = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_alta = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    puntos = table.Column<int>(type: "integer", nullable: false),
                    PersonaIdPersona = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente", x => x.id_cliente);
                    table.ForeignKey(
                        name: "FK_cliente_persona_PersonaIdPersona",
                        column: x => x.PersonaIdPersona,
                        principalTable: "persona",
                        principalColumn: "id_persona",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "empleado",
                columns: table => new
                {
                    id_empleado = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_persona = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_ingreso = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    PersonaIdPersona = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empleado", x => x.id_empleado);
                    table.ForeignKey(
                        name: "FK_empleado_persona_PersonaIdPersona",
                        column: x => x.PersonaIdPersona,
                        principalTable: "persona",
                        principalColumn: "id_persona",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "persona_rol",
                columns: table => new
                {
                    id_persona = table.Column<Guid>(type: "uuid", nullable: false),
                    id_rol = table.Column<int>(type: "integer", nullable: false),
                    PersonaIdPersona = table.Column<Guid>(type: "uuid", nullable: false),
                    RolIdRol = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persona_rol", x => new { x.id_persona, x.id_rol });
                    table.ForeignKey(
                        name: "FK_persona_rol_persona_PersonaIdPersona",
                        column: x => x.PersonaIdPersona,
                        principalTable: "persona",
                        principalColumn: "id_persona",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_persona_rol_rol_RolIdRol",
                        column: x => x.RolIdRol,
                        principalTable: "rol",
                        principalColumn: "id_rol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "salario",
                columns: table => new
                {
                    id_salario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_empleado = table.Column<int>(type: "integer", nullable: false),
                    monto = table.Column<decimal>(type: "numeric", nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EmpleadoIdEmpleado = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salario", x => x.id_salario);
                    table.ForeignKey(
                        name: "FK_salario_empleado_EmpleadoIdEmpleado",
                        column: x => x.EmpleadoIdEmpleado,
                        principalTable: "empleado",
                        principalColumn: "id_empleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cliente_PersonaIdPersona",
                table: "cliente",
                column: "PersonaIdPersona");

            migrationBuilder.CreateIndex(
                name: "IX_empleado_PersonaIdPersona",
                table: "empleado",
                column: "PersonaIdPersona");

            migrationBuilder.CreateIndex(
                name: "IX_persona_rol_PersonaIdPersona",
                table: "persona_rol",
                column: "PersonaIdPersona");

            migrationBuilder.CreateIndex(
                name: "IX_persona_rol_RolIdRol",
                table: "persona_rol",
                column: "RolIdRol");

            migrationBuilder.CreateIndex(
                name: "IX_salario_EmpleadoIdEmpleado",
                table: "salario",
                column: "EmpleadoIdEmpleado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cliente");

            migrationBuilder.DropTable(
                name: "persona_rol");

            migrationBuilder.DropTable(
                name: "salario");

            migrationBuilder.DropTable(
                name: "rol");

            migrationBuilder.DropTable(
                name: "empleado");

            migrationBuilder.DropTable(
                name: "persona");
        }
    }
}
