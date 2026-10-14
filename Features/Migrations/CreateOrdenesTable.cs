using FluentMigrator;

[Migration(202610140006)]
public class CreateOrdenesTable : Migration
{
    public override void Up()
    {
        Create.Table("ordenes")
            .WithColumn("id_orden").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("id_usuario").AsInt32().NotNullable()
            .WithColumn("monto_total").AsDecimal(10, 2).NotNullable()
            .WithColumn("estado").AsString(20).NotNullable().WithDefaultValue("Pendiente") // ENUM mapeado a String
            .WithColumn("metodo_pago").AsString(50).Nullable()
            .WithColumn("fecha_creacion").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);

        // Clave foránea hacia usuarios
        Create.ForeignKey("FK_ordenes_usuarios_id_usuario")
            .FromTable("ordenes").ForeignColumn("id_usuario")
            .ToTable("usuarios").PrimaryColumn("id_usuario");
    }

    public override void Down()
    {
        Delete.Table("ordenes");
    }
}