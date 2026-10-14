using FluentMigrator;

[Migration(202610140002)]
public class CreateUsuariosTable : Migration
{
    public override void Up()
    {
        Create.Table("usuarios")
            .WithColumn("id_usuario").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("nombre_usuario").AsString(50).NotNullable().Unique()
            .WithColumn("email").AsString(100).NotNullable().Unique()
            .WithColumn("password_hash").AsString(255).NotNullable()
            .WithColumn("saldo_billetera").AsDecimal(10, 2).NotNullable().WithDefaultValue(0.00m)
            .WithColumn("tipo_usuario").AsString(20).NotNullable().WithDefaultValue("Cliente")
            .WithColumn("id_empresa").AsInt32().Nullable()
            .WithColumn("fecha_registro").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);

        // Definición de la Clave Foránea (Foreign Key)
        Create.ForeignKey("FK_usuarios_empresas_id_empresa")
            .FromTable("usuarios").ForeignColumn("id_empresa")
            .ToTable("empresas").PrimaryColumn("id_empresa")
            .OnDelete(System.Data.Rule.SetNull);
    }

    public override void Down()
    {
        Delete.Table("usuarios");
    }
}