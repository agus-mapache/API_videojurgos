using FluentMigrator;

[Migration(202611140004)]
public class UpgradeSeededJuegosImagesToHero : Migration
{
    public override void Up()
    {
        Execute.Sql("""
            UPDATE juegos
            SET imagen_url = REPLACE(imagen_url, '/header.jpg', '/library_hero.jpg')
            WHERE imagen_url LIKE 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/%/header.jpg';
            """);
    }

    public override void Down()
    {
        Execute.Sql("""
            UPDATE juegos
            SET imagen_url = REPLACE(imagen_url, '/library_hero.jpg', '/header.jpg')
            WHERE imagen_url LIKE 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/%/library_hero.jpg';
            """);
    }
}
