/* =========================
   1) CREAR BD Y USARLA
   ========================= */
IF DB_ID(N'Agraria') IS NULL
BEGIN
    CREATE DATABASE Agraria;
END
GO
USE Agraria;
GO

/* =========================
   2) TABLA: Usuarios
   ========================= */
IF OBJECT_ID('dbo.Usuarios','U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios
    (
        IdUsuario           INT IDENTITY(1,1) PRIMARY KEY,
        Nombre              NVARCHAR(100) NOT NULL,
        Apellido            NVARCHAR(100) NOT NULL,
        DNI                 NVARCHAR(20)  NULL,
        Email               NVARCHAR(150) NULL,
        Telefono            NVARCHAR(50)  NULL,
        UsuarioLogin        NVARCHAR(100) NOT NULL UNIQUE,
        Contrasenia         NVARCHAR(200) NULL,   -- usada por "Olvidé mi contraseña" y seed
        Direccion           NVARCHAR(200) NULL,
        Localidad           NVARCHAR(100) NULL,
        Provincia           NVARCHAR(100) NULL,
        Observaciones       NVARCHAR(MAX) NULL,
        Rol                 NVARCHAR(50)  NULL,   -- 'Administrador','Docente','Invitado'...
        Estado              NVARCHAR(50)  NULL,   -- 'Activo','Inactivo'
        Area                NVARCHAR(100) NULL,
        PreguntaSeguridad   NVARCHAR(200) NULL,
        RespuestaSeguridad  NVARCHAR(200) NULL,
        FechaAlta           DATETIME2 NOT NULL CONSTRAINT DF_Usuarios_FechaAlta DEFAULT SYSUTCDATETIME()
    );
END
GO

/* Seed: admin (solo si no existe) */
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE UsuarioLogin = N'admin')
BEGIN
    INSERT INTO dbo.Usuarios (Nombre, Apellido, UsuarioLogin, Contrasenia, Rol, Estado)
    VALUES (N'Admin', N'Principal', N'admin', N'1234', N'Administrador', N'Activo');
END
GO

/* =========================
   3) TABLA: EntornosFormativos
   ========================= */
IF OBJECT_ID('dbo.EntornosFormativos','U') IS NULL
BEGIN
    CREATE TABLE dbo.EntornosFormativos
    (
        Id         INT IDENTITY(1,1) PRIMARY KEY,
        Nombre     NVARCHAR(120) NOT NULL,
        Tipo       NVARCHAR(80)  NOT NULL,
        Profesor   NVARCHAR(120) NOT NULL,
        Anio       NVARCHAR(20)  NOT NULL,
        Division   NVARCHAR(20)  NOT NULL,
        Grupo      NVARCHAR(40)  NOT NULL,
        Observaciones NVARCHAR(300) NULL
    );

    INSERT INTO dbo.EntornosFormativos (Nombre, Tipo, Profesor, Anio, Division, Grupo)
    VALUES
    (N'Huerta', N'Productivo', N'Prof. García', N'2025', N'5ºA', N'1'),
    (N'Laboratorio', N'Tecnológico', N'Prof. López',  N'2025', N'5ºB', N'2');
END
GO

/* =========================
   4) TABLA: Actividades
   ========================= */
