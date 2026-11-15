using FluentMigrator;

[Migration(202611140002)]
public class SeedInitialData : Migration
{
    public override void Up()
    {
        // todos los usuarios seed tienen la pass "password123"
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("password123");

        Execute.Sql($"""
            INSERT INTO empresas (id_empresa, nombre, sitio_web) VALUES
            (1, 'Valve Corporation', 'https://www.valvesoftware.com'),
            (2, 'CD Projekt Red', 'https://www.cdprojektred.com'),
            (3, 'FromSoftware', 'https://www.fromsoftware.jp'),
            (4, 'Larian Studios', 'https://larian.com'),
            (5, 'Nintendo', 'https://www.nintendo.com'),
            (6, 'Sony Interactive Entertainment', 'https://www.playstation.com'),
            (7, 'Rockstar Games', 'https://www.rockstargames.com'),
            (8, 'Electronic Arts', 'https://www.ea.com'),
            (9, 'Capcom', 'https://www.capcom.com'),
            (10, 'IndieMegacorp', 'https://www.indiemegacorp.test');

            INSERT INTO usuarios (id_usuario, nombre_usuario, email, password_hash, saldo_billetera, tipo_usuario, id_empresa) VALUES
            (1, 'gamer_pro99', 'gamer99@email.com', '{passwordHash}', 45.50, 'Cliente', NULL),
            (2, 'elena_stark', 'elena@email.com', '{passwordHash}', 120.00, 'Desarrollador', 1),
            (3, 'carlos_98', 'carlos98@email.com', '{passwordHash}', 5.25, 'Cliente', NULL),
            (4, 'lucia_gamer', 'lucia@email.com', '{passwordHash}', 250.00, 'Admin', NULL),
            (5, 'darksouls_fan', 'chosenundead@email.com', '{passwordHash}', 0.00, 'Cliente', NULL),
            (6, 'valeria_dev', 'valeria@email.com', '{passwordHash}', 15.80, 'Desarrollador', 2),
            (7, 'marcos_pc', 'marcos@email.com', '{passwordHash}', 89.90, 'Cliente', NULL),
            (8, 'sofia_rpg', 'sofia@email.com', '{passwordHash}', 62.10, 'Desarrollador', 3),
            (9, 'tomas_fps', 'tomas@email.com', '{passwordHash}', 14.00, 'Cliente', NULL),
            (10, 'ana_twitch', 'ana@email.com', '{passwordHash}', 310.45, 'Admin', NULL);

            INSERT INTO juegos (id_juego, titulo, slug, descripcion, precio_base, descuento_porcentaje, fecha_lanzamiento, id_desarrollador, id_editor, imagen_url, trailer_url) VALUES
            (1, 'Half-Life 2', 'half-life-2', 'Juego clásico de disparos en primera persona.', 9.99, 50, '2004-11-16', 1, 1, 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/220/library_hero.jpg', NULL),
            (2, 'Cyberpunk 2077', 'cyberpunk-2077', 'RPG de mundo abierto futurista.', 59.99, 20, '2020-12-10', 2, 2, '/images/imagenes-juegos/imagen_cyberpunk_2077.webp', 'https://www.youtube.com/embed/UnA7tepsc7s?rel=0'),
            (3, 'Elden Ring', 'elden-ring', 'Action RPG de mundo abierto desafiante.', 59.99, 0, '2022-02-25', 3, 6, 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1245620/library_hero.jpg', 'https://www.youtube.com/embed/K_03kFqWfqs?rel=0'),
            (4, 'Baldurs Gate 3', 'baldurs-gate-3', 'RPG táctico basado en D&D.', 59.99, 0, '2023-08-03', 4, 4, 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1086940/library_hero.jpg', 'https://www.youtube.com/embed/1_hXhnzUaVE?rel=0'),
            (5, 'Portal 2', 'portal-2', 'Juego de puzles en primera persona.', 9.99, 75, '2011-04-19', 1, 1, 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/620/library_hero.jpg', 'https://www.youtube.com/embed/tax4e4hBBZc?rel=0'),
            (6, 'The Witcher 3: Wild Hunt', 'the-witcher-3-wild-hunt', 'Aventura épica de rol en mundo abierto.', 39.99, 60, '2015-05-19', 2, 2, '/images/imagenes-juegos/imagen_the_witcher_3_wild_hunt.webp', 'https://www.youtube.com/embed/hBoCRYX6Vq0?rel=0'),
            (7, 'Dark Souls III', 'dark-souls-iii', 'Action RPG de alta dificultad.', 39.99, 50, '2016-03-24', 3, 8, 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/374320/library_hero.jpg', 'https://www.youtube.com/embed/QjY99zF3_to?rel=0'),
            (8, 'Grand Theft Auto V', 'grand-theft-auto-v', 'Acción y aventura en mundo abierto.', 29.99, 40, '2015-04-14', 7, 7, 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/271590/library_hero.jpg', 'https://www.youtube.com/embed/hvoD7ehZPcM?rel=0'),
            (9, 'Resident Evil 4 Remake', 'resident-evil-4-remake', 'Survival horror de acción reimaginado.', 49.99, 25, '2023-03-24', 9, 9, 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/2050650/library_hero.jpg', 'https://www.youtube.com/embed/jZ_zrFJdCIA?rel=0'),
            (10, 'Apex Legends', 'apex-legends', 'Battle Royale multijugador gratuito.', 0.00, 0, '2019-02-04', 8, 8, 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1172470/library_hero.jpg', 'https://www.youtube.com/embed/vvB2wiDUDdA?rel=0');

            INSERT INTO generos (id_genero, nombre) VALUES
            (1, 'Acción'),
            (2, 'Aventura'),
            (3, 'RPG'),
            (4, 'Estrategia'),
            (5, 'Shooter'),
            (6, 'Indie'),
            (7, 'Puzle'),
            (8, 'Terror'),
            (9, 'Deportes'),
            (10, 'Mundo Abierto');

            INSERT INTO juego_generos (id_juego, id_genero) VALUES
            (1, 1), (1, 5),
            (2, 1), (2, 3), (2, 10),
            (3, 1), (3, 3), (3, 10),
            (4, 3), (4, 4),
            (5, 2), (5, 7),
            (6, 2), (6, 3), (6, 10),
            (7, 1), (7, 3),
            (8, 1), (8, 2), (8, 10),
            (9, 1), (9, 8),
            (10, 1), (10, 5);

            INSERT INTO ordenes (id_orden, id_usuario, monto_total, estado, metodo_pago) VALUES
            (1, 1, 4.99, 'Completado', 'Tarjeta de Crédito'),
            (2, 2, 47.99, 'Completado', 'PayPal'),
            (3, 3, 59.99, 'Completado', 'Saldo Billetera'),
            (4, 4, 15.99, 'Completado', 'Tarjeta de Débito'),
            (5, 5, 19.99, 'Completado', 'PayPal'),
            (6, 6, 59.99, 'Completado', 'Tarjeta de Crédito'),
            (7, 7, 2.50, 'Pendiente', 'PayPal'),
            (8, 8, 39.99, 'Completado', 'Saldo Billetera'),
            (9, 9, 37.49, 'Completado', 'Tarjeta de Crédito'),
            (10, 10, 0.00, 'Completado', 'Gratuito');

            INSERT INTO detalle_orden (id_detalle, id_orden, id_juego, precio_comprado) VALUES
            (1, 1, 5, 4.99),
            (2, 2, 2, 47.99),
            (3, 3, 3, 59.99),
            (4, 4, 1, 4.99),
            (5, 5, 7, 19.99),
            (6, 6, 4, 59.99),
            (7, 7, 5, 2.50),
            (8, 8, 6, 39.99),
            (9, 9, 9, 37.49),
            (10, 10, 10, 0.00);

            INSERT INTO biblioteca (id_usuario, id_juego, horas_jugadas) VALUES
            (1, 5, 42.5),
            (2, 2, 85.0),
            (3, 3, 120.3),
            (4, 1, 15.0),
            (5, 7, 240.0),
            (6, 4, 95.5),
            (8, 6, 110.0),
            (9, 9, 22.4),
            (10, 10, 350.8),
            (1, 1, 10.2);

            INSERT INTO resenas (id_resena, id_usuario, id_juego, recomendado, comentario) VALUES
            (1, 1, 5, 1, '¡Un clásico absoluto de los puzles! La secuela mejora todo lo del primero.'),
            (2, 2, 2, 1, 'A pesar de sus bugs iniciales, la historia y la atmósfera de Night City son increíbles.'),
            (3, 3, 3, 1, 'Una obra maestra absoluta de FromSoftware. Desafiante pero muy gratificante.'),
            (4, 4, 1, 1, 'Un pilar histórico de los shooters. Ha envejecido muy bien.'),
            (5, 5, 7, 1, 'Morí más de 100 veces y lo volvería a hacer. ¡Excelente.'),
            (6, 6, 4, 1, 'El mejor RPG de elecciones de los últimos años. GOTY sin duda.'),
            (7, 7, 6, 1, 'Las misiones secundarias superan a la historia principal. Una maravilla.'),
            (8, 8, 9, 1, 'El remake perfecto. Mantiene la esencia pero con jugabilidad moderna.'),
            (9, 9, 10, 1, 'Muy buen battle royale, la movilidad de las leyendas es genial.'),
            (10, 10, 5, 1, 'La modalidad cooperativa con un amigo es desternillante y brillante.');
            """);
    }

    public override void Down()
    {
        Execute.Sql("""
            DELETE FROM resenas WHERE id_resena BETWEEN 1 AND 10;
            DELETE FROM biblioteca WHERE id_usuario BETWEEN 1 AND 10 AND id_juego BETWEEN 1 AND 10;
            DELETE FROM detalle_orden WHERE id_detalle BETWEEN 1 AND 10;
            DELETE FROM ordenes WHERE id_orden BETWEEN 1 AND 10;
            DELETE FROM juego_generos WHERE id_juego BETWEEN 1 AND 10;
            DELETE FROM generos WHERE id_genero BETWEEN 1 AND 10;
            DELETE FROM juegos WHERE id_juego BETWEEN 1 AND 10;
            DELETE FROM usuarios WHERE id_usuario BETWEEN 1 AND 10;
            DELETE FROM empresas WHERE id_empresa BETWEEN 1 AND 10;
            """);
    }
}
