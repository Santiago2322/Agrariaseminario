/* =========================================================
   AGRARIA – Esquema completo (idempotente)
   Compatible con tus formularios WinForms
   ========================================================= */

----------------------------------------
-- 0) Crear DB si no existe y usarla
----------------------------------------
IF DB_ID(N'Agraria') IS NULL
BEGIN
    CREATE DATABASE Agraria;
END
GO
USE Agraria;
GO

/* =========================================================
   1) USUARIOS
   ========================================================= */
IF OBJECT_ID('dbo.Usuarios','U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios
    (
        IdUsuario           INT IDENTITY(1,1) CONSTRAINT PK_Usuarios PRIMARY KEY,
        Nombre              NVARCHAR(100) NULL,
        Apellido            NVARCHAR(100) NULL,
        DNI                 NVARCHAR(20)  NULL,
        Email               NVARCHAR(150) NULL,
        Telefono            NVARCHAR(50)  NULL,
        UsuarioLogin        NVARCHAR(100) NOT NULL,
        Contrasenia         NVARCHAR(200) NULL,
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
ELSE
BEGIN
    -- Normalización suave: asegurar columnas mínimas usadas por el proyecto
    IF COL_LENGTH('dbo.Usuarios','UsuarioLogin') IS NULL
        ALTER TABLE dbo.Usuarios ADD UsuarioLogin NVARCHAR(100) NOT NULL CONSTRAINT DF_Usuarios_UsuarioLogin DEFAULT N'';
    IF COL_LENGTH('dbo.Usuarios','Contrasenia') IS NULL
        ALTER TABLE dbo.Usuarios ADD Contrasenia NVARCHAR(200) NULL;
    IF COL_LENGTH('dbo.Usuarios','Rol') IS NULL
        ALTER TABLE dbo.Usuarios ADD Rol NVARCHAR(50) NOT NULL CONSTRAINT DF_Usuarios_Rol DEFAULT N'Invitado';
    IF COL_LENGTH('dbo.Usuarios','Estado') IS NULL
        ALTER TABLE dbo.Usuarios ADD Estado NVARCHAR(20) NULL CONSTRAINT DF_Usuarios_Estado DEFAULT N'Activo';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='UX_Usuarios_UsuarioLogin' AND object_id=OBJECT_ID('dbo.Usuarios'))
    CREATE UNIQUE INDEX UX_Usuarios_UsuarioLogin ON dbo.Usuarios(UsuarioLogin);
GO

-- Semillas (solo si no existen)
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

/* =========================================================
   2) ENTORNOS FORMATIVOS
   - PK: IdEntorno
   - Columna calculada Id para compatibilidad con código viejo
   ========================================================= */
IF OBJECT_ID('dbo.EntornosFormativos','U') IS NULL
BEGIN
    CREATE TABLE dbo.EntornosFormativos
    (
        IdEntorno      INT IDENTITY(1,1) CONSTRAINT PK_Entornos PRIMARY KEY,
        Nombre         NVARCHAR(120) NOT NULL,
        Tipo           NVARCHAR(80)  NOT NULL,
        Profesor       NVARCHAR(120) NULL,
        Anio           NVARCHAR(20)  NULL,
        Division       NVARCHAR(20)  NULL,
        Grupo          NVARCHAR(40)  NULL,
        Observaciones  NVARCHAR(300) NULL,
        FechaAlta      DATETIME2(7)  NOT NULL CONSTRAINT DF_Ent_FechaAlta DEFAULT SYSUTCDATETIME()
    );
END
ELSE
BEGIN
    IF COL_LENGTH('dbo.EntornosFormativos','IdEntorno') IS NULL
        ALTER TABLE dbo.EntornosFormativos ADD IdEntorno INT IDENTITY(1,1);
    IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name='PK_Entornos' AND parent_object_id=OBJECT_ID('dbo.EntornosFormativos'))
        ALTER TABLE dbo.EntornosFormativos ADD CONSTRAINT PK_Entornos PRIMARY KEY (IdEntorno);
    IF COL_LENGTH('dbo.EntornosFormativos','Nombre') IS NULL
        ALTER TABLE dbo.EntornosFormativos ADD Nombre NVARCHAR(120) NOT NULL CONSTRAINT DF_Ent_Nombre DEFAULT N'';
    IF COL_LENGTH('dbo.EntornosFormativos','Tipo') IS NULL
        ALTER TABLE dbo.EntornosFormativos ADD Tipo NVARCHAR(80) NOT NULL CONSTRAINT DF_Ent_Tipo DEFAULT N'';
    IF COL_LENGTH('dbo.EntornosFormativos','Profesor') IS NULL
        ALTER TABLE dbo.EntornosFormativos ADD Profesor NVARCHAR(120) NULL;
    IF COL_LENGTH('dbo.EntornosFormativos','Anio') IS NULL
        ALTER TABLE dbo.EntornosFormativos ADD Anio NVARCHAR(20) NULL;
    IF COL_LENGTH('dbo.EntornosFormativos','Division') IS NULL
        ALTER TABLE dbo.EntornosFormativos ADD Division NVARCHAR(20) NULL;
    IF COL_LENGTH('dbo.EntornosFormativos','Grupo') IS NULL
        ALTER TABLE dbo.EntornosFormativos ADD Grupo NVARCHAR(40) NULL;
    IF COL_LENGTH('dbo.EntornosFormativos','Observaciones') IS NULL
        ALTER TABLE dbo.EntornosFormativos ADD Observaciones NVARCHAR(300) NULL;
    IF COL_LENGTH('dbo.EntornosFormativos','FechaAlta') IS NULL
        ALTER TABLE dbo.EntornosFormativos ADD FechaAlta DATETIME2(7) NOT NULL CONSTRAINT DF_Ent_FechaAlta DEFAULT SYSUTCDATETIME();