IF OBJECT_ID('dbo.Actividades','U') IS NULL
BEGIN
    CREATE TABLE dbo.Actividades
    (
        IdActividad INT IDENTITY(1,1) PRIMARY KEY,
        Nombre      NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(300) NULL,
        Responsable NVARCHAR(100) NULL,
        Fecha       DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END
GO

/* =========================
   5) TABLA: Inventario
   ========================= */
IF OBJECT_ID('dbo.Inventario','U') IS NULL
BEGIN
    CREATE TABLE dbo.Inventario
    (
        IdItem             INT IDENTITY(1,1) PRIMARY KEY,
        Nombre             NVARCHAR(120) NOT NULL,
        Categoria          NVARCHAR(80)  NULL,
        Unidad             NVARCHAR(30)  NULL CONSTRAINT DF_Inv_Unidad DEFAULT N'unid',
        Stock              INT           NOT NULL CONSTRAINT DF_Inv_Stock DEFAULT 0,
        StockMinimo        INT           NOT NULL CONSTRAINT DF_Inv_StockMin DEFAULT 0,
        CostoUnitario      DECIMAL(10,2) NOT NULL CONSTRAINT DF_Inv_Costo DEFAULT 0,
        Ubicacion          NVARCHAR(120) NULL,
        Observaciones      NVARCHAR(300) NULL,
        FechaActualizacion DATETIME2     NOT NULL CONSTRAINT DF_Inv_Fecha DEFAULT SYSUTCDATETIME()
    );
END
GO

/* =========================
   6) TABLA: Productos (para cargar combos)
   ========================= */
IF OBJECT_ID('dbo.Productos','U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos
    (
        IdProducto     INT IDENTITY(1,1) PRIMARY KEY,
        NombreProducto NVARCHAR(150) NOT NULL UNIQUE,
        PrecioUnitario DECIMAL(10,2) NOT NULL CONSTRAINT DF_Prod_Precio DEFAULT 0
    );

    INSERT INTO dbo.Productos (NombreProducto, PrecioUnitario)
    VALUES (N'Maceta 10L', 1200),
           (N'Planta ornamental', 3500),
           (N'Fertilizante 2kg', 2500),
           (N'Semillas de albahaca', 300);
END
GO

/* =========================
   7) TABLA: Ventas (coincide con tu Form de Registro_de_una_Venta)
   ========================= */
IF OBJECT_ID('dbo.Ventas','U') IS NULL
BEGIN
    CREATE TABLE dbo.Ventas
    (
        IdVenta        INT IDENTITY(1,1) PRIMARY KEY,
        Fecha          DATE NOT NULL CONSTRAINT DF_Ventas_Fecha DEFAULT (CONVERT(date, SYSUTCDATETIME())),
        Hora           NVARCHAR(10) NULL, -- tu form guarda texto
        Cliente        NVARCHAR(150) NULL,
        Producto       NVARCHAR(150) NOT NULL,  -- se guarda el nombre elegido del combo
        Cantidad       INT NOT NULL,
        PrecioUnitario DECIMAL(10,2) NOT NULL,
        Total          DECIMAL(10,2) NOT NULL,
        Observaciones  NVARCHAR(300) NULL
    );
END
GO

/* =========================
   8) SP de Login (solo usuario, sin validar pass)
   ========================= */
IF OBJECT_ID('dbo.sp_Usuarios_Login','P') IS NULL
    EXEC ('CREATE PROCEDURE dbo.sp_Usuarios_Login AS SET NOCOUNT ON;');
GO
ALTER PROCEDURE dbo.sp_Usuarios_Login
    @UsuarioLogin NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        u.IdUsuario,
        u.UsuarioLogin,
        u.Nombre,
        u.Apellido,
        u.Rol,
        u.Estado,
        u.Area
    FROM dbo.Usuarios u
    WHERE u.UsuarioLogin = @UsuarioLogin;
END
GO
USE Agraria;
GO

-- (Opcional) Mirá qué columnas tiene hoy:
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'EntornosFormativos'
ORDER BY ORDINAL_POSITION;
GO

-- Agregá las columnas que falten (con valores por defecto para no romper NOT NULL)
IF COL_LENGTH('dbo.EntornosFormativos','Tipo') IS NULL
    ALTER TABLE dbo.EntornosFormativos ADD Tipo NVARCHAR(80) NOT NULL 
        CONSTRAINT DF_Ent_Tipo DEFAULT N'Productivo';

IF COL_LENGTH('dbo.EntornosFormativos','Profesor') IS NULL
    ALTER TABLE dbo.EntornosFormativos ADD Profesor NVARCHAR(120) NOT NULL 
        CONSTRAINT DF_Ent_Profesor DEFAULT N'Desconocido';

IF COL_LENGTH('dbo.EntornosFormativos','Anio') IS NULL
    ALTER TABLE dbo.EntornosFormativos ADD Anio NVARCHAR(20) NOT NULL 
        CONSTRAINT DF_Ent_Anio DEFAULT N'2025';

IF COL_LENGTH('dbo.EntornosFormativos','Division') IS NULL
    ALTER TABLE dbo.EntornosFormativos ADD Division NVARCHAR(20) NOT NULL 
        CONSTRAINT DF_Ent_Division DEFAULT N'1ºA';

IF COL_LENGTH('dbo.EntornosFormativos','Grupo') IS NULL
    ALTER TABLE dbo.EntornosFormativos ADD Grupo NVARCHAR(40) NOT NULL 
        CONSTRAINT DF_Ent_Grupo DEFAULT N'1';

-- (Opcional) también Observaciones, si querés usarla desde el alta
IF COL_LENGTH('dbo.EntornosFormativos','Observaciones') IS NULL
    ALTER TABLE dbo.EntornosFormativos ADD Observaciones NVARCHAR(300) NULL;
GO
ALTER PROCEDURE dbo.sp_Usuarios_Login
    @UsuarioLogin NVARCHAR(100),
    @Contrasenia NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        u.IdUsuario,
        u.UsuarioLogin,
        u.Nombre,
        u.Apellido,
        u.Rol,
        u.Estado,
        u.Area
    FROM dbo.Usuarios u
    WHERE u.UsuarioLogin = @UsuarioLogin
      AND u.Contrasenia = @Contrasenia;
END
GO
EXEC dbo.sp_Usuarios_Login @UsuarioLogin='admin', @Contrasenia='1234';


USE Agraria;
GO

-- 1) Crear tabla estándar si NO existe
IF OBJECT_ID('dbo.Ventas','U') IS NULL
BEGIN
    CREATE TABLE dbo.Ventas
    (
        IdVenta        INT IDENTITY(1,1) PRIMARY KEY,
        Fecha          DATETIME2 NOT NULL CONSTRAINT DF_Ventas_Fecha DEFAULT SYSUTCDATETIME(),
        Producto       NVARCHAR(120) NOT NULL,
        Cantidad       INT NOT NULL CONSTRAINT DF_Ventas_Cantidad DEFAULT(0),
        PrecioUnitario DECIMAL(10,2) NOT NULL CONSTRAINT DF_Ventas_Precio DEFAULT(0),
        Observaciones  NVARCHAR(300) NULL
    );
END
GO

-- 2) Si la tabla existe pero le faltan columnas, agregarlas
IF COL_LENGTH('dbo.Ventas','Cantidad') IS NULL
    ALTER TABLE dbo.Ventas ADD Cantidad INT NOT NULL CONSTRAINT DF_Ventas_Cantidad DEFAULT(0);
