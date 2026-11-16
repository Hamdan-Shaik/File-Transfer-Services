using Fxfer.Bff.Host.Middleware;

namespace Fxfer.Bff.Host.Composition;

public static class Middleware
{
    public static WebApplication UseFxferBff(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Fxfer.Bff");
        logger.LogInformation("Current Environment is {environmentName}", app.Environment.EnvironmentName);

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        // T-002: Authenticate the operator JWT before deriving tenant context.
        app.UseAuthentication();

        app.UseMiddleware<CorrelationMiddleware>();
        app.UseMiddleware<ErrorNormalizationMiddleware>();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