END
GO

-- Columna calculada Id para compatibilidad con código que usa Id
IF COL_LENGTH('dbo.EntornosFormativos','Id') IS NULL
    ALTER TABLE dbo.EntornosFormativos ADD Id AS (IdEntorno);
GO

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

/* =========================================================
   3) ACTIVIDADES (por entorno) – tabla principal para tus grillas
   ========================================================= */
IF OBJECT_ID('dbo.Actividades','U') IS NULL
BEGIN
    CREATE TABLE dbo.Actividades
    (
        IdActividad INT IDENTITY(1,1) CONSTRAINT PK_Actividades PRIMARY KEY,
        Id          INT NOT NULL,  -- FK a EntornosFormativos(IdEntorno)
        Nombre      NVARCHAR(120) NULL,
        Fecha       DATE          NULL,
        Hora        NVARCHAR(10)  NULL,
        Descripcion NVARCHAR(400) NULL,
        Responsable NVARCHAR(120) NULL,
        Estado      NVARCHAR(60)  NULL
    );
END
ELSE
BEGIN
    IF COL_LENGTH('dbo.Actividades','IdActividad') IS NULL
        ALTER TABLE dbo.Actividades ADD IdActividad INT IDENTITY(1,1);
    IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name='PK_Actividades' AND parent_object_id=OBJECT_ID('dbo.Actividades'))
        ALTER TABLE dbo.Actividades ADD CONSTRAINT PK_Actividades PRIMARY KEY (IdActividad);
    IF COL_LENGTH('dbo.Actividades','Id') IS NULL
        ALTER TABLE dbo.Actividades ADD Id INT NOT NULL CONSTRAINT DF_Act_Id DEFAULT(0);
    IF COL_LENGTH('dbo.Actividades','Nombre') IS NULL
        ALTER TABLE dbo.Actividades ADD Nombre NVARCHAR(120) NULL;
    IF COL_LENGTH('dbo.Actividades','Fecha') IS NULL
        ALTER TABLE dbo.Actividades ADD Fecha DATE NULL;
    IF COL_LENGTH('dbo.Actividades','Hora') IS NULL
        ALTER TABLE dbo.Actividades ADD Hora NVARCHAR(10) NULL;
    IF COL_LENGTH('dbo.Actividades','Descripcion') IS NULL
        ALTER TABLE dbo.Actividades ADD Descripcion NVARCHAR(400) NULL;
    IF COL_LENGTH('dbo.Actividades','Responsable') IS NULL
        ALTER TABLE dbo.Actividades ADD Responsable NVARCHAR(120) NULL;
    IF COL_LENGTH('dbo.Actividades','Estado') IS NULL
        ALTER TABLE dbo.Actividades ADD Estado NVARCHAR(60) NULL;
