using TGC.HomeAutomation.API;
using TGC.HomeAutomation.API.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(options =>
{
	options.ValidateScopes = true;
	options.ValidateOnBuild = true;
});

builder.WebHost.UseKestrel(options => options.AddServerHeader = false);

builder.Configuration.AddEnvironmentVariables();

// All injections are/should be handled inside extension
builder.Services.AddHomeAutomationApiInjections(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
app.UseCors("CORS_ORIGINS_POLICY");
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSignalR();

app.MapControllers();

app.Run();

public partial class Program { }
