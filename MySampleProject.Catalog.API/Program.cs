using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Required for IdentityDbContext

// --- ADDED REQUIRED USINGS FOR YOUR PROJECT STRUCTURE ---
// Note: These usings replace the placeholder classes previously at the bottom of the file
using MyEshopOnContainers.MySampleProject.Catalog.API.src.Catalog;// For ApplicationDbContext (assuming Identity context is here)
// --- END OF ADDED USINGS ---


// =======================================================================
// ARCHITECTURAL NOTES:
// This file combines all configurations for the Catalog and Identity bounded contexts
// into a single service, creating a Monolithic structure.
// NOTE: Both DbContexts (Catalog and Identity) are now configured to use the same connection string.
// =======================================================================

// --- NOTE: Top-level statements begin here and must be contiguous ---

var builder = WebApplication.CreateBuilder(args);


// =======================================================================
// 2. SERVICE REGISTRATION (Dependency Injection Container Setup)
// =======================================================================

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = 
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = null; 
    });

// --- CATALOG CONTEXT REGISTRATION ---
builder.Services.AddDbContext<CatalogContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("CatalogDB");
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 21)); 
    
    options.UseMySql(
        connectionString,
        serverVersion,
        options => options.SchemaBehavior(MySqlSchemaBehavior.Ignore) 
    );
});

// Register the Catalog Application Service
builder.Services.AddScoped<CatalogService>(); 

// --- IDENTITY CONTEXT AND SERVICE REGISTRATION ---
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    // FIX: Using "CatalogDB" connection string as requested, removing "IdentityDB" reference.
    var connectionString = builder.Configuration.GetConnectionString("CatalogDB"); 
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 21)),
        mySqlOptions => mySqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)
    );
});

// Add ASP.NET Identity Framework (Registers UserManager, SignInManager, etc.)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>() 
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();

// Register the AccountService (FIXES THE InvalidOperationException)
// This is now resolvable because the UserManager dependency is registered above.
builder.Services.AddScoped<AccountService>(); 


// Add services to the container.
builder.Services.AddOpenApi();

var app = builder.Build();

// =======================================================================
// 3. HTTP PIPELINE CONFIGURATION
// =======================================================================

// Apply Migrations to both databases on startup
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    
    // Catalog DB Migration
    var catalogDb = serviceProvider.GetRequiredService<CatalogContext>();
    catalogDb.Database.Migrate(); 

    // Identity DB Migration (Will use the same CatalogDB connection)
    var identityDb = serviceProvider.GetRequiredService<ApplicationContext>();
    identityDb.Database.Migrate(); 
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage(); 
}

app.UseHttpsRedirection();

// Maps routes for API Controllers (Now handles both CatalogController and AccountController)
app.MapControllers();

app.Run();

public partial class Program { }

// =======================================================================
// CLASS/TYPE DEFINITIONS (REMOVED TO RESOLVE CS0101 DUPLICATION ERRORS)
// =======================================================================