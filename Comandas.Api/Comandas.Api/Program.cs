using Comandas.Api;
using Comandas.Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var conexao = builder.Configuration.GetConnectionString("ConexaoSqlServerSomee");
// Add services to the container.
builder.Services.AddDbContext<ComandaDbContext>(options =>
{
    options.UseSqlServer(conexao).EnableSensitiveDataLogging();
    //options.UseMySql(conexao, ServerVersion.Parse("9.1.0 - MySQL Community Server - GPL"));
});
// Adicionando suporte a autenticação JWT 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = false,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3e8acfc238f45a314fd4b2bde272678ad30bd1774743a11dbc5c53ac71ca494b"))
    };
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
});

var app = builder.Build();

// Execução das migration do banco de forma automatica , ao iniciar o aplicativo.
using (var escopo = app.Services.CreateScope())
{
    var contexto = escopo.ServiceProvider.GetRequiredService<ComandaDbContext>();
    contexto.Database.Migrate();
    InicializarDados.Semear(contexto);
}

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
        app.UseSwagger();
        app.UseSwaggerUI();
   // }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
