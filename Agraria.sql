/* =========================================================
   AGRARIA – Esquema base para Proyecto_Agraria_Pacifico
   Idempotente: lo podés correr varias veces sin romper nada
   ========================================================= */

-----------------------------
-- 0) Crear DB si no existe
-----------------------------
IF DB_ID('Agraria') IS NULL
BEGIN
    CREATE DATABASE Agraria;
END
GO

USE Agraria;
GO

------------------------------------------------------------
-- 1) Tabla: USUARIOS (usado por Login, Alta/Consulta/ABM)
------------------------------------------------------------
IF OBJECT_ID('dbo.Usuarios','U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios
    (
        IdUsuario           INT IDENTITY(1,1) PRIMARY KEY,
        Nombre              NVARCHAR(100) NULL,
        Apellido            NVARCHAR(100) NULL,
        DNI                 NVARCHAR(20)  NULL,
        Email               NVARCHAR(150) NULL,
        Telefono            NVARCHAR(50)  NULL,
        UsuarioLogin        NVARCHAR(100) NOT NULL,
        Contrasenia         NVARCHAR(200) NULL,   -- se asegura abajo si faltara
        Direccion           NVARCHAR(150) NULL,
        Localidad           NVARCHAR(100) NULL,
        Provincia           NVARCHAR(100) NULL,
        Observaciones       NVARCHAR(400) NULL,
        Rol                 NVARCHAR(50)  NOT NULL,
        Estado              NVARCHAR(20)  NULL CONSTRAINT DF_Usuarios_Estado DEFAULT N'Activo',
        Area                NVARCHAR(80)  NULL,
        PreguntaSeguridad   NVARCHAR(200) NULL,
        RespuestaSeguridad  NVARCHAR(200) NULL,
        FechaAlta           DATETIME2      NOT NULL CONSTRAINT DF_Usuarios_FechaAlta DEFAULT SYSUTCDATETIME()
    );
END
GO

-- Columnas/índice de compatibilidad
IF COL_LENGTH('dbo.Usuarios','Contrasenia') IS NULL
    ALTER TABLE dbo.Usuarios ADD Contrasenia NVARCHAR(200) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='UX_Usuarios_UsuarioLogin' AND object_id=OBJECT_ID('dbo.Usuarios'))
    CREATE UNIQUE INDEX UX_Usuarios_UsuarioLogin ON dbo.Usuarios(UsuarioLogin);
GO

-- Semillas de usuarios (solo si no existen)
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE UsuarioLogin = N'admin')
BEGIN
    INSERT INTO dbo.Usuarios (Nombre, Apellido, UsuarioLogin, Contrasenia, Rol, Estado, Area,
                              PreguntaSeguridad, RespuestaSeguridad, Email)
    VALUES (N'Admin', N'Sistema', N'admin', N'1234', N'Administrador', N'Activo', N'Administración',
            N'¿Ciudad donde naciste?', N'Adrogué', N'admin@demo.local');
END

IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE UsuarioLogin = N'jefe')
BEGIN
    INSERT INTO dbo.Usuarios (Nombre, Apellido, UsuarioLogin, Contrasenia, Rol, Estado, Area)
    VALUES (N'Jefe', N'De Área', N'jefe', N'jefe123', N'Jefe de area', N'Activo', N'Vegetal');
END

IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE UsuarioLogin = N'docente')
BEGIN
    INSERT INTO dbo.Usuarios (Nombre, Apellido, UsuarioLogin, Contrasenia, Rol, Estado, Area)
    VALUES (N'Docente', N'Prueba', N'docente', N'docente123', N'Docente', N'Activo', N'Animal');
END

IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE UsuarioLogin = N'invitado')
BEGIN
    INSERT INTO dbo.Usuarios (Nombre, Apellido, UsuarioLogin, Contrasenia, Rol, Estado)
    VALUES (N'Invitado', N'Demo', N'invitado', N'invitado', N'Invitado', N'Activo');
END
GO


------------------------------------------------------------
-- 2) Tabla: ENTORNOS FORMATIVOS (Alta / Consulta / Activ.)
------------------------------------------------------------
IF OBJECT_ID('dbo.EntornosFormativos','U') IS NULL
BEGIN
    CREATE TABLE dbo.EntornosFormativos
    (
        Id            INT IDENTITY(1,1) PRIMARY KEY,
        Nombre        NVARCHAR(120) NOT NULL,
        Tipo          NVARCHAR(80)  NOT NULL,
        Profesor      NVARCHAR(120) NULL,
        Anio          NVARCHAR(20)  NULL,
        Division      NVARCHAR(20)  NULL,
        Grupo         NVARCHAR(40)  NULL,
        Observaciones NVARCHAR(300) NULL
    );
END
GO

-- Índice útil por nombre/año (no único)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Entornos_Nombre_Anio' AND object_id=OBJECT_ID('dbo.EntornosFormativos'))
    CREATE INDEX IX_Entornos_Nombre_Anio ON dbo.EntornosFormativos(Nombre, Anio, Division, Grupo);
