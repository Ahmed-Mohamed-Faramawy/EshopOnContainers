

using MyEshopOnContainers.MySampleProject.Catalog.API.src.Catalog;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationContext: IdentityDbContext<ApplicationUser>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {}

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
}