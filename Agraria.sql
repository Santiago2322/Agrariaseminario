/* =========================================================
   AGRARIA – ESQUEMA BASE OPTIMIZADO PARA EXPORTAR
   Compatible con SQL Server Express / SSMS
   Idempotente: se puede ejecutar varias veces sin romper nada
   ========================================================= */

------------------------------------------------------------
-- 0) Crear BD si no existe + configuración básica
------------------------------------------------------------
IF DB_ID(N'Agraria') IS NULL
BEGIN
    CREATE DATABASE Agraria;
END
GO
ALTER DATABASE Agraria SET RECOVERY SIMPLE;
GO
USE Agraria;
GO

------------------------------------------------------------
-- 1) Tabla: USUARIOS  (alineado a formularios)
------------------------------------------------------------
IF OBJECT_ID('dbo.Usuarios','U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios
    (
        IdUsuario           INT IDENTITY(1,1) PRIMARY KEY,
        Nombre              NVARCHAR(80)   NULL,
        Apellido            NVARCHAR(80)   NULL,
        DNI                 NVARCHAR(12)   NULL,
        Email               NVARCHAR(120)  NULL,
        Telefono            NVARCHAR(40)   NULL,
        UsuarioLogin        NVARCHAR(60)   NOT NULL,
        Contrasenia         NVARCHAR(200)  NULL,
        Direccion           NVARCHAR(150)  NULL,
        Localidad           NVARCHAR(80)   NULL,
        Provincia           NVARCHAR(80)   NULL,
        Observaciones       NVARCHAR(300)  NULL,
        Rol                 NVARCHAR(40)   NULL,
        Estado              NVARCHAR(20)   NULL CONSTRAINT DF_Usuarios_Estado DEFAULT (N'Activo'),
        Area                NVARCHAR(60)   NULL,
        PreguntaSeguridad   NVARCHAR(200)  NULL,
        RespuestaSeguridad  NVARCHAR(200)  NULL
    );
END
ELSE
BEGIN
    -- Asegurar columnas que espera la app
    IF COL_LENGTH('dbo.Usuarios','Nombre')             IS NULL ALTER TABLE dbo.Usuarios ADD Nombre NVARCHAR(80)   NULL;
    IF COL_LENGTH('dbo.Usuarios','Apellido')           IS NULL ALTER TABLE dbo.Usuarios ADD Apellido NVARCHAR(80)  NULL;
    IF COL_LENGTH('dbo.Usuarios','DNI')                IS NULL ALTER TABLE dbo.Usuarios ADD DNI NVARCHAR(12)       NULL;
    IF COL_LENGTH('dbo.Usuarios','Email')              IS NULL ALTER TABLE dbo.Usuarios ADD Email NVARCHAR(120)    NULL;
    IF COL_LENGTH('dbo.Usuarios','Telefono')           IS NULL ALTER TABLE dbo.Usuarios ADD Telefono NVARCHAR(40)  NULL;
    IF COL_LENGTH('dbo.Usuarios','UsuarioLogin')       IS NULL ALTER TABLE dbo.Usuarios ADD UsuarioLogin NVARCHAR(60) NOT NULL CONSTRAINT DF_Usuarios_Login DEFAULT(N'pendiente');
    IF COL_LENGTH('dbo.Usuarios','Contrasenia')        IS NULL ALTER TABLE dbo.Usuarios ADD Contrasenia NVARCHAR(200) NULL;
    IF COL_LENGTH('dbo.Usuarios','Direccion')          IS NULL ALTER TABLE dbo.Usuarios ADD Direccion NVARCHAR(150) NULL;
    IF COL_LENGTH('dbo.Usuarios','Localidad')          IS NULL ALTER TABLE dbo.Usuarios ADD Localidad NVARCHAR(80)  NULL;
    IF COL_LENGTH('dbo.Usuarios','Provincia')          IS NULL ALTER TABLE dbo.Usuarios ADD Provincia NVARCHAR(80)  NULL;
    IF COL_LENGTH('dbo.Usuarios','Observaciones')      IS NULL ALTER TABLE dbo.Usuarios ADD Observaciones NVARCHAR(300) NULL;
    IF COL_LENGTH('dbo.Usuarios','Rol')                IS NULL ALTER TABLE dbo.Usuarios ADD Rol NVARCHAR(40)       NULL;
    IF COL_LENGTH('dbo.Usuarios','Estado')             IS NULL ALTER TABLE dbo.Usuarios ADD Estado NVARCHAR(20)     NULL CONSTRAINT DF_Usuarios_Estado DEFAULT (N'Activo');
    IF COL_LENGTH('dbo.Usuarios','Area')               IS NULL ALTER TABLE dbo.Usuarios ADD Area NVARCHAR(60)       NULL;
    IF COL_LENGTH('dbo.Usuarios','PreguntaSeguridad')  IS NULL ALTER TABLE dbo.Usuarios ADD PreguntaSeguridad NVARCHAR(200) NULL;
    IF COL_LENGTH('dbo.Usuarios','RespuestaSeguridad') IS NULL ALTER TABLE dbo.Usuarios ADD RespuestaSeguridad NVARCHAR(200) NULL;
END
GO

-- Índices
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='UX_Usuarios_Login' AND object_id=OBJECT_ID('dbo.Usuarios'))
    CREATE UNIQUE INDEX UX_Usuarios_Login ON dbo.Usuarios(UsuarioLogin);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Usuarios_Rol_Estado' AND object_id=OBJECT_ID('dbo.Usuarios'))
    CREATE INDEX IX_Usuarios_Rol_Estado ON dbo.Usuarios(Rol, Estado);
