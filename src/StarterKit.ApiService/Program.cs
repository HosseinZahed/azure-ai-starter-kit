using StarterKit.ApiService;
using StarterKit.SemanticKernelService;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Swagger UI
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// Add User Secrets
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Add Semantic Kernel Service
builder.AddSemanticKernelService();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Register all endpoints
app.RegisterAllEndpoints();

// Configure Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.MapDefaultEndpoints();

app.Run();
