using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PinoyCleanArch;
using PinoyTodo.Reader.Application.Common.Interfaces;
using PinoyTodo.Reader.Infrastructure.Messaging;
using PinoyTodo.Reader.Infrastructure.Messaging.EventProcessors;
using PinoyTodo.Reader.Infrastructure.Messaging.Events;
using PinoyTodo.Reader.Infrastructure.Persistence;
using PinoyTodo.Reader.Infrastructure.Persistence.Repositories;

namespace PinoyTodo.Reader.Infrastructure;

public static partial class DependencyInjectionRegister
{
    public static IServiceCollection AddLocalInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TaskDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("TaskDbContext");
            options.UseNpgsql(connectionString);
        });

        services.AddInfrastructure();

        services.Configure<AWSSettings>(configuration.GetSection("AWS"));
        services.AddSingleton<IAmazonSQS>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<IAmazonSQS>>();
            var awsSettings = sp.GetRequiredService<IOptions<AWSSettings>>().Value;

            if (awsSettings.UseLocalStack)
            {
                var serviceUrl = !string.IsNullOrEmpty(awsSettings.ServiceUrl) ? awsSettings.ServiceUrl : "http://localhost:4566";

                logger.LogInformation("Configuring AmazonSQSClient for LocalStack at {ServiceUrl}", serviceUrl);

                var config = new AmazonSQSConfig { ServiceURL = serviceUrl };
                return new AmazonSQSClient(new AnonymousAWSCredentials(), config);
            }

            var region = awsSettings.Region ?? "us-east-1";
            logger.LogInformation("Configuring AmazonSQSClient for AWS region {Region}", region);
            var profileName = Environment.GetEnvironmentVariable("AWS_PROFILE");

            if (!string.IsNullOrEmpty(profileName))
            {
                var credentialProfileStoreChain = new CredentialProfileStoreChain();
                if (credentialProfileStoreChain.TryGetProfile(profileName, out var profile))
                {
                    var credentials = profile.GetAWSCredentials(credentialProfileStoreChain);
                    return new AmazonSQSClient(credentials, RegionEndpoint.GetBySystemName(region));
                }
            }

            return new AmazonSQSClient(RegionEndpoint.GetBySystemName(region));
        });

        services.AddSqsMessageProcessors(configuration);

        services.AddScoped<ITaskRepository, TaskRepository>();
        return services;
    }

    private static IServiceCollection AddSqsMessageProcessors(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<QueueSettings>(configuration.GetSection("QueueSettings"));

        services.AddMessaging<TaskCreatedEvent, TaskCreatedEventProcessor>(sp =>
        {
            return sp.GetRequiredService<IOptions<QueueSettings>>().Value.TaskCreated ?? throw new InvalidOperationException("TaskCreated queue URL is not configured.");
        });

        services.AddMessaging<TaskCompletedEvent, TaskCompletedEventProcessor>(sp =>
        {
            return sp.GetRequiredService<IOptions<QueueSettings>>().Value.TaskCompleted ?? throw new InvalidOperationException("TaskCompleted queue URL is not configured.");
        });

        services.AddMessaging<TaskDeletedEvent, TaskDeletedEventProcessor>(sp =>
        {
            return sp.GetRequiredService<IOptions<QueueSettings>>().Value.TaskDeleted ?? throw new InvalidOperationException("TaskDeleted queue URL is not configured.");
        });

        return services;
    }

    public static IServiceCollection AddMessaging<TEvent, TProcessor>(
    this IServiceCollection services,
    Func<IServiceProvider, string> queueUrlFactory)
        where TProcessor : class, IEventProcessor<TEvent>
    {

        services.AddScoped<IEventProcessor<TEvent>, TProcessor>();

        services.AddHostedService(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<SqsConsumerBackgroundService<TEvent, TProcessor>>>();
            var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            var sqs = sp.GetRequiredService<IAmazonSQS>();
            var queueUrl = queueUrlFactory(sp);

            logger.LogInformation("Starting SQS consumer for queue: {QueueUrl}", queueUrl);

            return new SqsConsumerBackgroundService<TEvent, TProcessor>(
                logger,
                serviceScopeFactory,
                sqs,
                queueUrl);
        });

        return services;
    }
}