using Microsoft.EntityFrameworkCore;

namespace UserService.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<Core.User.User> Users { get; init; }

    public DbSet<Core.EmailConfirmation.UserEmailConfirmation>
        UserEmailConfirmations { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Core.User.User>(builder =>
        {
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.NickName)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(x => x.SecretWort)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(x => x.NickNameHash)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(x => x.SecretWortHash)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(x => x.Image)
                .HasMaxLength(500);

            builder.HasIndex(x => x.NickNameHash)
                .IsUnique();

            builder.HasIndex(x => x.EmailHash)
                .IsUnique();
            
            builder.Property(x => x.ConfirmedEmail)
                .IsRequired()
                .HasDefaultValue(false);
        });

        modelBuilder.Entity<Core.EmailConfirmation.UserEmailConfirmation>(
            builder =>
            {
                builder.HasKey(x => x.Id);

                builder.Property(x => x.TokenHash)
                    .IsRequired();

                builder.HasIndex(x => x.TokenHash)
                    .IsUnique();

                builder.Property(x => x.CreatedAt)
                    .IsRequired();

                builder.Property(x => x.ExpiresAt)
                    .IsRequired();

                builder.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
    }
}