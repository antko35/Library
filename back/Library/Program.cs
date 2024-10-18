using FluentValidation;
using Library.Application.Authorization;
using Library.Application.DTOs.Genre;
using Library.Application.Mapping;
using Library.Application.Services;
using Library.Application.Servises;
using Library.Application.Use_Cases.Genre;
using Library.Core.Abstractions.IInfrastructure;
using Library.Core.Abstractions.IRepository;
using Library.Core.Abstractions.IService;
using Library.Core.Contracts.Author;
using Library.Core.Contracts.Book;
using Library.Core.Contracts.Genre;
using Library.Core.Contracts.RequestValidation;
using Library.Core.Contracts.User;
using Library.Core.Enums;
using Library.Persistence;
using Library.Persistence.Repositories;
using Library.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

builder.Services.AddScoped<IBooksRepository, BooksRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IGenreRepository, GenreRepository>();
//builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IGetAllUseCase, GetAllUseCase>();
builder.Services.AddScoped<ICreateGenreUseCase, CreateGenreUseCase>();
builder.Services.AddScoped<IDeleteGenreUseCase, DeleteGenreUseCase>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService,UserService>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();


builder.Services.AddScoped<IValidator<RequestAuthorDto>, RequestAuthorDtoValidator>();
builder.Services.AddScoped<IValidator<RequestUpdateAuthorDto>, RequestUpdateAuthorDtoValidator>();

builder.Services.AddScoped<IValidator<RequestBookDto>, RequestBookDtoValidator>();

builder.Services.AddScoped<IValidator<RequestGenreDto>, RequestGenreDtoValidator>();

builder.Services.AddScoped<IValidator<RegisterRequestDto>, RegisterRequestDtoValidator>();
builder.Services.AddScoped<IValidator<LoginRequestUserDto>, LoginRequestUserDtoValidator>();

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

// TODO: реализуй exception handling middleware и в нем отдавай статус коды

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
