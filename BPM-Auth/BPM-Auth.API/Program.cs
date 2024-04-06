using BPM_Auth.API.Middlewares;
using BPM_Auth.BLL;

var builder = WebApplication.CreateBuilder(args);

const string BPM_POLICY = "BPMPolicy";

builder.Services.AddControllers();

builder.Services.AddCors(options => options.AddPolicy(BPM_POLICY, policyBuilder =>
{
    policyBuilder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithExposedHeaders("Token-Expired");
}));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

BLLModule.Load(builder.Services, builder.Configuration);

var app = builder.Build();

app.UseMiddleware<GlobalErrorHandlingMiddlewar>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
