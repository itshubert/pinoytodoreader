using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PinoyTodo.Reader.Infrastructure.Persistence.Configurations;


public sealed class TaskConfiguration : IEntityTypeConfiguration<Domain.TaskAggregate.Task>
{
    public void Configure(EntityTypeBuilder<Domain.TaskAggregate.Task> builder)
    {
        builder.ToTable("Tasks");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.IsCompleted)
            .IsRequired();

        builder.Property(t => t.CompletionTime);

        builder.HasIndex(t => t.IsCompleted);
        builder.HasIndex(t => t.CompletionTime);
    }
}