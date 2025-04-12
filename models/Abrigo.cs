using Microsoft.EntityFrameworkCore.Migrations.Operations;

public class Abrigo {
  public int abrigoID {get; set;}
  public string nome {get; set;}
  public int qtdPets {get; set;}
  public int EnderecoID {get; set;}
  public Endereco Endereco {get; set;}
  public List<Pet> Pets {get; set;}
  public List<Adocao> Adocoes {get; set;}
}