using GraphStoreApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IStorage, InMemory>();

builder.Services.AddCors(o => o.AddPolicy("allow-all", policy  => policy.WithOrigins("*")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("allow-all");
app.AddEndpoints();

app.Run();