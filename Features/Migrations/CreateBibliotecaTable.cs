using FluentMigrator;

[Migration(202610140008)]
public class CreateBibliotecaTable : Migration
{
    public override void Up()
    {
        Create.Table("biblioteca")
            .WithColumn("id_usuario").AsInt32().NotNullable().PrimaryKey()
            .WithColumn("id_juego").AsInt32().NotNullable().PrimaryKey()
            .WithColumn("fecha_adquisicion").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("horas_jugadas").AsDecimal(6, 1).NotNullable().WithDefaultValue(0.0m);

        // Clave foránea hacia usuarios (CASCADE)
        Create.ForeignKey("FK_biblioteca_usuarios_id_usuario")
            .FromTable("biblioteca").ForeignColumn("id_usuario")
            .ToTable("usuarios").PrimaryColumn("id_usuario")
            .OnDelete(System.Data.Rule.Cascade);

        // Clave foránea hacia juegos (CASCADE)
        Create.ForeignKey("FK_biblioteca_juegos_id_juego")
            .FromTable("biblioteca").ForeignColumn("id_juego")
            .ToTable("juegos").PrimaryColumn("id_juego")
            .OnDelete(System.Data.Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("biblioteca");
    }
}