END
GO

-- FK a EntornosFormativos.IdEntorno (cascada en delete/update)
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_Act_Entorno' AND parent_object_id=OBJECT_ID('dbo.Actividades'))
    ALTER TABLE dbo.Actividades DROP CONSTRAINT FK_Act_Entorno;
GO
ALTER TABLE dbo.Actividades WITH CHECK
ADD CONSTRAINT FK_Act_Entorno FOREIGN KEY (Id) REFERENCES dbo.EntornosFormativos(IdEntorno)
    ON UPDATE CASCADE ON DELETE CASCADE;
GO

-- Índices útiles
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Act_Entorno_Fecha' AND object_id=OBJECT_ID('dbo.Actividades'))
    CREATE INDEX IX_Act_Entorno_Fecha ON dbo.Actividades(Id, Fecha);
GO

-- Semilla mínima para ver datos en la grilla
IF NOT EXISTS (SELECT 1 FROM dbo.Actividades)
BEGIN
    DECLARE @eid INT = (SELECT TOP 1 IdEntorno FROM dbo.EntornosFormativos ORDER BY IdEntorno);
    IF @eid IS NOT NULL
    BEGIN
        INSERT INTO dbo.Actividades (Id, Nombre, Fecha, Hora, Descripcion, Responsable, Estado)
        VALUES (@eid, N'Siembra de lechuga', CONVERT(date,GETDATE()), N'08:00', N'Actividad inicial', N'Equipo A', N'Pendiente');
    END
END
GO

/* =========================================================
   4) ACTIVIDAD (singular) – bitácora opcional (no usada por tus grillas)
   ========================================================= */
IF OBJECT_ID('dbo.Actividad','U') IS NULL
BEGIN
    CREATE TABLE dbo.Actividad
    (
        IdActividad    INT IDENTITY(1,1) CONSTRAINT PK_Actividad PRIMARY KEY,
        IdUsuario      INT NOT NULL,
        IdEntorno      INT NULL,
        FechaHora      DATETIME2(7) NOT NULL CONSTRAINT DF_Actividad_FechaHora DEFAULT SYSUTCDATETIME(),
        Tipo           NVARCHAR(80) NULL,
        Descripcion    NVARCHAR(400) NOT NULL CONSTRAINT DF_Actividad_Desc DEFAULT N'',
        Observaciones  NVARCHAR(400) NOT NULL CONSTRAINT DF_Actividad_Obs DEFAULT N'',
        IpEquipo       NVARCHAR(64)  NULL
    );
END
ELSE
BEGIN
    IF COL_LENGTH('dbo.Actividad','IdActividad') IS NULL
        ALTER TABLE dbo.Actividad ADD IdActividad INT IDENTITY(1,1);
    IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name='PK_Actividad' AND parent_object_id=OBJECT_ID('dbo.Actividad'))
        ALTER TABLE dbo.Actividad ADD CONSTRAINT PK_Actividad PRIMARY KEY (IdActividad);
END
GO
-- FKs (silenciosos si ya existen)
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_Actividad_Usuario')
BEGIN
    ALTER TABLE dbo.Actividad WITH CHECK
    ADD CONSTRAINT FK_Actividad_Usuario FOREIGN KEY(IdUsuario) REFERENCES dbo.Usuarios(IdUsuario);
END
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_Actividad_Entorno')
BEGIN
    ALTER TABLE dbo.Actividad WITH CHECK
    ADD CONSTRAINT FK_Actividad_Entorno FOREIGN KEY(IdEntorno) REFERENCES dbo.EntornosFormativos(IdEntorno) ON UPDATE CASCADE ON DELETE SET NULL;
END
GO

/* =========================================================
   5) INVENTARIO
   ========================================================= */
