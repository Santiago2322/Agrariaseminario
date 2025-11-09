/* =========================================================
   AGRARIA – PATCH: FK Actividades→Entornos + alias IdEntorno
   + índices útiles + semilla Inventario (30 ítems)
   Seguro para correr varias veces
   ========================================================= */
SET NOCOUNT ON;

IF DB_ID(N'Agraria') IS NULL
BEGIN
    PRINT 'Creando base Agraria...';
    CREATE DATABASE Agraria;
END
GO
USE Agraria;
GO

/* ---------- ENTORNOS FORMATIVOS ---------- */
IF OBJECT_ID('dbo.EntornosFormativos','U') IS NULL
BEGIN
    PRINT 'Creando dbo.EntornosFormativos...';
    CREATE TABLE dbo.EntornosFormativos
    (
        Id            INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Entornos PRIMARY KEY,
        Nombre        NVARCHAR(120) NOT NULL,
        Tipo          NVARCHAR(80)  NOT NULL,
        Profesor      NVARCHAR(120) NULL,
        Anio          NVARCHAR(20)  NULL,
        Division      NVARCHAR(20)  NULL,
        Grupo         NVARCHAR(40)  NULL,
        Observaciones NVARCHAR(300) NULL
    );
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.key_constraints 
                   WHERE parent_object_id = OBJECT_ID('dbo.EntornosFormativos')
                     AND name = 'PK_Entornos')
    BEGIN
        /* Si no tiene PK, la agrego sobre Id si existe; si no, la creo */
        IF COL_LENGTH('dbo.EntornosFormativos','Id') IS NULL
            ALTER TABLE dbo.EntornosFormativos ADD Id INT IDENTITY(1,1);
        ALTER TABLE dbo.EntornosFormativos ADD CONSTRAINT PK_Entornos PRIMARY KEY(Id);
    END
END;

/* Alias de compatibilidad: IdEntorno (calculada) si no existe y sí existe Id */
IF COL_LENGTH('dbo.EntornosFormativos','IdEntorno') IS NULL
AND COL_LENGTH('dbo.EntornosFormativos','Id')        IS NOT NULL
BEGIN
    PRINT 'Agregando columna calculada IdEntorno = Id...';
    ALTER TABLE dbo.EntornosFormativos 
    ADD IdEntorno AS (Id) PERSISTED;
END

/* Índice útil para búsquedas */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Entornos_Nombre_Anio' AND object_id=OBJECT_ID('dbo.EntornosFormativos'))
    CREATE INDEX IX_Entornos_Nombre_Anio ON dbo.EntornosFormativos(Nombre, Anio, Division, Grupo);


/* ---------- ACTIVIDADES ---------- */
IF OBJECT_ID('dbo.Actividades','U') IS NULL
BEGIN
    PRINT 'Creando dbo.Actividades...';
    CREATE TABLE dbo.Actividades
    (
        IdActividad INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Actividades PRIMARY KEY,
        Id          INT NOT NULL,  -- FK a EntornosFormativos
        Nombre      NVARCHAR(120) NULL,
        Fecha       DATE NOT NULL CONSTRAINT DF_Act_Fecha DEFAULT (CONVERT(date,GETDATE())),
        Hora        NVARCHAR(10) NULL,
        Descripcion NVARCHAR(300) NULL,
        Responsable NVARCHAR(120) NULL,
        Estado      NVARCHAR(50)  NULL
    );
END
ELSE
BEGIN
    /* Asegurar columnas clave */
    IF COL_LENGTH('dbo.Actividades','IdActividad') IS NULL
    BEGIN
        ALTER TABLE dbo.Actividades ADD IdActividad INT IDENTITY(1,1);
        IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name='PK_Actividades' AND parent_object_id = OBJECT_ID('dbo.Actividades'))
            ALTER TABLE dbo.Actividades ADD CONSTRAINT PK_Actividades PRIMARY KEY(IdActividad);
    END
    IF COL_LENGTH('dbo.Actividades','Id') IS NULL
        ALTER TABLE dbo.Actividades ADD Id INT NOT NULL CONSTRAINT DF_Act_Id DEFAULT (0);
END

/* Índices útiles */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Act_Entorno' AND object_id=OBJECT_ID('dbo.Actividades'))
    CREATE INDEX IX_Act_Entorno ON dbo.Actividades(Id);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Act_Fecha' AND object_id=OBJECT_ID('dbo.Actividades'))
    CREATE INDEX IX_Act_Fecha ON dbo.Actividades(Fecha);

/* FK limpia (re-creación idempotente). 
   Como garantizamos que Entornos tiene IdEntorno (real o calculado) y Actividades usa Id,
   fijamos la FK a EntornosFormativos(IdEntorno). */
