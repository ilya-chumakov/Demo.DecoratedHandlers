using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo.DecoratedHandlers.Abstractions;

// shallow checks, room for improvement
internal class RegistrationVerifier(DelayedLog log)
{
    private static readonly Type RequestHandlerType = typeof(IRequestHandler<,>);

    public void VerifyRegistryCount(IReadOnlyCollection<Type> registryTypes)
    {
        if (registryTypes.Count == 1)
        {
            return;
        }
        else if (registryTypes.Count == 0)
        {
            log.Add(logger =>
                logger.LogWarning("No registry was found. Check if the source generator has run."));
        }
        else
        {
            string types = string.Join("," + Environment.NewLine, registryTypes.Select(x => x.FullName));

            string message =
                "More than one registry was found. Check for multiple registry implementations. Found types:"
                + Environment.NewLine
                + types;

            log.Add(logger => logger.LogWarning(message));
        }
    }

    public void VerifyServices(IServiceCollection services)
    {
        bool anyPipelineIsFound = services.Any(sd =>
            sd.ServiceType.IsGenericType &&
            sd.ServiceType.GetGenericTypeDefinition() == RequestHandlerType);

        if (!anyPipelineIsFound)
        {
            log.Add(logger =>
                logger.LogWarning("No pipelines registered. Check if a request handler is declared. " +
                                  "Check the generated registry's code to ensure the handler is found."));
        }
    }
}