using MovieAPI.Services;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<ITmdbClient, TmdbClient>((sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var token = config["Tmdb:ReadAccessToken"];

    if (string.IsNullOrWhiteSpace(token))
    {
        throw new InvalidOperationException(
            "TMDB ReadAccessToken is missing. Configure it using User Secrets or environment variables.");
    }

    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
    client.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", token);
});

builder.Services.AddScoped<IMovieService, MovieService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
