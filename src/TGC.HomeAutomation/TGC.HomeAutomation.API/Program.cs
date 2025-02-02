using TGC.HomeAutomation.API;

var builder = WebApplication.CreateBuilder(args);

// All injections are/should be handled inside extension
builder.Services.AddHomeAutomationApiInjections();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
