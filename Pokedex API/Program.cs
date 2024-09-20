using Pokedex.API.Business.V1;
using Pokedex_API_Data.V1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<PokedexApiService>();
builder.Services.AddScoped<IPokedexApiService, PokedexApiService>();
builder.Services.AddScoped<IPokedexRepository, PokedexRepository>();

builder.Services.AddHttpClient<PokedexRepository>();

builder.Services.Configure<Endpoints>(
    builder.Configuration.GetSection("Endpoints"));

var app = builder.Build();

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
