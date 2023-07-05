using Microsoft.EntityFrameworkCore;
using TechChallangeApi.Models;

public class ApplicationContext : DbContext
{     
        public ApplicationContext(DbContextOptions<ApplicationContext> opts) : base(opts) { }
        public DbSet<Foto> Fotos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Publicacao> Publicacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
		//usuario
		modelBuilder.Entity<Usuario>()
			  .HasKey(x => x.Id);

		//publicacao 
		modelBuilder.Entity<Publicacao>()
			  .HasKey(x => x.Id);

		//modelBuilder.Entity<Publicacao>()
		//   .HasOne(x => x.Usuario)
		//   .WithMany(x => x.Publicacoes)
		//   .HasForeignKey(x => x.UsuarioId);

		modelBuilder.Entity<Publicacao>()
		   .HasOne(x => x.Foto)
		   .WithOne(x => x.Publicacao);
	}

}