IF OBJECT_ID('dbo.Inventario','U') IS NULL
BEGIN
    CREATE TABLE dbo.Inventario
    (
        IdItem             INT IDENTITY(1,1) CONSTRAINT PK_Inventario PRIMARY KEY,
        Nombre             NVARCHAR(120) NOT NULL,
        Categoria          NVARCHAR(80)  NOT NULL,
        Unidad             NVARCHAR(30)  NOT NULL CONSTRAINT DF_Inv_Unidad DEFAULT N'unid',
        Stock              INT           NOT NULL CONSTRAINT DF_Inv_Stock DEFAULT 0,
        StockMinimo        INT           NOT NULL CONSTRAINT DF_Inv_StockMin DEFAULT 0,
        CostoUnitario      DECIMAL(10,2) NOT NULL CONSTRAINT DF_Inv_Costo DEFAULT 0,
        Ubicacion          NVARCHAR(120) NOT NULL,
        Observaciones      NVARCHAR(300) NOT NULL CONSTRAINT DF_Inv_Obs DEFAULT N'',
        FechaActualizacion DATETIME2     NOT NULL CONSTRAINT DF_Inv_Fecha DEFAULT SYSUTCDATETIME()
    );
END
ELSE
BEGIN
    IF COL_LENGTH('dbo.Inventario','Unidad') IS NULL
        ALTER TABLE dbo.Inventario ADD Unidad NVARCHAR(30) NOT NULL CONSTRAINT DF_Inv_Unidad DEFAULT N'unid';
    IF COL_LENGTH('dbo.Inventario','StockMinimo') IS NULL
        ALTER TABLE dbo.Inventario ADD StockMinimo INT NOT NULL CONSTRAINT DF_Inv_StockMin DEFAULT 0;
    IF COL_LENGTH('dbo.Inventario','CostoUnitario') IS NULL
        ALTER TABLE dbo.Inventario ADD CostoUnitario DECIMAL(10,2) NOT NULL CONSTRAINT DF_Inv_Costo DEFAULT 0;
END
GO

IF COL_LENGTH('dbo.Inventario','Cantidad') IS NULL
    ALTER TABLE dbo.Inventario ADD Cantidad AS (Stock) PERSISTED;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Inv_Categoria_Nombre' AND object_id=OBJECT_ID('dbo.Inventario'))
    CREATE INDEX IX_Inv_Categoria_Nombre ON dbo.Inventario(Categoria, Nombre);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Inv_Nombre' AND object_id=OBJECT_ID('dbo.Inventario'))
    CREATE INDEX IX_Inv_Nombre ON dbo.Inventario(Nombre);
GO

