using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Abrigo> Abrigos { get; set; }
    public DbSet<Adocao> Adocoes { get; set; }
    public DbSet<Endereco> Enderecos { get; set;}
    public DbSet<Pet> Pets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=MeuAbrigo.db");
    }

}