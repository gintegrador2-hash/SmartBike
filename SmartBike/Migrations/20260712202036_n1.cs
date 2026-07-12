using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SmartBike.Migrations
{
    /// <inheritdoc />
    public partial class n1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "campus",
                columns: table => new
                {
                    campus_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    direccion = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campus", x => x.campus_id);
                });

            migrationBuilder.CreateTable(
                name: "estados_estacionamiento",
                columns: table => new
                {
                    estado_estacionamiento_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estados_estacionamiento", x => x.estado_estacionamiento_id);
                });

            migrationBuilder.CreateTable(
                name: "facultades",
                columns: table => new
                {
                    facultad_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_facultades", x => x.facultad_id);
                });

            migrationBuilder.CreateTable(
                name: "permisos",
                columns: table => new
                {
                    permiso_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo_permiso = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permisos", x => x.permiso_id);
                });

            migrationBuilder.CreateTable(
                name: "preguntas_frecuentes",
                columns: table => new
                {
                    pregunta_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    texto_pregunta = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    respuesta_default = table.Column<string>(type: "text", nullable: true),
                    orden = table.Column<int>(type: "integer", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_preguntas_frecuentes", x => x.pregunta_id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    rol_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_rol = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.rol_id);
                });

            migrationBuilder.CreateTable(
                name: "tipos_transporte",
                columns: table => new
                {
                    tipo_transporte_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    factor_co2_g_km = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    factor_calorias_km = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    factor_ahorro_dolar_km = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipos_transporte", x => x.tipo_transporte_id);
                });

            migrationBuilder.CreateTable(
                name: "estacionamientos",
                columns: table => new
                {
                    estacionamiento_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    campus_id = table.Column<int>(type: "integer", nullable: false),
                    estado_estacionamiento_id = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    capacidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estacionamientos", x => x.estacionamiento_id);
                    table.ForeignKey(
                        name: "FK_estacionamientos_campus_campus_id",
                        column: x => x.campus_id,
                        principalTable: "campus",
                        principalColumn: "campus_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_estacionamientos_estados_estacionamiento_estado_estacionami~",
                        column: x => x.estado_estacionamiento_id,
                        principalTable: "estados_estacionamiento",
                        principalColumn: "estado_estacionamiento_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "carreras",
                columns: table => new
                {
                    carrera_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    facultad_id = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carreras", x => x.carrera_id);
                    table.ForeignKey(
                        name: "FK_carreras_facultades_facultad_id",
                        column: x => x.facultad_id,
                        principalTable: "facultades",
                        principalColumn: "facultad_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermisoRol",
                columns: table => new
                {
                    PermisosPermisoId = table.Column<int>(type: "integer", nullable: false),
                    RolesRolId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermisoRol", x => new { x.PermisosPermisoId, x.RolesRolId });
                    table.ForeignKey(
                        name: "FK_PermisoRol_permisos_PermisosPermisoId",
                        column: x => x.PermisosPermisoId,
                        principalTable: "permisos",
                        principalColumn: "permiso_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermisoRol_roles_RolesRolId",
                        column: x => x.RolesRolId,
                        principalTable: "roles",
                        principalColumn: "rol_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "registros_viajes",
                columns: table => new
                {
                    registro_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usuario_cedula = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    tipo_transporte_id = table.Column<int>(type: "integer", nullable: false),
                    estacionamiento_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    validado_por = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    distancia_km = table.Column<decimal>(type: "numeric(6,2)", nullable: false),
                    validado = table.Column<bool>(type: "boolean", nullable: false),
                    fecha_registro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registros_viajes", x => x.registro_id);
                    table.ForeignKey(
                        name: "FK_registros_viajes_estacionamientos_estacionamiento_id",
                        column: x => x.estacionamiento_id,
                        principalTable: "estacionamientos",
                        principalColumn: "estacionamiento_id");
                    table.ForeignKey(
                        name: "FK_registros_viajes_tipos_transporte_tipo_transporte_id",
                        column: x => x.tipo_transporte_id,
                        principalTable: "tipos_transporte",
                        principalColumn: "tipo_transporte_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    cedula = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    rol_id = table.Column<int>(type: "integer", nullable: false),
                    campus_id = table.Column<int>(type: "integer", nullable: false),
                    carrera_id = table.Column<int>(type: "integer", nullable: true),
                    nombres = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    apellidos = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    correo_institucional = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    contrasena_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    avatar_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fecha_registro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.cedula);
                    table.ForeignKey(
                        name: "FK_usuarios_campus_campus_id",
                        column: x => x.campus_id,
                        principalTable: "campus",
                        principalColumn: "campus_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usuarios_carreras_carrera_id",
                        column: x => x.carrera_id,
                        principalTable: "carreras",
                        principalColumn: "carrera_id");
                    table.ForeignKey(
                        name: "FK_usuarios_roles_rol_id",
                        column: x => x.rol_id,
                        principalTable: "roles",
                        principalColumn: "rol_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "conversaciones_chatbot",
                columns: table => new
                {
                    conversacion_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usuario_cedula = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversaciones_chatbot", x => x.conversacion_id);
                    table.ForeignKey(
                        name: "FK_conversaciones_chatbot_usuarios_usuario_cedula",
                        column: x => x.usuario_cedula,
                        principalTable: "usuarios",
                        principalColumn: "cedula",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "metricas_usuario",
                columns: table => new
                {
                    usuario_cedula = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    total_km = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    total_co2_evitado_g = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    total_calorias = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    total_ahorro_dolares = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    dias_registrados = table.Column<int>(type: "integer", nullable: false),
                    racha_actual_dias = table.Column<int>(type: "integer", nullable: false),
                    mejor_racha_dias = table.Column<int>(type: "integer", nullable: false),
                    ultima_actualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metricas_usuario", x => x.usuario_cedula);
                    table.ForeignKey(
                        name: "FK_metricas_usuario_usuarios_usuario_cedula",
                        column: x => x.usuario_cedula,
                        principalTable: "usuarios",
                        principalColumn: "cedula",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "interacciones_chatbot",
                columns: table => new
                {
                    interaccion_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    conversacion_id = table.Column<int>(type: "integer", nullable: false),
                    pregunta_id = table.Column<int>(type: "integer", nullable: true),
                    mensaje_usuario = table.Column<string>(type: "text", nullable: true),
                    respuesta_bot = table.Column<string>(type: "text", nullable: true),
                    fecha_interaccion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interacciones_chatbot", x => x.interaccion_id);
                    table.ForeignKey(
                        name: "FK_interacciones_chatbot_conversaciones_chatbot_conversacion_id",
                        column: x => x.conversacion_id,
                        principalTable: "conversaciones_chatbot",
                        principalColumn: "conversacion_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_interacciones_chatbot_preguntas_frecuentes_pregunta_id",
                        column: x => x.pregunta_id,
                        principalTable: "preguntas_frecuentes",
                        principalColumn: "pregunta_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_carreras_facultad_id",
                table: "carreras",
                column: "facultad_id");

            migrationBuilder.CreateIndex(
                name: "IX_conversaciones_chatbot_usuario_cedula",
                table: "conversaciones_chatbot",
                column: "usuario_cedula");

            migrationBuilder.CreateIndex(
                name: "IX_estacionamientos_campus_id",
                table: "estacionamientos",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "IX_estacionamientos_estado_estacionamiento_id",
                table: "estacionamientos",
                column: "estado_estacionamiento_id");

            migrationBuilder.CreateIndex(
                name: "IX_interacciones_chatbot_conversacion_id",
                table: "interacciones_chatbot",
                column: "conversacion_id");

            migrationBuilder.CreateIndex(
                name: "IX_interacciones_chatbot_pregunta_id",
                table: "interacciones_chatbot",
                column: "pregunta_id");

            migrationBuilder.CreateIndex(
                name: "IX_PermisoRol_RolesRolId",
                table: "PermisoRol",
                column: "RolesRolId");

            migrationBuilder.CreateIndex(
                name: "IX_registros_viajes_estacionamiento_id",
                table: "registros_viajes",
                column: "estacionamiento_id");

            migrationBuilder.CreateIndex(
                name: "IX_registros_viajes_tipo_transporte_id",
                table: "registros_viajes",
                column: "tipo_transporte_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_campus_id",
                table: "usuarios",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_carrera_id",
                table: "usuarios",
                column: "carrera_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_rol_id",
                table: "usuarios",
                column: "rol_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "interacciones_chatbot");

            migrationBuilder.DropTable(
                name: "metricas_usuario");

            migrationBuilder.DropTable(
                name: "PermisoRol");

            migrationBuilder.DropTable(
                name: "registros_viajes");

            migrationBuilder.DropTable(
                name: "conversaciones_chatbot");

            migrationBuilder.DropTable(
                name: "preguntas_frecuentes");

            migrationBuilder.DropTable(
                name: "permisos");

            migrationBuilder.DropTable(
                name: "estacionamientos");

            migrationBuilder.DropTable(
                name: "tipos_transporte");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "estados_estacionamiento");

            migrationBuilder.DropTable(
                name: "campus");

            migrationBuilder.DropTable(
                name: "carreras");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "facultades");
        }
    }
}
