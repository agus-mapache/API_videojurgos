using FluentMigrator;

[Migration(202610140007)]
public class CreateDetalleOrdenTable : Migration
{
    public override void Up()
    {
        Create.Table("detalle_orden")
            .WithColumn("id_detalle").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("id_orden").AsInt32().NotNullable()
            .WithColumn("id_juego").AsInt32().NotNullable()
            .WithColumn("precio_comprado").AsDecimal(8, 2).NotNullable();

        // Clave foránea hacia ordenes (CASCADE)
        Create.ForeignKey("FK_detalle_orden_ordenes_id_orden")
            .FromTable("detalle_orden").ForeignColumn("id_orden")
            .ToTable("ordenes").PrimaryColumn("id_orden")
            .OnDelete(System.Data.Rule.Cascade);

        // Clave foránea hacia juegos
        Create.ForeignKey("FK_detalle_orden_juegos_id_juego")
            .FromTable("detalle_orden").ForeignColumn("id_juego")
            .ToTable("juegos").PrimaryColumn("id_juego");
    }

    public override void Down()
    {
        Delete.Table("detalle_orden");
    }
}