IF COL_LENGTH('dbo.Ventas','PrecioUnitario') IS NULL
    ALTER TABLE dbo.Ventas ADD PrecioUnitario DECIMAL(10,2) NOT NULL CONSTRAINT DF_Ventas_Precio DEFAULT(0);
IF COL_LENGTH('dbo.Ventas','Producto') IS NULL
    ALTER TABLE dbo.Ventas ADD Producto NVARCHAR(120) NOT NULL CONSTRAINT DF_Ventas_Producto DEFAULT(N'Desconocido');
IF COL_LENGTH('dbo.Ventas','Observaciones') IS NULL
    ALTER TABLE dbo.Ventas ADD Observaciones NVARCHAR(300) NULL;
GO

-- 3) (Opcional) Mapear datos desde nombres alternativos, si existen
IF COL_LENGTH('dbo.Ventas','CantidadVendida') IS NOT NULL
    UPDATE V SET V.Cantidad = V.CantidadVendida
    FROM dbo.Ventas AS V
    WHERE (V.Cantidad = 0 OR V.Cantidad IS NULL);

IF COL_LENGTH('dbo.Ventas','Precio') IS NOT NULL
    UPDATE V SET V.PrecioUnitario = V.Precio
    FROM dbo.Ventas AS V
    WHERE (V.PrecioUnitario = 0 OR V.PrecioUnitario IS NULL);

