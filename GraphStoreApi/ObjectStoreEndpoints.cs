using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace GraphStoreApi;

public static class ObjectStoreEndpoints
{
    public static WebApplication? AddEndpoints(this WebApplication app)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }
        
        app
            .MapGet("/{endpointAddress}", ([FromServices] IStorage objectStore, [FromRoute] string endpointAddress) => objectStore.GetAll(endpointAddress))
            .WithOpenApi();;
        
        app
            .MapGet("/{endpointAddress}/{key}", ([FromServices] IStorage objectStore, [FromRoute] string endpointAddress, [FromRoute] string key) =>
            {
                try
                {
                    return objectStore.Get(endpointAddress, key);
                }
                catch (KeyNotFoundException)
                {
                    return Results.StatusCode(StatusCodes.Status404NotFound);
                }
            })
            .WithOpenApi();

        app
            .MapPost("/{endpointAddress}", async ([FromServices] IStorage objectStore, [FromRoute] string endpointAddress, HttpRequest req, Stream body) =>
            {
                const int maxMessageSize = 80 * 1024;
                var readSize = (int?)req.ContentLength ?? (maxMessageSize + 1);
                var buffer = new byte[readSize];
                var read = await body.ReadAtLeastAsync(buffer, readSize, false);
                
                if (read > maxMessageSize)
                {
                    return Results.StatusCode(StatusCodes.Status400BadRequest);
                }
                
                var jsonString = Encoding.UTF8.GetString(buffer);
                var jObject = JsonNode.Parse(jsonString);
                if (jObject == null)
                {
                    return Results.StatusCode(StatusCodes.Status400BadRequest);
                }
                
                jObject["id"] ??= Guid.NewGuid().ToString();
                var id = jObject["id"]?.GetValue<object>().ToString();
                
                try
                {
                    objectStore.Create(endpointAddress, id, jObject);
                    return Results.StatusCode(StatusCodes.Status201Created);
                }
                catch (ArgumentException)
                {
                    return Results.StatusCode(StatusCodes.Status409Conflict);
                }
            })
            .WithOpenApi();

        app
            .MapPut("{endpointAddress}/{key}", ([FromServices] IStorage objectStore, [FromRoute] string endpointAddress, [FromRoute] string key, [FromBody] object payload) =>
            {
                try
                {
                    objectStore.Update(endpointAddress, key, payload);
                    return Results.StatusCode(StatusCodes.Status200OK);
                }
                catch (KeyNotFoundException)
                {
                    return Results.StatusCode(StatusCodes.Status404NotFound);
                }
            })
            .WithOpenApi();    

        app
            .MapPatch("{endpointAddress}/{key}", ([FromRoute] string endpointAddress, [FromRoute] string key) => Results.StatusCode(StatusCodes.Status418ImATeapot))
            .WithOpenApi();
        
         app
             .MapDelete("{endpointAddress}/{key}", ([FromServices] IStorage objectStore, [FromRoute] string endpointAddress, [FromRoute] string key) =>
             {
                 try
                 {
                     objectStore.Delete(endpointAddress, key);
                     return Results.StatusCode(StatusCodes.Status200OK);
                 }
                 catch (KeyNotFoundException)
                 {
                     return Results.StatusCode(StatusCodes.Status404NotFound);
                 }
             })
             .WithOpenApi();

        return app;
    }
}