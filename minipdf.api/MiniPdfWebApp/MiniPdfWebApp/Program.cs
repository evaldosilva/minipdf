using Data.MiniPdf;
using Domain.AppUser;
using Domain.PdfCompressor;
using Microsoft.AspNetCore.Http.Features;
using Service.AppUser;
using Service.PdfCompressor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IPdfCompressor, PdfCompressor>();
builder.Services.AddScoped<IAppUser, AppUser>();
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();

// Set the limit to 1GB
builder.Services.Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = 1073741824; });

// Set the limit to 1GB on Kestrel as well
builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 1073741824);

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("http://localhost:4200", "https://localhost:4200"));

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
