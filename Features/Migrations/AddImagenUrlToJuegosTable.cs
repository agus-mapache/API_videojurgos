using FluentMigrator;

[Migration(202611140001)]
public class AddImagenUrlToJuegosTable : Migration
{
    public override void Up()
    {
        if (!Schema.Table("juegos").Column("imagen_url").Exists())
        {
            Alter.Table("juegos")
                .AddColumn("imagen_url").AsString(500).Nullable();
        }
    }

    public override void Down()
    {
        if (Schema.Table("juegos").Column("imagen_url").Exists())
        {
            Delete.Column("imagen_url").FromTable("juegos");
        }
    }
}
