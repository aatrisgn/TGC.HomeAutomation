using TGC.HomeAutomation.API;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// All injections are/should be handled inside extension
builder.Services.AddHomeAutomationApiInjections(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseCors("ALLOW_DEVELOPMENT_CORS_ORIGINS_POLICY");
}
else
{
	//TODO: Make this in a better way
	app.UseCors("ALLOW_PROD_CORS_ORIGINS_POLICY");
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
