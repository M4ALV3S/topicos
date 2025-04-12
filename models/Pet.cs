using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

public class Pet {
  public int petID {get; set;}
  public string nome {get; set;}
  public int idade {get; set;}
  public string porte {get; set;}
  public string descricao {get; set;}
  public int AbrigoID {get; set;}
  public Abrigo Abrigo {get; set;}
  public Adocao Adocao {get; set;}

}