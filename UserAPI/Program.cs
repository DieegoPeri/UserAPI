using System.Net;
using System.Text.Json;
using UserAPI;

var builder = WebApplication.CreateBuilder(args);

// Swagger configuration for testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuration
app.UseSwagger();
app.UseSwaggerUI();

#region POST and GET methods
app.MapGet("/", () => "Hello World!");

app.MapPost("/GetUser", async context =>
{
    try
    {
        if (!context.Request.HasJsonContentType())
        {
            context.Response.StatusCode = (int) HttpStatusCode.UnsupportedMediaType;
            return;
        }

        var userInfo = JsonSerializer.Deserialize(context.Request.Body, UserJsonContext.Default.User);

        if (userInfo == null)
        {
            // In case a User cannot be built, it means the request is not valid (UserID or Email not set)
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        await context.Response.WriteAsync(userInfo.GetUserJson());
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return;
    }
});

app.MapGet("/GetUserByEmail/{email}", (string email) =>
{
    User user = new User() { Email= email };
    Results.Ok(user.GetUserJson());
});

app.MapGet("/GetUserByUserId/{userId}", (string userId) =>
{
    User user = new User() { UserId= userId };
    Results.Ok(user.GetUserJson());
});
#endregion
app.Run();
