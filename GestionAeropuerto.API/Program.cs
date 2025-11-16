using GestionAeropuerto.API.Data;
using GestionAeropuerto.API.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configurar Swagger/OpenAPI - VERSIÓN SIMPLE
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar SqlConnectionFactory
builder.Services.AddSingleton<SqlConnectionFactory>();

// Registrar Repositories
builder.Services.AddScoped<IVueloRepository, VueloRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IPasajeroRepository, PasajeroRepository>();
builder.Services.AddScoped<IPuertaRepository, PuertaRepository>();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();