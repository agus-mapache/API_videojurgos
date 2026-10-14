using FluentMigrator;

[Migration(202610140009)]
public class CreateResenasTable : Migration
{
    public override void Up()
    {
        Create.Table("resenas")
            .WithColumn("id_resena").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("id_usuario").AsInt32().NotNullable()
            .WithColumn("id_juego").AsInt32().NotNullable()
            .WithColumn("recomendado").AsBoolean().NotNullable()
            .WithColumn("comentario").AsString(int.MaxValue).Nullable() // Equivalente a TEXT
            .WithColumn("fecha_publicacion").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);

        // Clave foránea hacia usuarios (comportamiento por defecto)
        Create.ForeignKey("FK_resenas_usuarios_id_usuario")
            .FromTable("resenas").ForeignColumn("id_usuario")
            .ToTable("usuarios").PrimaryColumn("id_usuario");

        // Clave foránea hacia juegos (CASCADE)
        Create.ForeignKey("FK_resenas_juegos_id_juego")
            .FromTable("resenas").ForeignColumn("id_juego")
            .ToTable("juegos").PrimaryColumn("id_juego")
            .OnDelete(System.Data.Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("resenas");
    }
} 