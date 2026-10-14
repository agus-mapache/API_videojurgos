using FluentMigrator;

[Migration(202610140003)]
public class CreateJuegosTable : Migration
{
    public override void Up()
    {
        Create.Table("juegos")
            .WithColumn("id_juego").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("titulo").AsString(150).NotNullable()
            .WithColumn("slug").AsString(150).NotNullable().Unique()
            .WithColumn("descripcion").AsString(int.MaxValue).Nullable() // Equivalente a TEXT
            .WithColumn("precio_base").AsDecimal(8, 2).NotNullable()
            .WithColumn("descuento_porcentaje").AsInt32().Nullable().WithDefaultValue(0)
            .WithColumn("fecha_lanzamiento").AsDate().Nullable()
            .WithColumn("id_desarrollador").AsInt32().Nullable()
            .WithColumn("id_editor").AsInt32().Nullable();

        // Clave foránea para el Desarrollador
        Create.ForeignKey("FK_juegos_empresas_id_desarrollador")
            .FromTable("juegos").ForeignColumn("id_desarrollador")
            .ToTable("empresas").PrimaryColumn("id_empresa")
            .OnDelete(System.Data.Rule.SetNull);

        // Clave foránea para el Editor
        Create.ForeignKey("FK_juegos_empresas_id_editor")
            .FromTable("juegos").ForeignColumn("id_editor")
            .ToTable("empresas").PrimaryColumn("id_empresa")
            .OnDelete(System.Data.Rule.SetNull);
    }

    public override void Down()
    {
        Delete.Table("juegos");
    }
}