-- 30 ítems semilla (idempotente)
;WITH items(Nombre, Categoria, Unidad, Stock, StockMin, Costo, Ubic, Obs) AS
(
    -- Lácteos / Industria
    SELECT N'Dulce de leche',            N'Lácteos',    N'unid', 10, 2, 2500.00, N'Industria', N'Lote inicial' UNION ALL
    SELECT N'Queso semiduro',            N'Lácteos',    N'kg',    5,  1, 4200.00, N'Industria', N'Piezas curadas' UNION ALL
    SELECT N'Queso cremoso',             N'Lácteos',    N'kg',    7,  2, 3900.00, N'Industria', N'Fresco' UNION ALL
    SELECT N'Yogur natural',             N'Lácteos',    N'unid', 24, 6,  700.00, N'Industria', N'Vasos 190g' UNION ALL
    SELECT N'Manteca',                   N'Lácteos',    N'kg',    4,  1, 5200.00, N'Industria', N'Tablas 1kg' UNION ALL

    -- Conservas / Mermeladas
    SELECT N'Mermelada de manzana',      N'Conservas',  N'unid', 20, 5, 1800.00, N'Industria', N'Frascos 450g' UNION ALL
    SELECT N'Mermelada de durazno',      N'Conservas',  N'unid', 18, 5, 1850.00, N'Industria', N'Frascos 450g' UNION ALL
    SELECT N'Mermelada de naranja/miel', N'Conservas',  N'unid', 15, 5, 2200.00, N'Industria', N'Lote otoño' UNION ALL
    SELECT N'Conserva de tomate',        N'Conservas',  N'unid', 30, 8, 1600.00, N'Industria', N'Botella 500ml' UNION ALL
    SELECT N'Pepinos en vinagre',        N'Conservas',  N'unid', 12, 3, 1900.00, N'Industria', N'Frascos 600g' UNION ALL

    -- Apicultura
    SELECT N'Miel',                      N'Apicultura', N'kg',    8,  2, 3500.00, N'Apicultura', N'Pura' UNION ALL
    SELECT N'Panales vacíos',            N'Apicultura', N'unid', 25, 5,  900.00, N'Apicultura', N'Envío reciente' UNION ALL
    SELECT N'Ahumador',                  N'Apicultura', N'unid',  3,  1, 9800.00, N'Apicultura', N'Inoxidable' UNION ALL
    SELECT N'Traje apícola',             N'Apicultura', N'unid',  6,  2, 32000.00, N'Apicultura', N'Talles varios' UNION ALL
    SELECT N'Escobillas apícolas',       N'Apicultura', N'unid', 15, 5,  700.00,  N'Apicultura', N'Limpieza' UNION ALL

    -- Vivero / Huerta
    SELECT N'Semillas de lechuga',       N'Vivero',     N'unid', 50, 10, 300.00,  N'Vivero', N'Sobres' UNION ALL
    SELECT N'Semillas de tomate',        N'Vivero',     N'unid', 60, 10, 350.00,  N'Vivero', N'Sobres' UNION ALL
    SELECT N'Semillas de zanahoria',     N'Vivero',     N'unid', 40, 10, 300.00,  N'Vivero', N'Sobres' UNION ALL
    SELECT N'Bandejas de almácigos',     N'Vivero',     N'unid', 30, 10, 900.00,  N'Vivero', N'Plástico' UNION ALL
    SELECT N'Sustrato para almácigos',   N'Vivero',     N'kg',   40, 10, 650.00,  N'Vivero', N'Mezcla estándar' UNION ALL
    SELECT N'Plantines de lechuga',      N'Vivero',     N'unid', 80, 20, 250.00,  N'Vivero', N'Maceta 6cm' UNION ALL
    SELECT N'Plantines de tomate',       N'Vivero',     N'unid', 75, 20, 280.00,  N'Vivero', N'Maceta 6cm' UNION ALL
    SELECT N'Tutores de caña',           N'Vivero',     N'unid', 90, 30, 180.00,  N'Vivero', N'1,5 m' UNION ALL
    SELECT N'Macetas 2L',                N'Vivero',     N'unid', 45, 10, 700.00,  N'Vivero', N'Plástico negro' UNION ALL
    SELECT N'Fertilizante NPK 15-15-15', N'Vivero',     N'kg',   25, 5,  2100.00, N'Vivero', N'Granulado' UNION ALL

    -- Pañol / Herramientas
    SELECT N'Guantes de nitrilo',        N'Pañol',      N'unid', 100, 20, 120.00, N'Pañol', N'Tallas S/M/L' UNION ALL
    SELECT N'Palas anchas',              N'Pañol',      N'unid', 10,  2, 6800.00, N'Pañol', N'Mango madera' UNION ALL
    SELECT N'Rastrillos',                N'Pañol',      N'unid', 12,  2, 5900.00, N'Pañol', N'Jardín' UNION ALL
    SELECT N'Tijeras de podar',          N'Pañol',      N'unid', 14,  4, 8200.00, N'Pañol', N'Bypass' UNION ALL
    SELECT N'Carretillas',               N'Pañol',      N'unid',  6,  1, 49000.00,N'Pañol', N'Rueda maciza' UNION ALL

    -- Insumos generales
    SELECT N'Film alimentario',          N'Industria',  N'unid', 20,  5, 1500.00, N'Almacén', N'Rollo 30cm x 300m' UNION ALL
    SELECT N'Frascos 450g con tapa',     N'Industria',  N'unid', 200, 50, 380.00, N'Almacén', N'Vidrio + tapa'
)
INSERT INTO dbo.Inventario (Nombre, Categoria, Unidad, Stock, StockMinimo, CostoUnitario, Ubicacion, Observaciones)
SELECT i.Nombre, i.Categoria, i.Unidad, i.Stock, i.StockMin, i.Costo, i.Ubic, i.Obs
FROM items i
WHERE NOT EXISTS (SELECT 1 FROM dbo.Inventario x WHERE x.Nombre = i.Nombre);
GO

