namespace API;

public static class CorsExtension
{
    public static IServiceCollection AddCorsExtension(this IServiceCollection services)
    {
        services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    policy.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true).AllowCredentials());
            }
        );
        return services;
    }
}