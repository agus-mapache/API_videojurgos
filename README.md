🎮 VideoJuegos API (Backend)
API REST minimalista desarrollada para gestionar el catálogo, la visualización y la compra de videojuegos en una plataforma de comercio electrónico.
---
🚀 Tecnologías Utilizadas
Node.js - Entorno de ejecución.
Express - Framework web para Node.js.
MongoDB / Mongoose - Base de datos para almacenar el catálogo y las órdenes.
Cors - Middleware para habilitar solicitudes de origen cruzado (CORS).
---
📂 Estructura del Proyecto
```text
/backend
│
├── src/
│   ├── controllers/   # Lógica de negocio (juegos, transacciones)
│   ├── models/        # Esquemas de la base de datos
│   ├── routes/        # Definición de rutas de la API
│   └── app.js         # Configuración principal de Express
│
├── .env.example       # Plantilla para variables de entorno
├── package.json       # Dependencias y scripts
└── README.md          # Documentación del proyecto
```
---
⚙️ Instalación y Configuración Local
Sigue estos pasos para clonar y poner en marcha la API en tu máquina local:
Clona el repositorio:
```bash
   git clone https://github.com/tu-usuario/tu-repositorio.git
   cd tu-repositorio
   ```
Instala las dependencias:
```bash
   npm install
   ```
Configura las variables de entorno:
Crea un archivo `.env` en la raíz del proyecto tomando de referencia `.env.example`:
```env
   PORT=3000
   MONGO_URI=mongodb://localhost:27017/gamestore
   ```
Ejecuta el servidor en modo desarrollo:
```bash
   npm run dev
   ```
El servidor estará disponible en `http://localhost:3000`.
---
🔌 Endpoints de la API
Videojuegos
Método	Ruta	Descripción
GET	`/api/games`	Obtiene todos los videojuegos del catálogo.
GET	`/api/games/:id`	Obtiene el detalle de un videojuego por ID.
POST	`/api/games`	Registra un nuevo videojuego en el catálogo.
PUT	`/api/games/:id`	Actualiza la información o precio de un videojuego.
DELETE	`/api/games/:id`	Elimina un videojuego del catálogo.
Compras / Transacciones
Método	Ruta	Descripción
POST	`/api/checkout`	Procesa la compra de una lista de videojuegos.
---
🛠️ Próximas Mejoras
[ ] Implementar autenticación y autorización mediante JWT.
[ ] Integrar pasarela de pago (MercadoPago / Stripe).
[ ] Añadir sistema de categorías y filtros avanzados.
---
📄 Licencia
Proyecto bajo la licencia MIT.
