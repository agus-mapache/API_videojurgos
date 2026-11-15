using FluentMigrator;

[Migration(202611140003)]
public class UpdateSeededJuegosImages : Migration
{
    public override void Up()
    {
        Execute.Sql("""
            UPDATE juegos SET imagen_url = 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/220/header.jpg' WHERE slug = 'half-life-2' AND imagen_url IS NULL;
            UPDATE juegos SET imagen_url = 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1245620/header.jpg' WHERE slug = 'elden-ring' AND imagen_url IS NULL;
            UPDATE juegos SET imagen_url = 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1086940/header.jpg' WHERE slug = 'baldurs-gate-3' AND imagen_url IS NULL;
            UPDATE juegos SET imagen_url = 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/620/header.jpg' WHERE slug = 'portal-2' AND imagen_url IS NULL;
            UPDATE juegos SET imagen_url = 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/374320/header.jpg' WHERE slug = 'dark-souls-iii' AND imagen_url IS NULL;
            UPDATE juegos SET imagen_url = 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/271590/header.jpg' WHERE slug = 'grand-theft-auto-v' AND imagen_url IS NULL;
            UPDATE juegos SET imagen_url = 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/2050650/header.jpg' WHERE slug = 'resident-evil-4-remake' AND imagen_url IS NULL;
            UPDATE juegos SET imagen_url = 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1172470/header.jpg' WHERE slug = 'apex-legends' AND imagen_url IS NULL;
            """);
    }

    public override void Down()
    {
        // No-op: preserves seeded image URLs on rollback
    }
}