DECLARE @fk sysname = N'FK_Actividades_Entornos';
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=@fk AND parent_object_id=OBJECT_ID('dbo.Actividades'))
BEGIN
    PRINT 'Dropping FK existente FK_Actividades_Entornos...';
    ALTER TABLE dbo.Actividades DROP CONSTRAINT FK_Actividades_Entornos;
END

IF COL_LENGTH('dbo.EntornosFormativos','IdEntorno') IS NOT NULL
BEGIN
    PRINT 'Creando FK_Actividades_Entornos (Actividades.Id → EntornosFormativos.IdEntorno)...';
    ALTER TABLE dbo.Actividades WITH CHECK
    ADD CONSTRAINT FK_Actividades_Entornos
    FOREIGN KEY (Id) REFERENCES dbo.EntornosFormativos(IdEntorno)
    ON UPDATE CASCADE ON DELETE CASCADE;
END
ELSE IF COL_LENGTH('dbo.EntornosFormativos','Id') IS NOT NULL
BEGIN
    PRINT 'Creando FK_Actividades_Entornos (fallback a EntornosFormativos.Id)...';
    ALTER TABLE dbo.Actividades WITH CHECK
    ADD CONSTRAINT FK_Actividades_Entornos
    FOREIGN KEY (Id) REFERENCES dbo.EntornosFormativos(Id)
    ON UPDATE CASCADE ON DELETE CASCADE;
END


/* ---------- VISTA v_ActividadesPorEntorno ---------- */
/* Usamos el alias IdEntorno (real o calculado), por lo que la JOIN es estable */
IF OBJECT_ID('dbo.v_ActividadesPorEntorno','V') IS NOT NULL
    DROP VIEW dbo.v_ActividadesPorEntorno;
GO
CREATE VIEW dbo.v_ActividadesPorEntorno
AS
    SELECT 
        a.IdActividad,
        e.Nombre       AS Entorno,
        a.Nombre,
        a.Fecha,
        a.Hora,
        a.Descripcion,
        a.Responsable,
        a.Estado
    FROM dbo.Actividades a
    INNER JOIN dbo.EntornosFormativos e
      ON a.Id = e.IdEntorno;   -- IdEntorno existe (real o calculado)
GO


/* ---------- INVENTARIO (30 ÍTEMS) ---------- */
IF OBJECT_ID('dbo.Inventario','U') IS NULL
BEGIN
    PRINT 'Creando dbo.Inventario...';
    CREATE TABLE dbo.Inventario
    (
        IdItem             INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Inventario PRIMARY KEY,
        Nombre             NVARCHAR(120) NOT NULL,
        Categoria          NVARCHAR(80)  NOT NULL,
        Unidad             NVARCHAR(30)  NOT NULL CONSTRAINT DF_Inv_Unidad DEFAULT (N'unid'),
        Stock              INT           NOT NULL CONSTRAINT DF_Inv_Stock DEFAULT (0),
        StockMinimo        INT           NOT NULL CONSTRAINT DF_Inv_StockMin DEFAULT (0),
        CostoUnitario      DECIMAL(10,2) NOT NULL CONSTRAINT DF_Inv_Costo DEFAULT (0),
        Ubicacion          NVARCHAR(120) NOT NULL,
        Observaciones      NVARCHAR(300) NOT NULL,
        FechaActualizacion DATETIME2     NOT NULL CONSTRAINT DF_Inv_Fecha DEFAULT SYSUTCDATETIME()
    );
END

IF COL_LENGTH('dbo.Inventario','Cantidad') IS NULL
    ALTER TABLE dbo.Inventario ADD Cantidad AS (Stock) PERSISTED;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Inv_Categoria_Nombre' AND object_id=OBJECT_ID('dbo.Inventario'))
    CREATE INDEX IX_Inv_Categoria_Nombre ON dbo.Inventario(Categoria, Nombre);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Inv_Nombre' AND object_id=OBJECT_ID('dbo.Inventario'))
    CREATE INDEX IX_Inv_Nombre ON dbo.Inventario(Nombre);

/* Semilla de 30 ítems (solo agrega los que falten por Nombre) */
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
    SELECT N'Escobillas apícolas',       N'Apicultura', N'unid', 15, 5,  700.00, N'Apicultura', N'Limpieza' UNION ALL

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


/* ---------- VERIFICACIONES RÁPIDAS ---------- */
PRINT '--- Controles rápidos ---';
SELECT 'EntornosFormativos' AS Tabla, COUNT(*) AS Filas FROM dbo.EntornosFormativos;
SELECT 'Actividades'        AS Tabla, COUNT(*) AS Filas FROM dbo.Actividades;
SELECT 'Inventario'         AS Tabla, COUNT(*) AS Filas FROM dbo.Inventario;

PRINT 'OK: Patch aplicado.';
