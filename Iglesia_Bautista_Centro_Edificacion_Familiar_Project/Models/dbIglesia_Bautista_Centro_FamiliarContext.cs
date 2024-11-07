using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class dbIglesia_Bautista_Centro_FamiliarContext : DbContext
    {
        public dbIglesia_Bautista_Centro_FamiliarContext()
        {
        }

        public dbIglesia_Bautista_Centro_FamiliarContext(DbContextOptions<dbIglesia_Bautista_Centro_FamiliarContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Almacene> Almacenes { get; set; } = null!;
        public virtual DbSet<AprovadoEntrega> AprovadoEntregas { get; set; } = null!;
        public virtual DbSet<Cargo> Cargos { get; set; } = null!;
        public virtual DbSet<Categoria> Categoria { get; set; } = null!;
        public virtual DbSet<DeVolucion> DeVolucions { get; set; } = null!;
        public virtual DbSet<Entrada> Entrada { get; set; } = null!;
        public virtual DbSet<Inventario> Inventarios { get; set; } = null!;
        public virtual DbSet<Marca> Marcas { get; set; } = null!;
        public virtual DbSet<ModeloMarca> ModeloMarcas { get; set; } = null!;
        public virtual DbSet<Perfil> Perfils { get; set; } = null!;
        public virtual DbSet<Permiso> Permisos { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<Responsable> Responsables { get; set; } = null!;
        public virtual DbSet<Salida> Salida { get; set; } = null!;
        public virtual DbSet<Solicitante> Solicitantes { get; set; } = null!;
        public virtual DbSet<SolicitudPrestamo> SolicitudPrestamos { get; set; } = null!;
        public virtual DbSet<TipoEntrada> TipoEntrada { get; set; } = null!;
        public virtual DbSet<TipoProducto> TipoProductos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Almacene>(entity =>
            {
                entity.HasKey(e => e.IdAlmacenes)
                    .HasName("PK__Almacene__449D609F4A8AE302");

                entity.Property(e => e.IdAlmacenes).HasColumnName("idAlmacenes");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Ubicacion)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ubicacion");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Almacenes)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Almacenes__idUsu__5EBF139D");
            });

            modelBuilder.Entity<AprovadoEntrega>(entity =>
            {
                entity.HasKey(e => e.IdAprovadoEntrega)
                    .HasName("PK__Aprovado__592C54257ADB0E64");

                entity.ToTable("Aprovado_Entrega");

                entity.Property(e => e.IdAprovadoEntrega).HasColumnName("idAprovado_Entrega");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.FechaAprovacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_Aprovacion")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdSolicitante).HasColumnName("idSolicitante");

                entity.Property(e => e.IdSolicitudPrestamo).HasColumnName("idSolicitud_Prestamo");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("observacion");

                entity.HasOne(d => d.IdSolicitanteNavigation)
                    .WithMany(p => p.AprovadoEntregas)
                    .HasForeignKey(d => d.IdSolicitante)
                    .HasConstraintName("FK__Aprovado___idSol__74AE54BC");

                entity.HasOne(d => d.IdSolicitudPrestamoNavigation)
                    .WithMany(p => p.AprovadoEntregas)
                    .HasForeignKey(d => d.IdSolicitudPrestamo)
                    .HasConstraintName("FK__Aprovado___idSol__75A278F5");
            });

            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.HasKey(e => e.IdCargo)
                    .HasName("PK__Cargo__3D0E29B8E34F58E6");

                entity.ToTable("Cargo");

                entity.Property(e => e.IdCargo).HasColumnName("idCargo");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.IdCategoria)
                    .HasName("PK__Categori__8A3D240C7BA146BE");

                entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<DeVolucion>(entity =>
            {
                entity.HasKey(e => e.IdDeVolucion)
                    .HasName("PK__DeVoluci__C675BDC5262010FD");

                entity.ToTable("DeVolucion");

                entity.Property(e => e.IdDeVolucion).HasColumnName("idDeVolucion");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.FechaDevolucion)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_Devolucion")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdInventario).HasColumnName("idInventario");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("observacion");

                entity.HasOne(d => d.IdInventarioNavigation)
                    .WithMany(p => p.DeVolucions)
                    .HasForeignKey(d => d.IdInventario)
                    .HasConstraintName("FK__DeVolucio__idInv__7A672E12");
            });

            modelBuilder.Entity<Entrada>(entity =>
            {
                entity.HasKey(e => e.IdEntrada)
                    .HasName("PK__Entrada__19943CE0718D5E16");

                entity.Property(e => e.IdEntrada).HasColumnName("idEntrada");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.IdProducto).HasColumnName("idProducto");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.Entrada)
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("FK__Entrada__idProdu__7D439ABD");
            });

            modelBuilder.Entity<Inventario>(entity =>
            {
                entity.HasKey(e => e.IdInventario)
                    .HasName("PK__Inventar__8F145B0DD21A2771");

                entity.ToTable("Inventario");

                entity.Property(e => e.IdInventario).HasColumnName("idInventario");

                entity.Property(e => e.FechaInventario)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_Inventario")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdAlmacenes).HasColumnName("idAlmacenes");

                entity.Property(e => e.IdProducto).HasColumnName("idProducto");

                entity.Property(e => e.IdTipoEntrada).HasColumnName("idTipo_Entrada");

                entity.Property(e => e.Statud).HasColumnName("statud");

                entity.Property(e => e.StockMaximo).HasColumnName("stock_Maximo");

                entity.Property(e => e.StockMinimo).HasColumnName("stock_minimo");

                entity.HasOne(d => d.IdAlmacenesNavigation)
                    .WithMany(p => p.Inventarios)
                    .HasForeignKey(d => d.IdAlmacenes)
                    .HasConstraintName("FK__Inventari__idAlm__6D0D32F4");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.Inventarios)
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("FK__Inventari__idPro__6B24EA82");

                entity.HasOne(d => d.IdTipoEntradaNavigation)
                    .WithMany(p => p.Inventarios)
                    .HasForeignKey(d => d.IdTipoEntrada)
                    .HasConstraintName("FK__Inventari__idTip__6C190EBB");
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.HasKey(e => e.IdMarca)
                    .HasName("PK__Marca__7033181214E016A4");

                entity.ToTable("Marca");

                entity.Property(e => e.IdMarca).HasColumnName("idMarca");

                entity.Property(e => e.IdModeloMarca).HasColumnName("idModelo_Marca");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.HasOne(d => d.IdModeloMarcaNavigation)
                    .WithMany(p => p.Marcas)
                    .HasForeignKey(d => d.IdModeloMarca)
                    .HasConstraintName("FK__Marca__idModelo___571DF1D5");
            });

            modelBuilder.Entity<ModeloMarca>(entity =>
            {
                entity.HasKey(e => e.IdModeloMarca)
                    .HasName("PK__Modelo_M__DDA24CF9E3FE922F");

                entity.ToTable("Modelo_Marca");

                entity.Property(e => e.IdModeloMarca).HasColumnName("idModelo_Marca");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.HasKey(e => e.IdPerfil)
                    .HasName("PK__Perfil__40F13B60E0F94520");

                entity.ToTable("Perfil");

                entity.Property(e => e.IdPerfil).HasColumnName("idPerfil");

                entity.Property(e => e.IdPermiso).HasColumnName("idPermiso");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.HasOne(d => d.IdPermisoNavigation)
                    .WithMany(p => p.Perfils)
                    .HasForeignKey(d => d.IdPermiso)
                    .HasConstraintName("FK__Perfil__idPermis__4BAC3F29");
            });

            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.HasKey(e => e.IdPermiso)
                    .HasName("PK__Permiso__06A584866D3F2101");

                entity.ToTable("Permiso");

                entity.Property(e => e.IdPermiso).HasColumnName("idPermiso");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.IdProducto)
                    .HasName("PK__Producto__07F4A132FF5A936B");

                entity.ToTable("Producto");

                entity.Property(e => e.IdProducto).HasColumnName("idProducto");

                entity.Property(e => e.Color)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("color");

                entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");

                entity.Property(e => e.IdMarca).HasColumnName("idMarca");

                entity.Property(e => e.IdTipoProducto).HasColumnName("idTipo_Producto");

                entity.Property(e => e.NombreProducto)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre_Producto");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdCategoria)
                    .HasConstraintName("FK__Producto__idCate__656C112C");

                entity.HasOne(d => d.IdMarcaNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdMarca)
                    .HasConstraintName("FK__Producto__idMarc__6754599E");

                entity.HasOne(d => d.IdTipoProductoNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdTipoProducto)
                    .HasConstraintName("FK__Producto__idTipo__66603565");
            });

            modelBuilder.Entity<Responsable>(entity =>
            {
                entity.HasKey(e => e.IdResponsable)
                    .HasName("PK__Responsa__6D0A5251328C45EF");

                entity.ToTable("Responsable");

                entity.Property(e => e.IdResponsable).HasColumnName("idResponsable");

                entity.Property(e => e.Cedula)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cedula");

                entity.Property(e => e.IdCargo).HasColumnName("idCargo");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.HasOne(d => d.IdCargoNavigation)
                    .WithMany(p => p.Responsables)
                    .HasForeignKey(d => d.IdCargo)
                    .HasConstraintName("FK__Responsab__idCar__5BE2A6F2");
            });

            modelBuilder.Entity<Salida>(entity =>
            {
                entity.HasKey(e => e.IdSalida)
                    .HasName("PK__Salida__BBE6FB5DA3E92919");

                entity.Property(e => e.IdSalida).HasColumnName("idSalida");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.IdProducto).HasColumnName("idProducto");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.Salida)
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("FK__Salida__idProduc__00200768");
            });

            modelBuilder.Entity<Solicitante>(entity =>
            {
                entity.HasKey(e => e.IdSolicitante)
                    .HasName("PK__Solicita__EC4E0BE8313B02B4");

                entity.ToTable("Solicitante");

                entity.Property(e => e.IdSolicitante).HasColumnName("idSolicitante");

                entity.Property(e => e.Apellido)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("apellido");

                entity.Property(e => e.Cedula)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cedula");

                entity.Property(e => e.Correo)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("correo");

                entity.Property(e => e.Dirrecion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("dirrecion");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("telefono");
            });

            modelBuilder.Entity<SolicitudPrestamo>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudPrestamo)
                    .HasName("PK__Solicitu__BCE9E2CE1EE62C75");

                entity.ToTable("Solicitud_Prestamo");

                entity.Property(e => e.IdSolicitudPrestamo).HasColumnName("idSolicitud_Prestamo");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.FechaEntrega)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("fecha_Entrega");

                entity.Property(e => e.FechaOperaciones)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_Operaciones")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdInventario).HasColumnName("idInventario");

                entity.Property(e => e.IdResponsable).HasColumnName("idResponsable");

                entity.Property(e => e.Lugar)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("lugar");

                entity.HasOne(d => d.IdInventarioNavigation)
                    .WithMany(p => p.SolicitudPrestamos)
                    .HasForeignKey(d => d.IdInventario)
                    .HasConstraintName("FK__Solicitud__idInv__70DDC3D8");

                entity.HasOne(d => d.IdResponsableNavigation)
                    .WithMany(p => p.SolicitudPrestamos)
                    .HasForeignKey(d => d.IdResponsable)
                    .HasConstraintName("FK__Solicitud__idRes__71D1E811");
            });

            modelBuilder.Entity<TipoEntrada>(entity =>
            {
                entity.HasKey(e => e.IdTipoEntrada)
                    .HasName("PK__Tipo_Ent__A7B93FE163516A55");

                entity.ToTable("Tipo_Entrada");

                entity.Property(e => e.IdTipoEntrada).HasColumnName("idTipo_Entrada");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<TipoProducto>(entity =>
            {
                entity.HasKey(e => e.IdTipoProducto)
                    .HasName("PK__Tipo_Pro__1E33F0F1FFFD975D");

                entity.ToTable("Tipo_Producto");

                entity.Property(e => e.IdTipoProducto).HasColumnName("idTipo_Producto");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuario__645723A6EFE2EF22");

                entity.ToTable("Usuario");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.ApellidoCompleto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("apellido_Completo");

                entity.Property(e => e.Cedula)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cedula");

                entity.Property(e => e.Contrasena)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("contrasena");

                entity.Property(e => e.Correo)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("correo");

                entity.Property(e => e.IdPerfil).HasColumnName("idPerfil");

                entity.Property(e => e.NombreCompleto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre_Completo");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("telefono");

                entity.HasOne(d => d.IdPerfilNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdPerfil)
                    .HasConstraintName("FK__Usuario__idPerfi__4E88ABD4");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
