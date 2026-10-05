DROP DATABASE IF EXISTS tienda_videojuegos;
CREATE DATABASE tienda_videojuegos;
USE tienda_videojuegos;

-- 1. EMPRESAS (DESARROLLADORES Y EDITORES)
CREATE TABLE empresas (
    id_empresa INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    sitio_web VARCHAR(255)
);

-- 2. USUARIOS
CREATE TABLE usuarios (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    nombre_usuario VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(100) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    saldo_billetera DECIMAL(10, 2) DEFAULT 0.00,
    tipo_usuario ENUM('Cliente', 'Desarrollador', 'Admin') DEFAULT 'Cliente',
    id_empresa INT NULL,
    fecha_registro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_empresa) REFERENCES empresas(id_empresa) ON DELETE SET NULL
);

-- 3. JUEGOS
CREATE TABLE juegos (
    id_juego INT AUTO_INCREMENT PRIMARY KEY,
    titulo VARCHAR(150) NOT NULL,
    slug VARCHAR(150) NOT NULL UNIQUE,
    descripcion TEXT,
    precio_base DECIMAL(8, 2) NOT NULL,
    descuento_porcentaje INT DEFAULT 0,
    fecha_lanzamiento DATE,
    id_desarrollador INT,
    id_editor INT,
    FOREIGN KEY (id_desarrollador) REFERENCES empresas(id_empresa) ON DELETE SET NULL,
    FOREIGN KEY (id_editor) REFERENCES empresas(id_empresa) ON DELETE SET NULL
);

-- 4. GÉNEROS Y RELACIÓN MUCHOS A MUCHOS
CREATE TABLE generos (
    id_genero INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE juego_generos (
    id_juego INT NOT NULL,
    id_genero INT NOT NULL,
    PRIMARY KEY (id_juego, id_genero),
    FOREIGN KEY (id_juego) REFERENCES juegos(id_juego) ON DELETE CASCADE,
    FOREIGN KEY (id_genero) REFERENCES generos(id_genero) ON DELETE CASCADE
);

-- 5. ÓRDENES Y DETALLES DE COMPRA
CREATE TABLE ordenes (
    id_orden INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    monto_total DECIMAL(10, 2) NOT NULL,
    estado ENUM('Pendiente', 'Completado', 'Reembolsado') DEFAULT 'Pendiente',
    metodo_pago VARCHAR(50),
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario)
);

CREATE TABLE detalle_orden (
    id_detalle INT AUTO_INCREMENT PRIMARY KEY,
    id_orden INT NOT NULL,
    id_juego INT NOT NULL,
    precio_comprado DECIMAL(8, 2) NOT NULL,
    FOREIGN KEY (id_orden) REFERENCES ordenes(id_orden) ON DELETE CASCADE,
    FOREIGN KEY (id_juego) REFERENCES juegos(id_juego)
);

-- 6. BIBLIOTECA DEL USUARIO (Licencias activas)
CREATE TABLE biblioteca (
    id_usuario INT NOT NULL,
    id_juego INT NOT NULL,
    fecha_adquisicion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    horas_jugadas DECIMAL(6, 1) DEFAULT 0.0,
    PRIMARY KEY (id_usuario, id_juego),
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario) ON DELETE CASCADE,
    FOREIGN KEY (id_juego) REFERENCES juegos(id_juego) ON DELETE CASCADE
);

-- 7. RESEÑAS
CREATE TABLE resenas (
    id_resena INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_juego INT NOT NULL,
    recomendado BOOLEAN NOT NULL,
    comentario TEXT,
    fecha_publicacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario),
    FOREIGN KEY (id_juego) REFERENCES juegos(id_juego) ON DELETE CASCADE
);

-- =============================================
-- INSERTS 
-- =============================================

INSERT INTO empresas (nombre, sitio_web) VALUES
('Valve Corporation', 'https://www.valvesoftware.com'),
('CD Projekt Red', 'https://www.cdprojektred.com'),
('FromSoftware', 'https://www.fromsoftware.jp'),
('Larian Studios', 'https://larian.com'),
('Nintendo', 'https://www.nintendo.com'),
('Sony Interactive Entertainment', 'https://www.playstation.com'),
('Rockstar Games', 'https://www.rockstargames.com'),
('Electronic Arts', 'https://www.ea.com'),
('Capcom', 'https://www.capcom.com'),
('IndieMegacorp', 'https://www.indiemegacorp.test');

INSERT INTO usuarios (nombre_usuario, email, password_hash, saldo_billetera, tipo_usuario, id_empresa) VALUES
('gamer_pro99', 'gamer99@email.com', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 45.50, 'Cliente', NULL),
('elena_stark', 'elena@email.com', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 120.00, 'Desarrollador', 1),
('carlos_98', 'carlos98@email.com', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 5.25, 'Cliente', NULL),
('lucia_gamer', 'lucia@email.com', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 250.00, 'Admin', NULL),
('darksouls_fan', 'chosenundead@email.com', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 0.00, 'Cliente', NULL),
('valeria_dev', 'valeria@email.com', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 15.80, 'Desarrollador', 2),
('marcos_pc', 'marcos@email.com', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 89.90, 'Cliente', NULL),
('sofia_rpg', 'sofia@email.com', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 62.10, 'Desarrollador', 3),
('tomas_fps', 'tomas@email.com', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 14.00, 'Cliente', NULL),
('ana_twitch', 'ana@email.com', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 310.45, 'Admin', NULL);

