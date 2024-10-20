using FluentValidation;
using FluentValidation.AspNetCore;
using Library.API;
using Library.Application.Authorization;
using Library.Application.Contracts.Author;
using Library.Application.Contracts.Book;
using Library.Application.Contracts.RequestValidation;
using Library.Application.Contracts.User;
using Library.Application.Contracts.Validation.Author;
using Library.Application.Contracts.Validation.Book;
using Library.Application.Contracts.Validation.User;
using Library.Application.DTOs.Genre;
using Library.Application.Mapping;
using Library.Application.Services;
using Library.Application.Use_Cases.Author;
using Library.Application.Use_Cases.Books;
using Library.Application.Use_Cases.Genre;
using Library.Application.Use_Cases.User;
using Library.Core.Abstractions;
using Library.Core.Abstractions.IInfrastructure;
using Library.Core.Abstractions.IRepository;
using Library.Core.Contracts.Genre;
using Library.Core.Enums;
using Library.Persistence;
using Library.Persistence.Repositories;
using Library.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddDbContext<LibraryDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(LibraryDbContext)));
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,

            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["jwt_cookie"];

                return Task.CompletedTask;
            }
        };
    }
    );

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Role", Role.Admin.ToString()));
    options.AddPolicy("User", policy => policy.RequireClaim("Role", Role.User.ToString()));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IImageService,ImageService>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IValidationService, ValidationService>();

builder.Services.AddScoped<IBooksRepository, BooksRepository>();
builder.Services.AddScoped<GetAllBooksUseCase>();
builder.Services.AddScoped<GetBooksByPageUseCase>();
builder.Services.AddScoped<GetBookByIdUseCase>();
builder.Services.AddScoped<GetBookByIsbnUseCase>();
builder.Services.AddScoped<GetBooksCountUseCase>();
builder.Services.AddScoped<CreateBookUseCase>();
builder.Services.AddScoped<UpdateBookUseCase>();
builder.Services.AddScoped<BorrowBookUseCase>();
builder.Services.AddScoped<ReturnBookUseCase>();
builder.Services.AddScoped<UploadCoverUseCase>();
builder.Services.AddScoped<DeleteBookUseCase>();

builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGetAllUseCase, GetAllUseCase>();
builder.Services.AddScoped<ICreateGenreUseCase, CreateGenreUseCase>();
builder.Services.AddScoped<IDeleteGenreUseCase, DeleteGenreUseCase>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<GetInfoUseCase>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegistrationUseCase>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<CreateAuthorUseCase>();
builder.Services.AddScoped<DeleteAuthorUseCase>();
builder.Services.AddScoped<GetAllAuthorsUseCase>();
builder.Services.AddScoped<GetAuthorByIdUseCase>();
builder.Services.AddScoped<GetBooksByAuthorUseCase>();
builder.Services.AddScoped<UpdateAuthorUseCase>();

builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestUserDtoValidator>();


builder.Services.AddScoped<IValidator, RequestAuthorDtoValidator>();
builder.Services.AddScoped<IValidator, RequestUpdateAuthorDtoValidator>();

builder.Services.AddScoped<IValidator, RequestBookDtoValidator>();
builder.Services.AddScoped<IValidator, RequestUpdateBookDtoValidator>();
builder.Services.AddScoped<IValidator, RequestUploadCoverValidator>();

builder.Services.AddScoped<IValidator, RequestGenreDtoValidator>();

builder.Services.AddScoped<IValidator, RegisterRequestDtoValidator>();
builder.Services.AddScoped<IValidator, LoginRequestUserDtoValidator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") 
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingBook), typeof(MappingAuthor),typeof(MappingGenres), typeof(MappingUser));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();


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

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();  

app.Run();
