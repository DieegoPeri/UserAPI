using System.Net;
using System.Text.Json;
using UserAPI;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

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

        //var userInfo = await context.Request.ReadFromJsonAsync<User>();

        var userInfo = JsonSerializer.Deserialize(context.Request.Body, UserJsonContext.Default.User);

        if (userInfo == null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        userInfo.Load(userInfo.UserId, userInfo.Email);

        await context.Response.WriteAsync(JsonSerializer.Serialize(userInfo));
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return;
    }
});
#endregion
app.Run();
