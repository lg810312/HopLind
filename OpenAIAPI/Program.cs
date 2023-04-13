using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using Serilog;
using HopLind.OpenAIService;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services));

builder.Services.AddCors(o => o.AddDefaultPolicy(b => b.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
    .WithExposedHeaders(builder.Configuration["CORS:ExposedHeaders"].Split(','))
    .WithOrigins(builder.Configuration.GetSection("CORS:Origins").Get<string[]>())));

builder.Services.AddControllers()
    .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "HopLind API", Version = "v1", Description = "View and test HopLind API" });
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"), true);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = Microsoft.Net.Http.Headers.HeaderNames.Authorization,
        In = ParameterLocation.Header,
        Scheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        [new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference() { Id = "Bearer", Type = ReferenceType.SecurityScheme, },
        }] = Array.Empty<string>()
    });
});

// Forward the original scheme and client IP address
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddHttpClient();

builder.Services.Configure<Config>(builder.Configuration.GetSection("OpenAI"));
builder.Services.AddSingleton<OpenAIService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
