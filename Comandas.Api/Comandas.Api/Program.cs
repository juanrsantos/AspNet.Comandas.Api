using Comandas.Api;
using Comandas.Api.Data;
using Comandas.Api.Repositories;
using Comandas.Api.Services.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var conexao = builder.Configuration.GetConnectionString("ConexaoSqlServerSomee");
// Add services to the container.
builder.Services.AddDbContext<ComandaDbContext>(options =>
{
    options.UseSqlServer(conexao).EnableSensitiveDataLogging();
    //options.UseMySql(conexao, ServerVersion.Parse("9.1.0 - MySQL Community Server - GPL"));
});

builder.Services.AddScoped<IMesaRepository, MesaRepository>(); // Injenção de dependencia, para usarmos o repositorio no controller , apos separação em camadas.
builder.Services.AddScoped<IMesaServices, MesaServices>();
builder.Services.AddScoped<IComandasRepository, ComandasRepository>();
builder.Services.AddScoped<IComandasServices, ComandasServices>();

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
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n" +
            "Digite 'Bearer' [espaço] e então seu token no campo abaixo.\r\n\r\n" +
            "Exemplo (informar sem as aspas): 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
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
