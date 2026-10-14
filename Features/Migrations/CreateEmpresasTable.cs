using FluentMigrator;

[Migration(202610140001)]
public class CreateEmpresasTable : Migration
{
    public override void Up()
    {
        Create.Table("empresas")
            .WithColumn("id_empresa").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("nombre").AsString(100).NotNullable()
            .WithColumn("sitio_web").AsString(255).Nullable();
    }

    public override void Down()
    {
        Delete.Table("empresas");
    }
}