INSERT INTO juegos (titulo, slug, descripcion, precio_base, descuento_porcentaje, fecha_lanzamiento, id_desarrollador, id_editor) VALUES
('Half-Life 2', 'half-life-2', 'Juego clásico de disparos en primera persona.', 9.99, 50, '2004-11-16', 1, 1),
('Cyberpunk 2077', 'cyberpunk-2077', 'RPG de mundo abierto futurista.', 59.99, 20, '2020-12-10', 2, 2),
('Elden Ring', 'elden-ring', 'Action RPG de mundo abierto desafiante.', 59.99, 0, '2022-02-25', 3, 6),
('Baldurs Gate 3', 'baldurs-gate-3', 'RPG táctico basado en D&D.', 59.99, 0, '2023-08-03', 4, 4),
('Portal 2', 'portal-2', 'Juego de puzles en primera persona.', 9.99, 75, '2011-04-19', 1, 1),
('The Witcher 3: Wild Hunt', 'the-witcher-3-wild-hunt', 'Aventura épica de rol en mundo abierto.', 39.99, 60, '2015-05-19', 2, 2),
('Dark Souls III', 'dark-souls-iii', 'Action RPG de alta dificultad.', 39.99, 50, '2016-03-24', 3, 8),
('Grand Theft Auto V', 'grand-theft-auto-v', 'Acción y aventura en mundo abierto.', 29.99, 40, '2015-04-14', 7, 7),
('Resident Evil 4 Remake', 'resident-evil-4-remake', 'Survival horror de acción reimaginado.', 49.99, 25, '2023-03-24', 9, 9),
('Apex Legends', 'apex-legends', 'Battle Royale multijugador gratuito.', 0.00, 0, '2019-02-04', 8, 8);

INSERT INTO generos (nombre) VALUES
('Acción'),
('Aventura'),
('RPG'),
('Estrategia'),
('Shooter'),
('Indie'),
('Puzle'),
('Terror'),
('Deportes'),
('Mundo Abierto');

INSERT INTO juego_generos (id_juego, id_genero) VALUES
(1, 1), (1, 5), -- Half-Life 2 (Acción, Shooter)
(2, 1), (2, 3), (2, 10), -- Cyberpunk 2077 (Acción, RPG, Mundo Abierto)
(3, 1), (3, 3), (3, 10), -- Elden Ring (Acción, RPG, Mundo Abierto)
(4, 3), (4, 4), -- Baldurs Gate 3 (RPG, Estrategia)
(5, 2), (5, 7), -- Portal 2 (Aventura, Puzle)
(6, 2), (6, 3), (6, 10), -- The Witcher 3 (Aventura, RPG, Mundo Abierto)
(7, 1), (7, 3), -- Dark Souls III (Acción, RPG)
(8, 1), (8, 2), (8, 10), -- GTA V (Acción, Aventura, Mundo Abierto)
(9, 1), (9, 8), -- Resident Evil 4 (Acción, Terror)
(10, 1), (10, 5); -- Apex Legends (Acción, Shooter)

INSERT INTO ordenes (id_usuario, monto_total, estado, metodo_pago) VALUES
(1, 4.99, 'Completado', 'Tarjeta de Crédito'),
(2, 47.99, 'Completado', 'PayPal'),
(3, 59.99, 'Completado', 'Saldo Billetera'),
(4, 15.99, 'Completado', 'Tarjeta de Débito'),
(5, 19.99, 'Completado', 'PayPal'),
(6, 59.99, 'Completado', 'Tarjeta de Crédito'),
(7, 2.50, 'Pendiente', 'PayPal'),
(8, 39.99, 'Completado', 'Saldo Billetera'),
(9, 37.49, 'Completado', 'Tarjeta de Crédito'),
(10, 0.00, 'Completado', 'Gratuito');

INSERT INTO detalle_orden (id_orden, id_juego, precio_comprado) VALUES
(1, 5, 4.99),   -- Portal 2 (con 75% descuento)
(2, 2, 47.99),  -- Cyberpunk 2077 (con 20% descuento)
(3, 3, 59.99),  -- Elden Ring
(4, 1, 4.99),   -- Half-Life 2 (con 50% descuento)
(5, 7, 19.99),  -- Dark Souls III (con 50% descuento)
(6, 4, 59.99),  -- Baldurs Gate 3
(7, 5, 2.50),   -- Portal 2 (precio promocional extra)
(8, 6, 39.99),  -- The Witcher 3
(9, 9, 37.49),  -- Resident Evil 4 Remake (con 25% descuento)
(10, 10, 0.00); -- Apex Legends (Gratis)

