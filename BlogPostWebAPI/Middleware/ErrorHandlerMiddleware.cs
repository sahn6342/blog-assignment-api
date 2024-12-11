namespace BlogPostWebAPI.Middleware
{
    /// <summary>
    /// Error hadler middleware to handle all global errors.
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        /// <summary>
        /// The next
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the specified context to handle all errors.
        /// </summary>
        /// <param name="context">The context.</param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
        }
    }
}
