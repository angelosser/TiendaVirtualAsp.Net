using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TiendaVirtual.ViewModel;

#nullable disable

namespace TiendaVirtual.Models
{
    public partial class TiendaVirtual20221Context : DbContext
    {
        public TiendaVirtual20221Context()
        {
        }

        public TiendaVirtual20221Context(DbContextOptions<TiendaVirtual20221Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Categorium> Categoria { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Marca> Marcas { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<PedidoDetalle> PedidoDetalles { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Tarjetum> Tarjeta { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=localhost\\SQLExpress; Initial Catalog=TiendaVirtual20221; Integrated Security=SSPI; User ID=sa2;Password=12345678;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Categorium>(entity =>
            {
                entity.Property(e => e.Nombre).HasMaxLength(50);
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Cliente");

                entity.Property(e => e.Apellidos).HasMaxLength(100);

                entity.Property(e => e.Correo).HasMaxLength(50);

                entity.Property(e => e.Dni).HasMaxLength(8);

                entity.Property(e => e.Nombres).HasMaxLength(100);

                entity.Property(e => e.Telefono).HasMaxLength(50);
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.ToTable("Marca");

                entity.Property(e => e.Nombre).HasMaxLength(50);
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.ToTable("Pedido");

                entity.Property(e => e.Estado).HasMaxLength(50);

                entity.Property(e => e.FechaHora).HasColumnType("datetime");

                entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK_Pedido_Cliente");

                entity.HasOne(d => d.IdTarjetaNavigation)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.IdTarjeta)
                    .HasConstraintName("FK_Pedido_Tarjeta");
            });

            modelBuilder.Entity<PedidoDetalle>(entity =>
            {
                entity.ToTable("PedidoDetalle");

                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.SubTotal).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.IdPedidoNavigation)
                    .WithMany(p => p.PedidoDetalles)
                    .HasForeignKey(d => d.IdPedido)
                    .HasConstraintName("FK_PedidoDetalle_Pedido");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.PedidoDetalles)
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("FK_PedidoDetalle_Producto");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("Producto");

                entity.Property(e => e.Nombre).HasMaxLength(100);

                entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdCategoria)
                    .HasConstraintName("FK_Producto_Categoria");

                entity.HasOne(d => d.IdMarcaNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdMarca)
                    .HasConstraintName("FK_Producto_Marca");
            });

            modelBuilder.Entity<Tarjetum>(entity =>
            {
                entity.Property(e => e.Marca).HasMaxLength(50);

                entity.Property(e => e.Numero).HasMaxLength(50);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.Property(e => e.Apellidos).HasMaxLength(50);

                entity.Property(e => e.Contrasena).HasMaxLength(100);

                entity.Property(e => e.Correo).HasMaxLength(50);

                entity.Property(e => e.Dni).HasMaxLength(8);

                entity.Property(e => e.Nombres).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<TiendaVirtual.ViewModel.UsuarioLogin> UsuarioLogin { get; set; }
    }
}