GO

-- Semilla admin (solo si tabla vacía)
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios)
    INSERT INTO dbo.Usuarios(Nombre,Apellido,UsuarioLogin,Contrasenia,Rol,Estado,Area,Email)
    VALUES (N'Administrador',N'Sistema',N'admin',N'admin',N'admin',N'Activo',N'Administración',N'admin@demo');
GO

-- SP de login que usa tu Form1  (devuelve al menos Rol y Estado)
CREATE OR ALTER PROCEDURE dbo.sp_Usuarios_Login
    @UsuarioLogin NVARCHAR(60),
    @Contrasenia  NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP(1)
           u.IdUsuario, u.UsuarioLogin, u.Rol, u.Estado,
           u.Nombre, u.Apellido, u.Email
    FROM dbo.Usuarios u
    WHERE u.UsuarioLogin = @UsuarioLogin
      AND (u.Contrasenia = @Contrasenia OR @Contrasenia IS NULL); -- permite null si luego cambiás a hash + verificación externa
END
GO

------------------------------------------------------------
-- 2) Tabla: ENTORNOS FORMATIVOS
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
ELSE
BEGIN
    -- Renombrar IdEntorno -> Id si hiciera falta
    IF COL_LENGTH('dbo.EntornosFormativos','Id') IS NULL
       AND COL_LENGTH('dbo.EntornosFormativos','IdEntorno') IS NOT NULL
       EXEC sp_rename 'dbo.EntornosFormativos.IdEntorno','Id','COLUMN';

    IF COL_LENGTH('dbo.EntornosFormativos','Observaciones') IS NULL
        ALTER TABLE dbo.EntornosFormativos ADD Observaciones NVARCHAR(300) NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Entornos_Nombre_Anio' AND object_id=OBJECT_ID('dbo.EntornosFormativos'))
    CREATE INDEX IX_Entornos_Nombre_Anio ON dbo.EntornosFormativos(Nombre,Anio,Division,Grupo);
GO

-- Semilla idempotente
MERGE dbo.EntornosFormativos AS T
USING (VALUES
 (N'Agraria: Entorno industria (dulces y conservas, lácteo)',  N'Productivo', N'', N'', N'', N'', N'Áreas: dulces/conservas y lácteo'),
 (N'Entorno huerta',                          N'Productivo', N'', N'', N'', N'', N''),
 (N'Entorno monte frutal',                    N'Productivo', N'', N'', N'', N'', N''),
 (N'Entorno monte frutal: donaciones',        N'Productivo', N'', N'', N'', N'', N'Donaciones'),
 (N'Entorno vivero',                          N'Productivo', N'', N'', N'', N'', N''),
 (N'Entorno pañol',                           N'Tecnológico',N'', N'', N'', N'', N'Herramental'),
 (N'Entorno cunicultura',                     N'Productivo', N'', N'', N'', N'', N'Conejos'),
 (N'Entorno animal: cunicultura, porcino, apicultura, parrillero, ponedora (sectores)', N'Productivo', N'', N'', N'', N'', N'Sectores divididos')
) AS S(Nombre,Tipo,Profesor,Anio,Division,Grupo,Observaciones)
ON T.Nombre = S.Nombre
WHEN NOT MATCHED BY TARGET THEN
  INSERT (Nombre,Tipo,Profesor,Anio,Division,Grupo,Observaciones)
  VALUES (S.Nombre,S.Tipo,S.Profesor,S.Anio,S.Division,S.Grupo,S.Observaciones);
