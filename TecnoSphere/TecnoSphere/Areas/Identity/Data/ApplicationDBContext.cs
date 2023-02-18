using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NuGet.Configuration;
using System.Reflection.Metadata;
using TecnoSphere.Areas.Identity.Data;
using TecnoSphere.Models;

namespace TecnoSphere.Areas.Identity.Data;
public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ApplicationUserEntityConfiguraion());
    }
    public class ApplicationUserEntityConfiguraion : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.FirstName).HasMaxLength(50);
            builder.Property(x => x.LastName).HasMaxLength(50);
            builder.Property(x => x.Email).HasMaxLength(255);
            builder.Property(x => x.DateOfBirth).HasMaxLength(50);
            builder.Property(x => x.ProfilePicture).HasMaxLength(50);
        }
    }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Galleries> Galleries { get; set; }
    public virtual DbSet<Blog> Blogs { get; set; }
    public virtual DbSet<Albums> Albums { get; set; }
    public virtual DbSet<BlogGallery> BlogGalleries { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<Services> Services { get; set; }
    public virtual DbSet<MenuPages> Pages { get; set; }
    public virtual DbSet<Portfolio> Portfolio { get; set; }

    public virtual DbSet<ClientLogo> ClientLogos { get; set; }

    public virtual DbSet<ContactUs> ContactUs { get; set; }
 }