GO

-- Semilla mínima
IF NOT EXISTS (SELECT 1 FROM dbo.EntornosFormativos WHERE Nombre LIKE N'%huerta%')
BEGIN
    INSERT INTO dbo.EntornosFormativos (Nombre, Tipo, Profesor, Anio, Division, Grupo, Observaciones)
    VALUES (N'Entorno huerta principal', N'Entorno huerta', N'Prof. Gómez', N'2025', N'3°', N'A', N'Huerta didáctica');
END
GO


-------------------------------------------
-- 3) Tabla: PRODUCTOS (Inventario/Ventas)
-------------------------------------------
IF OBJECT_ID('dbo.Productos','U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos
    (
        IdProducto     INT IDENTITY(1,1) PRIMARY KEY,
        NombreProducto NVARCHAR(150) NOT NULL,
        PrecioUnitario DECIMAL(10,2) NOT NULL CONSTRAINT DF_Prod_Precio DEFAULT(0),
        Stock          INT           NOT NULL CONSTRAINT DF_Prod_Stock  DEFAULT(0),
        Unidad         NVARCHAR(30)  NULL
    );
END
GO

-- Vista de compatibilidad para grillas (alias)
IF OBJECT_ID('dbo.v_ProductosVenta','V') IS NULL
    EXEC('CREATE VIEW dbo.v_ProductosVenta AS
          SELECT IdProducto,
                 NombreProducto AS Nombre,
                 NombreProducto,
                 PrecioUnitario AS PrecioBase,
                 PrecioUnitario,
                 Stock,
                 Unidad
          FROM dbo.Productos;');
GO

-- Semillas de producto
IF NOT EXISTS (SELECT 1 FROM dbo.Productos WHERE NombreProducto = N'Miel 500g')
    INSERT INTO dbo.Productos (NombreProducto, PrecioUnitario, Stock, Unidad) VALUES (N'Miel 500g', 3500, 20, N'unid');
IF NOT EXISTS (SELECT 1 FROM dbo.Productos WHERE NombreProducto = N'Mermelada de durazno 450g')
    INSERT INTO dbo.Productos (NombreProducto, PrecioUnitario, Stock, Unidad) VALUES (N'Mermelada de durazno 450g', 2700, 35, N'unid');
GO


-------------------------------------------
-- 4) Tabla: INVENTARIO (form Inventario)
-------------------------------------------
IF OBJECT_ID('dbo.Inventario','U') IS NULL
BEGIN
    CREATE TABLE dbo.Inventario
    (
        IdItem             INT IDENTITY(1,1) PRIMARY KEY,
        Nombre             NVARCHAR(120) NOT NULL,
        Categoria          NVARCHAR(80)  NOT NULL,
        Unidad             NVARCHAR(30)  NOT NULL CONSTRAINT DF_Inv_Unidad DEFAULT N'unid',
        Stock              INT           NOT NULL CONSTRAINT DF_Inv_Stock DEFAULT 0,
        StockMinimo        INT           NOT NULL CONSTRAINT DF_Inv_StockMin DEFAULT 0,
        CostoUnitario      DECIMAL(10,2) NOT NULL CONSTRAINT DF_Inv_Costo DEFAULT 0,
        Ubicacion          NVARCHAR(120) NOT NULL,
        Observaciones      NVARCHAR(300) NOT NULL,
        FechaActualizacion DATETIME2     NOT NULL CONSTRAINT DF_Inv_Fecha DEFAULT SYSUTCDATETIME()
    );
END
GO
IF COL_LENGTH('dbo.Inventario','Unidad') IS NULL
    ALTER TABLE dbo.Inventario ADD Unidad NVARCHAR(30) NOT NULL CONSTRAINT DF_Inv_Unidad DEFAULT N'unid';
IF COL_LENGTH('dbo.Inventario','Cantidad') IS NULL
    ALTER TABLE dbo.Inventario ADD Cantidad AS (Stock) PERSISTED;
GO

-- Semillas de inventario
IF NOT EXISTS (SELECT 1 FROM dbo.Inventario WHERE Nombre = N'Dulce de leche')
    INSERT INTO dbo.Inventario (Nombre, Categoria, Unidad, Stock, StockMinimo, CostoUnitario, Ubicacion, Observaciones)
    VALUES (N'Dulce de leche', N'Lácteos', N'unid', 0, 0, 0, N'Depósito', N'');
GO


-------------------------------------------
-- 5) Tabla: VENTAS (Registro / Consulta)
-------------------------------------------
IF OBJECT_ID('dbo.Ventas','U') IS NULL
BEGIN
    CREATE TABLE dbo.Ventas
    (
        IdVenta        INT IDENTITY(1,1) PRIMARY KEY,
        Fecha          DATE          NOT NULL,
        Hora           NVARCHAR(10)  NULL,
        Cliente        NVARCHAR(120) NULL,
        Producto       NVARCHAR(150) NOT NULL,
        Cantidad       INT           NOT NULL,
        PrecioUnitario DECIMAL(10,2) NOT NULL,
        Observaciones  NVARCHAR(300) NULL
        -- Total se calcula en vista
    );