GO

------------------------------------------------------------
-- 3) INVENTARIO
------------------------------------------------------------
IF OBJECT_ID('dbo.Inventario','U') IS NULL
BEGIN
    CREATE TABLE dbo.Inventario
    (
        IdItem             INT IDENTITY(1,1) PRIMARY KEY,
        Nombre             NVARCHAR(120) NOT NULL,
        Categoria          NVARCHAR(80)  NULL,
        Cantidad           DECIMAL(18,2) NOT NULL CONSTRAINT DF_Inv_Cantidad DEFAULT(0),
        CostoUnitario      DECIMAL(10,2) NOT NULL CONSTRAINT DF_Inv_Costo DEFAULT(0),
        Ubicacion          NVARCHAR(120) NULL,
        Observaciones      NVARCHAR(300) NULL,
        FechaActualizacion DATETIME2      NOT NULL CONSTRAINT DF_Inv_Fecha DEFAULT SYSUTCDATETIME()
    );
END
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='UX_Inventario_Nombre' AND object_id=OBJECT_ID('dbo.Inventario'))
    CREATE UNIQUE INDEX UX_Inventario_Nombre ON dbo.Inventario(Nombre);
GO

------------------------------------------------------------
-- 4) PRODUCTOS
------------------------------------------------------------
IF OBJECT_ID('dbo.Productos','U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos
    (
        IdProducto INT IDENTITY(1,1) PRIMARY KEY,
        Nombre     NVARCHAR(120) NOT NULL,
        PrecioBase DECIMAL(10,2) NOT NULL CONSTRAINT DF_Prod_Precio DEFAULT(0),
        Stock      DECIMAL(18,2) NOT NULL CONSTRAINT DF_Prod_Stock  DEFAULT(0),
        Unidad     NVARCHAR(20)  NULL           CONSTRAINT DF_Prod_Unidad DEFAULT N'unid',
        Observaciones NVARCHAR(300) NULL
    );
END
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='UX_Productos_Nombre' AND object_id=OBJECT_ID('dbo.Productos'))
    CREATE UNIQUE INDEX UX_Productos_Nombre ON dbo.Productos(Nombre);
GO

------------------------------------------------------------
-- 5) CLIENTES
------------------------------------------------------------
IF OBJECT_ID('dbo.Clientes','U') IS NULL
BEGIN
    CREATE TABLE dbo.Clientes
    (
        IdCliente INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(120) NOT NULL,
        Telefono NVARCHAR(40) NULL,
        Email NVARCHAR(120) NULL
    );
END
GO

------------------------------------------------------------
-- 6) ACTIVIDADES  (FK a EntornosFormativos.Id)
------------------------------------------------------------
IF OBJECT_ID('dbo.Actividades','U') IS NULL
BEGIN
    CREATE TABLE dbo.Actividades
    (
        IdActividad INT IDENTITY(1,1) PRIMARY KEY,
        Id          INT NOT NULL,  -- FK a EntornosFormativos.Id
        Fecha       DATE NOT NULL CONSTRAINT DF_Act_Fecha DEFAULT (CONVERT(date,GETDATE())),
        Hora        NVARCHAR(10) NULL,
        Descripcion NVARCHAR(300) NULL,
        Responsable NVARCHAR(120) NULL,
        Estado      NVARCHAR(50) NULL
    );
END
GO

-- FK con cascada (crea si faltara)
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_Act_Entorno' AND parent_object_id=OBJECT_ID('dbo.Actividades'))
BEGIN
    BEGIN TRY
        ALTER TABLE dbo.Actividades WITH CHECK
        ADD CONSTRAINT FK_Act_Entorno FOREIGN KEY(Id) REFERENCES dbo.EntornosFormativos(Id)
            ON UPDATE CASCADE ON DELETE CASCADE;
    END TRY
    BEGIN CATCH
        PRINT 'Aviso: revise datos existentes para crear FK.';
    END CATCH
END
GO

