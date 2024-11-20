using Comandas.Api;
using Comandas.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var conexao = builder.Configuration.GetConnectionString("ConexaoMySql");
// Add services to the container.
builder.Services.AddDbContext<ComandaDbContext>(options =>
{
    options.UseMySql(conexao, ServerVersion.Parse("9.1.0 - MySQL Community Server - GPL"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Execução das migration do banco de forma automatica , ao iniciar o aplicativo.
using (var escopo = app.Services.CreateScope())
{
    var contexto = escopo.ServiceProvider.GetRequiredService<ComandaDbContext>();
    contexto.Database.Migrate();
    InicializarDados.Semear(contexto);
}

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