-- 4) Crear/asegurar la columna calculada Total
--    Si ya existe como calculada, se deja; si no existe, se crea.
IF COL_LENGTH('dbo.Ventas','Total') IS NULL
    ALTER TABLE dbo.Ventas ADD Total AS (Cantidad * PrecioUnitario) PERSISTED;
GO
USE Agraria;
GO

-- 1) Crear tabla estándar si no existe
IF OBJECT_ID('dbo.Ventas','U') IS NULL
BEGIN
    CREATE TABLE dbo.Ventas
    (
        IdVenta        INT IDENTITY(1,1) PRIMARY KEY,
        Fecha          DATETIME2 NOT NULL CONSTRAINT DF_Ventas_Fecha DEFAULT SYSUTCDATETIME(),
        Producto       NVARCHAR(120) NOT NULL,
        Cantidad       INT NOT NULL CONSTRAINT DF_Ventas_Cantidad DEFAULT(0),
        PrecioUnitario DECIMAL(10,2) NOT NULL CONSTRAINT DF_Ventas_Precio DEFAULT(0),
        Observaciones  NVARCHAR(300) NULL
    );
END;
GO

-- 2) Asegurar columnas requeridas
IF COL_LENGTH('dbo.Ventas','Producto') IS NULL
    ALTER TABLE dbo.Ventas ADD Producto NVARCHAR(120) NOT NULL CONSTRAINT DF_Ventas_Producto DEFAULT(N'Desconocido');

IF COL_LENGTH('dbo.Ventas','Cantidad') IS NULL
    ALTER TABLE dbo.Ventas ADD Cantidad INT NOT NULL CONSTRAINT DF_Ventas_Cantidad DEFAULT(0);

IF COL_LENGTH('dbo.Ventas','PrecioUnitario') IS NULL
    ALTER TABLE dbo.Ventas ADD PrecioUnitario DECIMAL(10,2) NOT NULL CONSTRAINT DF_Ventas_Precio DEFAULT(0);

IF COL_LENGTH('dbo.Ventas','Observaciones') IS NULL
    ALTER TABLE dbo.Ventas ADD Observaciones NVARCHAR(300) NULL;

-- 3) Crear columna calculada Total si falta
IF COL_LENGTH('dbo.Ventas','Total') IS NULL
    ALTER TABLE dbo.Ventas ADD Total AS (Cantidad * PrecioUnitario) PERSISTED;
GO