------------------------------------------------------------
-- 7) VENTAS (Total calculado)
------------------------------------------------------------
IF OBJECT_ID('dbo.Ventas','U') IS NULL
BEGIN
    CREATE TABLE dbo.Ventas
    (
        IdVenta        INT IDENTITY(1,1) PRIMARY KEY,
        Fecha          DATETIME2 NOT NULL CONSTRAINT DF_Ventas_Fecha DEFAULT SYSUTCDATETIME(),
        IdCliente      INT NULL,
        IdProducto     INT NULL,
        Cantidad       DECIMAL(18,2) NOT NULL,
        PrecioUnitario DECIMAL(10,2) NOT NULL,
        Total          AS (Cantidad * PrecioUnitario) PERSISTED,
        Observaciones  NVARCHAR(300) NULL
    );
END
GO

-- FKs (si faltaran)
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_Ventas_Cliente' AND parent_object_id=OBJECT_ID('dbo.Ventas'))
    ALTER TABLE dbo.Ventas ADD CONSTRAINT FK_Ventas_Cliente  FOREIGN KEY(IdCliente)  REFERENCES dbo.Clientes(IdCliente);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_Ventas_Producto' AND parent_object_id=OBJECT_ID('dbo.Ventas'))
    ALTER TABLE dbo.Ventas ADD CONSTRAINT FK_Ventas_Producto FOREIGN KEY(IdProducto) REFERENCES dbo.Productos(IdProducto);
GO

------------------------------------------------------------
-- 8) Vistas (para formularios)
------------------------------------------------------------
CREATE OR ALTER VIEW dbo.v_ActividadesPorEntorno
AS
    SELECT a.IdActividad, e.Nombre AS Entorno, a.Fecha, a.Hora,
           a.Descripcion, a.Responsable, a.Estado
    FROM dbo.Actividades a
    INNER JOIN dbo.EntornosFormativos e ON a.Id = e.Id;
GO

CREATE OR ALTER VIEW dbo.v_ProductosVenta
AS
    SELECT p.IdProducto, p.Nombre, p.PrecioBase AS PrecioUnitario,
           p.Stock, p.Unidad
    FROM dbo.Productos p;
GO

------------------------------------------------------------
-- 9) Semillas mínimas (opcionales / idempotentes)
------------------------------------------------------------
-- Actividad de ejemplo si hay “huerta”
INSERT INTO dbo.Actividades (Id, Fecha, Hora, Descripcion, Responsable, Estado)
SELECT TOP 1 e.Id, CONVERT(date,GETDATE()), N'08:00', N'Siembra de lechuga', N'Prof. Gómez', N'Programada'
FROM dbo.EntornosFormativos e
WHERE e.Nombre LIKE N'%huerta%'
  AND NOT EXISTS (SELECT 1 FROM dbo.Actividades x WHERE x.Descripcion=N'Siembra de lechuga');

-- Inventario básico
DECLARE @now DATETIME2 = SYSUTCDATETIME();
;WITH items (Nombre, Categoria, Cantidad, Costo, Obs) AS (
    SELECT N'Dulce de leche', N'Lácteos', 0, 0, N'' UNION ALL
    SELECT N'Queso semiduro', N'Lácteos', 0, 0, N'' UNION ALL
    SELECT N'Salmuera', N'Conservas', 0, 0, N'Para queso' UNION ALL
    SELECT N'Mermelada de manzana', N'Conservas', 0, 0, N'' UNION ALL
    SELECT N'Mermelada de naranja con miel', N'Conservas', 0, 0, N'' UNION ALL
    SELECT N'Mermelada de durazno', N'Conservas', 0, 0, N'' UNION ALL
    SELECT N'Miel', N'Apicultura', 0, 0, N''
)
INSERT INTO dbo.Inventario(Nombre, Categoria, Cantidad, CostoUnitario, Ubicacion, Observaciones, FechaActualizacion)
SELECT i.Nombre, i.Categoria, i.Cantidad, i.Costo, NULL, i.Obs, @now
FROM items i
WHERE NOT EXISTS (SELECT 1 FROM dbo.Inventario x WHERE x.Nombre=i.Nombre);
GO

/* ---------------------------------------------------------
   10) (Opcional) BACKUP para exportar .bak (ajustá la ruta)
--------------------------------------------------------- 
-- BACKUP DATABASE Agraria
-- TO DISK = 'C:\Backups\Agraria.bak'
-- WITH FORMAT, INIT, NAME = 'Backup Agraria';
*/
