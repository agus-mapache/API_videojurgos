using FluentMigrator;

[Migration(202610140004)]
public class CreateGenerosTable : Migration
{
    public override void Up()
    {
        Create.Table("generos")
            .WithColumn("id_genero").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("nombre").AsString(50).NotNullable().Unique();
    }

    public override void Down()
    {
        Delete.Table("generos");
    }
}