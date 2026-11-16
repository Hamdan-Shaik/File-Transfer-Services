namespace FxferService.Host.Composition;

public static class Middleware
{
    public static WebApplication UseBasFileTransferApiHost(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("FxferService.Host");
        logger.LogInformation("Current Environment is {environmentName}", app.Environment.EnvironmentName);

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<ApiExceptionHandlingMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<RequestContextMiddleware>();

        app.MapControllers();

        return app;
    }

    public static WebApplication UseFxferMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<ApiExceptionHandlingMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<RequestContextMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }
}
