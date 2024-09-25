using AutoMapper;
using FluentValidation;
using Library.Application.Mapping;
using Library.Application.Services;
using Library.Application.Servises;
using Library.Core.Contracts.Author;
using Library.Core.Contracts.Book;
using Library.Core.Contracts.Genre;
using Library.Core.Contracts.RequestValidation;
using Library.Persistence;
using Library.Persistence.Entities;
using Library.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.


builder.Services.AddDbContext<LibraryDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(LibraryDbContext)));
    });

builder.Services.AddScoped<IBooksRepository, BooksRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGenreService, GenreService>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

builder.Services.AddControllers();

builder.Services.AddScoped<IValidator<RequestAuthorDto>, RequestAuthorDtoValidator>();
builder.Services.AddScoped<IValidator<RequestUpdateAuthorDto>, RequestUpdateAuthorDtoValidator>();

builder.Services.AddScoped<IValidator<RequestBookDto>, RequestBookDtoValidator>();

builder.Services.AddScoped<IValidator<RequestGenreDto>, RequestGenreDtoValidator>();

builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingBook), typeof(MappingAuthor),typeof(MappingGenres));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();  

app.Run();
