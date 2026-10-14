using FluentMigrator;

[Migration(202610140005)]
public class CreateJuegoGenerosTable : Migration
{
    public override void Up()
    {
        Create.Table("juego_generos")
            .WithColumn("id_juego").AsInt32().NotNullable().PrimaryKey()
            .WithColumn("id_genero").AsInt32().NotNullable().PrimaryKey();

        Create.ForeignKey("FK_juego_generos_juegos_id_juego")
            .FromTable("juego_generos").ForeignColumn("id_juego")
            .ToTable("juegos").PrimaryColumn("id_juego")
            .OnDelete(System.Data.Rule.Cascade);

        Create.ForeignKey("FK_juego_generos_generos_id_genero")
            .FromTable("juego_generos").ForeignColumn("id_genero")
            .ToTable("generos").PrimaryColumn("id_genero")
            .OnDelete(System.Data.Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("juego_generos");
    }
}