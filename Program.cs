using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
var app = builder.Build();


app.MapPost("/enderecos/cadastrar", ([FromBody] Endereco endereco, [FromServices] AppDbContext context) =>
{
    Endereco? enderecoBuscado = context.Enderecos.FirstOrDefault(e => e.logradouro.ToUpper().Trim() == endereco.logradouro.ToUpper().Trim() && e.numero == endereco.numero);
    if (enderecoBuscado is null)
    {
        context.Enderecos.Add(endereco);
        context.SaveChanges();
        return Results.Created("Endereco cadastrado com sucesso!", endereco);
    }
    return Results.BadRequest($"Já existe esse endereco cadastrado no sistema, cujo id é: {enderecoBuscado.EnderecoID}");

});



app.MapGet("/abrigos/listar", ([FromServices] AppDbContext context) =>
{
    if (context.Abrigos.Any())
    {
        return Results.Ok(context.Abrigos.Include(x => x.Endereco).Include(x => x.Pets).Include(x => x.Adocoes)
            .ToList());

    }
    return Results.NotFound("Não há nenhum abrigo cadastrado!");

});



app.MapGet("/abrigos/buscar-por-cidade/{cidade}", ([FromRoute] string cidade, [FromServices] AppDbContext context) =>
{
    List<Abrigo> abrigos = context.Abrigos.Where(a => a.Endereco.cidade.ToUpper().Trim() == cidade.ToUpper().Trim())
    .Include(x => x.Endereco).Include(x => x.Pets).Include(x => x.Adocoes).ToList();

     if (!abrigos.Any())
    {
        return Results.NotFound("Não há nenhum abrigo nessa cidade");
    }

    return Results.Ok(abrigos);
});



app.MapPost("/abrigos/cadastrar", ([FromBody] Abrigo abrigo, [FromServices] AppDbContext context) =>
{
    Abrigo? abrigoBuscado = context.Abrigos.FirstOrDefault(a => a.nome.ToUpper().Trim() == abrigo.nome.ToUpper().Trim());
    if (abrigoBuscado is null)
    {
        context.Abrigos.Add(abrigo);
        context.SaveChanges();
        return Results.Created("Abrigo cadastrado com sucesso!", abrigo);
    }
    return Results.BadRequest($"Já existe um abrigo cadastrado no sistema com este nome, cujo id é: {abrigoBuscado.abrigoID}");

});



app.MapDelete("/abrigos/excluir/{id}", ([FromRoute] int id, [FromServices] AppDbContext context) =>
{
    Abrigo? abrigo = context.Abrigos.Find(id);
    if (abrigo is null)
    {
        return Results.NotFound("Não existe nenhum abrigo com esse ID");
    }
    context.Abrigos.Remove(abrigo);
    context.SaveChanges();
    return Results.Ok("Abrigo excluído com sucesso!");

});



app.MapPost("/pets/cadastrar", (Pet pet, [FromServices] AppDbContext context) =>
{
    Abrigo abrigo = context.Abrigos.FirstOrDefault(x => x.abrigoID == pet.AbrigoID);

    if (abrigo is null)
    {
        return Results.NotFound("Não existe um abrigo com esse ID");
    } 

    Pet? petBuscado = context.Pets.FirstOrDefault(p => p.petID == pet.petID);
    if (petBuscado is null)
    {
        context.Pets.Add(pet);
        context.SaveChanges();
        return Results.Created("Pet cadastrado com sucesso!", pet);
    }
    return Results.BadRequest("Já existe um pet cadastrado no sistema com este id.");
});



app.MapGet("/pets/listar", ([FromServices] AppDbContext context) =>
{
    if (context.Pets.Any())
    {
        return Results.Ok(context.Pets.Include(a => a.Abrigo).ToList());

    }
    return Results.NotFound("Não há nenhum pet cadastrado!");
});




app.MapGet("/pets/buscar/{id}", ([FromServices] AppDbContext context, int id) =>
{
    return Results.Ok(context.Pets.FirstOrDefault(p => p.petID == id));
});



app.MapPut("/pets/alterar/{id}", ([FromRoute] int id, [FromBody] Pet petAlterado, [FromServices] AppDbContext context) =>
{
    if (id != petAlterado.petID)
    {
        return Results.BadRequest("O ID do pet na URL deve ser igual ao ID do corpo da requisição.");
    }

    Pet? pet = context.Pets.Find(id);
    if (pet is null)
    {
        return Results.NotFound("Pet não encontrado!");
    }

    pet.nome = petAlterado.nome;
    pet.idade = petAlterado.idade;
    pet.porte = petAlterado.porte;
    pet.descricao = petAlterado.descricao;
    pet.AbrigoID = petAlterado.AbrigoID;

    context.SaveChanges();
    return Results.Ok("Pet alterado com sucesso!");
});



app.MapDelete("/pets/excluir/{id}", ([FromRoute] int id, [FromServices] AppDbContext context) =>
{
    Pet? pet = context.Pets.Find(id);
    if (pet is null)
    {
        return Results.NotFound("Não existe nenhum pet com esse ID");
    }
    context.Pets.Remove(pet);
    context.SaveChanges();
    return Results.Ok("Pet excluído com sucesso!");

});



app.MapGet("/pets/buscar-por-abrigo/{nome}", ([FromRoute] string nome, [FromServices] AppDbContext context) =>
{
    var pets = context.Pets
        .Where(p => p.Abrigo.nome.ToUpper().Trim() == nome.ToUpper().Trim())
        .Include(x => x.Abrigo)
        .Include(x => x.Adocao)
        .ToList();

    if (!pets.Any())
    {
        return Results.NotFound("Não há pet nesse abrigo.");
    }

    return Results.Ok(pets);
});



app.MapPost("/adocoes/cadastrar", (Adocao adocao, [FromServices] AppDbContext context) =>
{
    Abrigo? abrigo = context.Abrigos.FirstOrDefault(x => x.abrigoID == adocao.AbrigoID);

    if (abrigo is null)
    {
        return Results.NotFound("Não existe um abrigo com esse ID");
    }    

    Pet? pet = context.Pets.FirstOrDefault(x => x.petID == adocao.petID);

    if (pet is null)
    {
        return Results.NotFound("Não existe nenhum pet com esse ID");
    }

    Adocao? petAdotado = context.Adocoes.FirstOrDefault(x => x.petID == adocao.petID);

    if (petAdotado is not null){
      return Results.BadRequest("Esse Pet já foi adotado!");  
    }  

    Adocao? adocaoBuscada = context.Adocoes.FirstOrDefault(a => a.AdocaoID == adocao.AdocaoID);
    if (adocaoBuscada is null)
    {
        context.Adocoes.Add(adocao);
        context.SaveChanges();
        return Results.Ok("Adocao cadastrada com sucesso!");
    }
    return Results.BadRequest("Adocao já cadastrada!");
});



app.MapGet("/adocoes/listar", ([FromServices] AppDbContext context) =>
{
    if (context.Adocoes.Any())
    {
        return Results.Ok(context.Adocoes.Include(x => x.Abrigo).Include(x => x.Pet).ToList());

    }
    return Results.NotFound("Não há nenhuma adocao cadastrada!");
});



app.Run();