-- 4) (Opcional) Mapear desde nombres alternativos usando SQL dinámico
IF COL_LENGTH('dbo.Ventas','CantidadVendida') IS NOT NULL
    EXEC('UPDATE V SET V.Cantidad = V.CantidadVendida
          FROM dbo.Ventas AS V
          WHERE (V.Cantidad = 0 OR V.Cantidad IS NULL);');

IF COL_LENGTH('dbo.Ventas','Precio') IS NOT NULL
    EXEC('UPDATE V SET V.PrecioUnitario = V.Precio
          FROM dbo.Ventas AS V
          WHERE (V.PrecioUnitario = 0 OR V.PrecioUnitario IS NULL);');
GO

-- 5) (Opcional) Sembrar datos si está vacía
IF NOT EXISTS (SELECT 1 FROM dbo.Ventas)
BEGIN
    INSERT INTO dbo.Ventas (Producto, Cantidad, PrecioUnitario, Observaciones)
    VALUES 
    (N'Maceta 10L', 20, 1200, N'Venta local'),
    (N'Planta ornamental', 15, 3500, N'Cliente externo'),
    (N'Fertilizante 2kg', 10, 2500, N''),
    (N'Semillas de albahaca', 30, 300, N'Promo primavera');
END;
GO
USE Agraria;
GO

-- Por si acaso: la tabla (tu código ya la crea, pero lo dejo aquí por completitud)
IF OBJECT_ID('dbo.Inventario','U') IS NULL
BEGIN
    CREATE TABLE dbo.Inventario
    (
        IdItem             INT IDENTITY(1,1) PRIMARY KEY,
        Nombre             NVARCHAR(120) NOT NULL,
        Categoria          NVARCHAR(80)  NULL,
        Unidad             NVARCHAR(30)  NULL CONSTRAINT DF_Inv_Unidad DEFAULT N'unid',
        Stock              INT           NOT NULL CONSTRAINT DF_Inv_Stock DEFAULT 0,
        StockMinimo        INT           NOT NULL CONSTRAINT DF_Inv_StockMin DEFAULT 0,
        CostoUnitario      DECIMAL(10,2) NOT NULL CONSTRAINT DF_Inv_Costo DEFAULT 0,
        Ubicacion          NVARCHAR(120) NULL,
        Observaciones      NVARCHAR(300) NULL,
        FechaActualizacion DATETIME2     NOT NULL CONSTRAINT DF_Inv_Fecha DEFAULT SYSUTCDATETIME()
    );
END;
GO

USE Agraria;
GO

-- Aseguro la tabla (por si aún no existe)
IF OBJECT_ID('dbo.Inventario','U') IS NULL
BEGIN
    CREATE TABLE dbo.Inventario
    (
        IdItem             INT IDENTITY(1,1) PRIMARY KEY,
        Nombre             NVARCHAR(120) NOT NULL,
        Categoria          NVARCHAR(80)  NULL,
        Unidad             NVARCHAR(30)  NULL CONSTRAINT DF_Inv_Unidad DEFAULT N'unid',
        Stock              INT           NOT NULL CONSTRAINT DF_Inv_Stock DEFAULT 0,
        StockMinimo        INT           NOT NULL CONSTRAINT DF_Inv_StockMin DEFAULT 0,
        CostoUnitario      DECIMAL(10,2) NOT NULL CONSTRAINT DF_Inv_Costo DEFAULT 0,
        Ubicacion          NVARCHAR(120) NULL,
        Observaciones      NVARCHAR(300) NULL,
        FechaActualizacion DATETIME2     NOT NULL CONSTRAINT DF_Inv_Fecha DEFAULT SYSUTCDATETIME()
    );
END;
GO

DECLARE @now DATETIME2 = SYSUTCDATETIME();

DECLARE @items TABLE(
    Nombre     NVARCHAR(120),
    Categoria  NVARCHAR(80),
    Unidad     NVARCHAR(30),
    StockMin   INT,
    Obs        NVARCHAR(300)
);

INSERT INTO @items (Nombre, Categoria, Unidad, StockMin, Obs) VALUES
-- LÁCTEOS / CONSERVAS DULCES
(N'Dulce de leche',                 N'Lácteos',       N'frasco',  0, N''),
(N'Queso semiduro',                 N'Lácteos',       N'kg',      0, N''),
(N'Salmuera',                       N'Lácteos',       N'lt',      0, N'Para queso'),
(N'Mermelada de manzana',           N'Mermeladas',    N'frasco',  0, N''),
(N'Mermelada de naranja con miel',  N'Mermeladas',    N'frasco',  0, N''),
(N'Mermelada de durazno',           N'Mermeladas',    N'frasco',  0, N''),
(N'Miel',                           N'Apicultura',    N'kg',      0, N''),

-- FRUTAS / INSUMOS RELACIONADOS
(N'Manzana',                        N'Frutas',        N'kg',      0, N''),
(N'Naranja',                        N'Frutas',        N'kg',      0, N''),
(N'Durazno',                        N'Frutas',        N'kg',      0, N''),
(N'Durazno en lata',                N'Conservas',     N'lat',     0, N''),
(N'Frasco',                         N'Insumos',       N'unid',    10, N'Envase 360 cm³ aprox.'),

-- ESCABECHE / CHACINADOS / PANIFICADOS / PASTAS
(N'Escabeche de pollo',             N'Conservas',     N'frasco',  0, N''),
(N'Escabeche de berenjena',         N'Conservas',     N'frasco',  0, N''),
(N'Salame',                         N'Chacinados',    N'kg',      0, N''),
(N'Chorizo',                        N'Chacinados',    N'kg',      0, N''),
(N'Pan',                            N'Panificados',   N'kg',      0, N''),
(N'Fideo verde',                    N'Pastas',        N'kg',      0, N'Con espinaca'),

-- PRODUCCIÓN ANIMAL
(N'Porcinos',                       N'Producción animal', N'anim', 0, N'Pie de cría / engorde'),
(N'Pollos parrilleros',             N'Producción animal', N'anim', 0, N''),
(N'Gallinas ponedoras',             N'Producción animal', N'anim', 0, N''),
(N'Ovejas',                         N'Producción animal', N'anim', 0, N''),

-- AGRÍCOLA / SEMILLAS
(N'Ciruela',                        N'Frutas',        N'kg',      0, N''),
(N'Pera',                           N'Frutas',        N'kg',      0, N''),
(N'Semillas',                       N'Agroinsumos',   N'kg',      0, N'Especifique variedad en Observaciones'),
(N'Cereal',                         N'Agroinsumos',   N'kg',      0, N''),
(N'Girasol',                        N'Agroinsumos',   N'kg',      0, N'Semilla / grano'),
(N'Lotus tenuis',                   N'Agroinsumos',   N'kg',      0, N'Semilla forrajera'),
(N'Cunicultura',                    N'Producción animal', N'anim', 0, N'Conejos (pie de cría)');

INSERT INTO dbo.Inventario
    (Nombre, Categoria, Unidad, Stock, StockMinimo, CostoUnitario, Ubicacion, Observaciones, FechaActualizacion)
SELECT i.Nombre, i.Categoria, i.Unidad, 0, i.StockMin, 0, NULL, i.Obs, @now
FROM @items i
WHERE NOT EXISTS (SELECT 1 FROM dbo.Inventario x WHERE x.Nombre = i.Nombre);
GO
SELECT * FROM dbo.Inventario ORDER BY Categoria, Nombre;
ALTER TABLE dbo.Ventas
ADD Hora TIME NULL;
UPDATE dbo.Ventas
SET Hora = CAST(Fecha AS TIME)
WHERE Hora IS NULL;
DROP TABLE IF EXISTS dbo.EntornosFormativos;

CREATE TABLE dbo.EntornosFormativos
(
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    Nombre    NVARCHAR(120) NOT NULL,
    Tipo      NVARCHAR(80)  NOT NULL,
    Profesor  NVARCHAR(120) NOT NULL,
    Anio      NVARCHAR(20)  NOT NULL,
    Division  NVARCHAR(20)  NOT NULL,
    Grupo     NVARCHAR(40)  NOT NULL
);
/* ========== Ajustes en dbo.EntornosFormativos SIN dropear ni tocar FKs ========== */

IF OBJECT_ID('dbo.EntornosFormativos','U') IS NULL
BEGIN
    -- Si no existiera, la creamos con el esquema que usa el form
    CREATE TABLE dbo.EntornosFormativos
    (
        Id        INT IDENTITY(1,1) PRIMARY KEY,
        Nombre    NVARCHAR(120) NOT NULL,
        Tipo      NVARCHAR(80)  NOT NULL,
        Profesor  NVARCHAR(120) NOT NULL,
        Anio      NVARCHAR(20)  NOT NULL,
        Division  NVARCHAR(20)  NOT NULL,
        Grupo     NVARCHAR(40)  NOT NULL
    );
END
ELSE
BEGIN
    /* ---- 1) Columna Id (alias de IdEntorno si existe) ---- */
    IF COL_LENGTH('dbo.EntornosFormativos','Id') IS NULL
    BEGIN
        -- Caso A: existe IdEntorno (viejo) → creo Id como columna calculada compatible
        IF COL_LENGTH('dbo.EntornosFormativos','IdEntorno') IS NOT NULL
        BEGIN
            ALTER TABLE dbo.EntornosFormativos
            ADD Id AS (CONVERT(INT, IdEntorno)) PERSISTED;
        END
        ELSE
        BEGIN
            -- Caso B: no existe IdEntorno ni Id → agrego Id físico (no toco PK ni FKs)
            ALTER TABLE dbo.EntornosFormativos
            ADD Id INT NULL;

            -- Si querés garantizar valores, podés rellenar secuenciales:
            ;WITH s AS (
                SELECT Id, ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS rn
                FROM dbo.EntornosFormativos
            )
            UPDATE s SET Id = rn;

            -- Y evitar nulos futuros:
            ALTER TABLE dbo.EntornosFormativos
            ADD CONSTRAINT DF_EF_Id DEFAULT(NULL) FOR Id; -- dejas controlado por app
        END
    END

    /* ---- 2) Agregar columnas faltantes con defaults seguros ---- */
    IF COL_LENGTH('dbo.EntornosFormativos','Nombre') IS NULL
        ALTER TABLE dbo.EntornosFormativos
        ADD Nombre NVARCHAR(120) NOT NULL CONSTRAINT DF_EF_Nombre DEFAULT N'Sin nombre';

    IF COL_LENGTH('dbo.EntornosFormativos','Tipo') IS NULL
        ALTER TABLE dbo.EntornosFormativos
        ADD Tipo NVARCHAR(80) NOT NULL CONSTRAINT DF_EF_Tipo DEFAULT N'General';

    IF COL_LENGTH('dbo.EntornosFormativos','Profesor') IS NULL
        ALTER TABLE dbo.EntornosFormativos
        ADD Profesor NVARCHAR(120) NOT NULL CONSTRAINT DF_EF_Profesor DEFAULT N'Sin asignar';

    IF COL_LENGTH('dbo.EntornosFormativos','Anio') IS NULL
        ALTER TABLE dbo.EntornosFormativos
        ADD Anio NVARCHAR(20) NOT NULL CONSTRAINT DF_EF_Anio DEFAULT N'2025';

    IF COL_LENGTH('dbo.EntornosFormativos','Division') IS NULL
        ALTER TABLE dbo.EntornosFormativos
        ADD Division NVARCHAR(20) NOT NULL CONSTRAINT DF_EF_Division DEFAULT N'';

    IF COL_LENGTH('dbo.EntornosFormativos','Grupo') IS NULL
        ALTER TABLE dbo.EntornosFormativos
        ADD Grupo NVARCHAR(40) NOT NULL CONSTRAINT DF_EF_Grupo DEFAULT N'';

    /* ---- 3) Backfill por si quedaron NULLs preexistentes ---- */
    UPDATE dbo.EntornosFormativos
    SET
        Nombre   = ISNULL(Nombre,   N'Sin nombre'),
        Tipo     = ISNULL(Tipo,     N'General'),
        Profesor = ISNULL(Profesor, N'Sin asignar'),
        Anio     = ISNULL(Anio,     N'2025'),
        Division = ISNULL(Division, N''),
        Grupo    = ISNULL(Grupo,    N'');
END;
GO
-- Agregar columna calculada Id que aliasée a IdEntorno (sin borrar nada)
ALTER TABLE dbo.EntornosFormativos
ADD Id AS (CONVERT(INT, IdEntorno)) PERSISTED;
GO