END
GO

-- Vista de consulta con Total calculado
IF OBJECT_ID('dbo.v_VentasDetalle','V') IS NULL
    EXEC('CREATE VIEW dbo.v_VentasDetalle AS
          SELECT  IdVenta,
                  Fecha,
                  Hora,
                  Cliente,
                  Producto,
                  Cantidad,
                  PrecioUnitario,
                  CAST(Cantidad * PrecioUnitario AS DECIMAL(12,2)) AS Total,
                  Observaciones
          FROM dbo.Ventas;');
GO


-------------------------------------------------
-- 6) Tabla: ACTIVIDADES (por Entorno formativo)
-------------------------------------------------
IF OBJECT_ID('dbo.Actividades','U') IS NULL
BEGIN
    CREATE TABLE dbo.Actividades
    (
        IdActividad INT IDENTITY(1,1) PRIMARY KEY,
        Id          INT NOT NULL, -- FK a EntornosFormativos.Id
        Nombre      NVARCHAR(120) NULL,
        Fecha       DATE          NULL,
        Hora        NVARCHAR(10)  NULL,
        Descripcion NVARCHAR(400) NULL,
        Responsable NVARCHAR(120) NULL,
        Estado      NVARCHAR(60)  NULL
    );
END
GO

-- (Opcional) índice por Id/Fecha, útil para la consulta
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Act_Entorno_Fecha' AND object_id=OBJECT_ID('dbo.Actividades'))
    CREATE INDEX IX_Act_Entorno_Fecha ON dbo.Actividades(Id, Fecha);
GO

-- Vista de actividades por entorno (para grillas)
IF OBJECT_ID('dbo.v_ActividadesPorEntorno','V') IS NULL
    EXEC('CREATE VIEW dbo.v_ActividadesPorEntorno AS
          SELECT a.IdActividad,
                 e.Nombre       AS Entorno,
                 a.Nombre,
                 a.Fecha,
                 a.Hora,
                 a.Descripcion,
                 a.Responsable,
                 a.Estado
          FROM dbo.Actividades a
          INNER JOIN dbo.EntornosFormativos e ON a.Id = e.Id;');
GO


-------------------------------------------
-- 7) Vista: Usuarios para ABM (grillas)
-------------------------------------------
IF OBJECT_ID('dbo.v_Usuarios_ABM','V') IS NULL
    EXEC('CREATE VIEW dbo.v_Usuarios_ABM AS
          SELECT IdUsuario, Nombre, Apellido, DNI, Email, Telefono, UsuarioLogin,
                 Direccion, Localidad, Provincia, Observaciones,
                 Rol, Estado, Area, PreguntaSeguridad, RespuestaSeguridad
          FROM dbo.Usuarios;');
GO


-------------------------------------------
-- 8) SP de Login (usado por Form1)
-------------------------------------------
IF OBJECT_ID('dbo.sp_Usuarios_Login','P') IS NULL
    EXEC('CREATE PROCEDURE dbo.sp_Usuarios_Login
        @UsuarioLogin NVARCHAR(100),
        @Contrasenia  NVARCHAR(200)
    AS
    BEGIN
        SET NOCOUNT ON;

        SELECT TOP(1)
            IdUsuario,
            UsuarioLogin,
            Rol,
            Estado
        FROM dbo.Usuarios
        WHERE UsuarioLogin = @UsuarioLogin
          AND Contrasenia  = @Contrasenia;
    END');
ELSE
    EXEC('ALTER PROCEDURE dbo.sp_Usuarios_Login
        @UsuarioLogin NVARCHAR(100),
        @Contrasenia  NVARCHAR(200)
    AS
    BEGIN
        SET NOCOUNT ON;

        SELECT TOP(1)
            IdUsuario,
            UsuarioLogin,
            Rol,
            Estado
        FROM dbo.Usuarios
        WHERE UsuarioLogin = @UsuarioLogin
          AND Contrasenia  = @Contrasenia;
    END');
GO


/* =============================
      FIN – Comprobaciones
   ============================= */
-- Chequeos rápidos
SELECT 'Usuarios' AS Tabla, COUNT(*) AS Filas FROM dbo.Usuarios;
SELECT 'Productos' AS Tabla, COUNT(*) AS Filas FROM dbo.Productos;
SELECT 'Inventario' AS Tabla, COUNT(*) AS Filas FROM dbo.Inventario;
SELECT 'Entornos' AS Tabla, COUNT(*) AS Filas FROM dbo.EntornosFormativos;


--<connectionStrings>
--  <add name="AgrariaDb"
--       connectionString="Data Source=localhost\SQLEXPRESS;Initial Catalog=Agraria;Integrated Security=True;TrustServerCertificate=True"
--       providerName="System.Data.SqlClient" />
--</connectionStrings>
--Data Source="nombre del equipo sql";