/* =========================================================
   6) PRODUCTOS (para ventas) + VENTAS
   ========================================================= */
IF OBJECT_ID('dbo.Productos','U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos
    (
        IdProducto     INT IDENTITY(1,1) CONSTRAINT PK_Productos PRIMARY KEY,
        NombreProducto NVARCHAR(150) NOT NULL,
        PrecioUnitario DECIMAL(10,2) NOT NULL CONSTRAINT DF_Prod_Precio DEFAULT(0),
        Stock          INT           NOT NULL CONSTRAINT DF_Prod_Stock  DEFAULT(0),
        Unidad         NVARCHAR(30)  NULL
    );
END
GO
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
IF NOT EXISTS (SELECT 1 FROM dbo.Productos WHERE NombreProducto = N'Miel 500g')
    INSERT INTO dbo.Productos (NombreProducto, PrecioUnitario, Stock, Unidad) VALUES (N'Miel 500g', 3500, 20, N'unid');
IF NOT EXISTS (SELECT 1 FROM dbo.Productos WHERE NombreProducto = N'Mermelada de durazno 450g')
    INSERT INTO dbo.Productos (NombreProducto, PrecioUnitario, Stock, Unidad) VALUES (N'Mermelada de durazno 450g', 2700, 35, N'unid');
GO

IF OBJECT_ID('dbo.Ventas','U') IS NULL
BEGIN
    CREATE TABLE dbo.Ventas
    (
        IdVenta        INT IDENTITY(1,1) CONSTRAINT PK_Ventas PRIMARY KEY,
        Fecha          DATE          NOT NULL CONSTRAINT DF_Ventas_Fecha DEFAULT (CONVERT(date,GETDATE())),
        Hora           NVARCHAR(10)  NULL,
        Cliente        NVARCHAR(120) NULL,
        Producto       NVARCHAR(150) NOT NULL,
        Cantidad       INT           NOT NULL,
        PrecioUnitario DECIMAL(10,2) NOT NULL,
        Observaciones  NVARCHAR(300) NULL
    );
END
GO
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

/* =========================================================
   7) VISTAS para formularios
   ========================================================= */
-- v_Usuarios_ABM (grillas)
IF OBJECT_ID('dbo.v_Usuarios_ABM','V') IS NULL
    EXEC('CREATE VIEW dbo.v_Usuarios_ABM AS
          SELECT IdUsuario, Nombre, Apellido, DNI, Email, Telefono, UsuarioLogin,
                 Direccion, Localidad, Provincia, Observaciones,
                 Rol, Estado, Area, PreguntaSeguridad, RespuestaSeguridad, FechaAlta
          FROM dbo.Usuarios;');
GO

-- v_ActividadesPorEntorno (usa Actividades + EntornosFormativos)
IF OBJECT_ID('dbo.v_ActividadesPorEntorno','V') IS NOT NULL
    DROP VIEW dbo.v_ActividadesPorEntorno;
GO
CREATE VIEW dbo.v_ActividadesPorEntorno
AS
    SELECT  a.IdActividad,
            e.Nombre       AS Entorno,
            a.Nombre,
            a.Fecha,
            a.Hora,
            a.Descripcion,
            a.Responsable,
            a.Estado
    FROM dbo.Actividades a
    INNER JOIN dbo.EntornosFormativos e
        ON a.Id = e.IdEntorno;
GO

/* =========================================================
   8) Procedimientos – Login
   ========================================================= */
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

/* =========================================================
   9) Chequeos rápidos
   ========================================================= */
SELECT 'Usuarios' AS Tabla, COUNT(*) AS Filas FROM dbo.Usuarios;
SELECT 'EntornosFormativos' AS Tabla, COUNT(*) AS Filas FROM dbo.EntornosFormativos;
SELECT 'Actividades' AS Tabla, COUNT(*) AS Filas FROM dbo.Actividades;
SELECT 'Inventario' AS Tabla, COUNT(*) AS Filas FROM dbo.Inventario;
SELECT 'Productos' AS Tabla, COUNT(*) AS Filas FROM dbo.Productos;
SELECT 'Ventas' AS Tabla, COUNT(*) AS Filas FROM dbo.Ventas;

PRINT 'OK: AGRARIA lista.';