INSERT INTO biblioteca (id_usuario, id_juego, horas_jugadas) VALUES
(1, 5, 42.5),  -- Portal 2
(2, 2, 85.0),  -- Cyberpunk 2077
(3, 3, 120.3), -- Elden Ring
(4, 1, 15.0),  -- Half-Life 2
(5, 7, 240.0), -- Dark Souls III
(6, 4, 95.5),  -- Baldurs Gate 3
(8, 6, 110.0), -- The Witcher 3
(9, 9, 22.4),  -- Resident Evil 4 Remake
(10, 10, 350.8),-- Apex Legends
(1, 1, 10.2);  -- Half-Life 2 (Usuario 1 también lo tiene)

INSERT INTO resenas (id_usuario, id_juego, recomendado, comentario) VALUES
(1, 5, 1, '¡Un clásico absoluto de los puzles! La secuela mejora todo lo del primero.'),
(2, 2, 1, 'A pesar de sus bugs iniciales, la historia y la atmósfera de Night City son increíbles.'),
(3, 3, 1, 'Una obra maestra absoluta de FromSoftware. Desafiante pero muy gratificante.'),
(4, 1, 1, 'Un pilar histórico de los shooters. Ha envejecido muy bien.'),
(5, 7, 1, 'Morí más de 100 veces y lo volvería a hacer. ¡Excelente.'),
(6, 4, 1, 'El mejor RPG de elecciones de los últimos años. GOTY sin duda.'),
(7, 6, 1, 'Las misiones secundarias superan a la historia principal. Una maravilla.'),
(8, 9, 1, 'El remake perfecto. Mantiene la esencia pero con jugabilidad moderna.'),
(9, 10, 1, 'Muy buen battle royale, la movilidad de las leyendas es genial.'),
(10, 5, 1, 'La modalidad cooperativa con un amigo es desternillante y brillante.');

-- consultas --

SELECT id_genero, nombre 
FROM generos 
ORDER BY nombre ASC;

SELECT id_usuario, nombre_usuario, email, saldo_billetera, fecha_registro, tipo_usuario 
FROM usuarios 
ORDER BY id_usuario DESC;

SELECT id_empresa, nombre, sitio_web
FROM empresas
ORDER BY nombre DESC;

SELECT 
    j.id_juego,
    j.titulo,
    e.nombre AS desarrollador,
    j.precio_base,
    j.descuento_porcentaje,
    ROUND(j.precio_base * (1 - j.descuento_porcentaje / 100.0), 2) AS precio_final,
    j.fecha_lanzamiento,
    GROUP_CONCAT(g.nombre SEPARATOR ', ') AS generos
FROM juegos j
LEFT JOIN empresas e ON j.id_desarrollador = e.id_empresa
LEFT JOIN juego_generos jg ON j.id_juego = jg.id_juego
LEFT JOIN generos g ON jg.id_genero = g.id_genero
GROUP BY j.id_juego
ORDER BY j.titulo ASC;

SELECT 
    id_empresa,
    nombre AS empresa,
    sitio_web
FROM empresas
ORDER BY nombre ASC;

SELECT 
    e.id_empresa,
    e.nombre AS desarrollador,
    e.sitio_web,
    COUNT(j.id_juego) AS total_juegos_desarrollados
FROM empresas e
LEFT JOIN juegos j ON e.id_empresa = j.id_desarrollador
GROUP BY e.id_empresa, e.nombre, e.sitio_web
ORDER BY total_juegos_desarrollados DESC;

SELECT 
    o.id_orden,
    u.id_usuario AS idUsuario,
    u.nombre_usuario AS nombreUsuario,
    u.email,
    o.monto_total AS montoTotal,
    o.estado,
    o.metodo_pago AS metodoPago,
    o.fecha_creacion AS fechaCreacion,
    d.id_juego AS idJuego,
    j.titulo AS tituloJuego,
    d.precio_comprado AS precioComprado
FROM 
    ordenes o
JOIN 
    usuarios u ON o.id_usuario = u.id_usuario
JOIN 
    detalle_orden d ON o.id_orden = d.id_orden
JOIN 
    juegos j ON d.id_juego = j.id_juego
ORDER BY 
    o.fecha_creacion DESC;

SELECT 
    r.id_resena,
    u.id_usuario,
    u.nombre_usuario,
    j.titulo AS juego,
    r.recomendado,
    r.comentario,
    r.fecha_publicacion
FROM resenas r
JOIN usuarios u ON r.id_usuario = u.id_usuario
JOIN juegos j ON r.id_juego = j.id_juego;

SELECT 
    b.id_juego, 
    u.id_usuario, 
    u.tipo_usuario
FROM usuarios u
JOIN biblioteca b ON u.id_usuario = b.id_usuario;

SELECT 
    j.id_juego, 
    u.id_usuario, 
    u.tipo_usuario
FROM usuarios u
JOIN empresas e ON u.id_empresa = e.id_empresa
JOIN juegos j ON e.id_empresa = j.id_desarrollador
WHERE u.tipo_usuario = 'Desarrollador';