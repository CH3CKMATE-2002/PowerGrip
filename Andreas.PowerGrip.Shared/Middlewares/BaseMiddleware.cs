namespace Andreas.PowerGrip.Shared.Middlewares;

public abstract class BaseMiddleware
{
    private readonly RequestDelegate _next;

    protected BaseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Pre-processing (before calling the next middleware)
        await BeforeRequestAsync(context);

        // Call the next middleware in the pipeline
        if (_next != null)
        {
            await _next(context);
        }

        // Post-processing (after the request has passed through)
        await AfterRequestAsync(context);
    }

    protected virtual Task BeforeRequestAsync(HttpContext context) => Task.CompletedTask;
    protected virtual Task AfterRequestAsync(HttpContext context) => Task.CompletedTask;
}