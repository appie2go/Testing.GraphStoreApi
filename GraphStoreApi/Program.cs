using GraphStoreApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IStorage, InMemory>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.AddEndpoints();

app.Run();