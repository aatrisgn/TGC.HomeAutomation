﻿using TGC.HomeAutomation.API;
using TGC.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// All injections are/should be handled inside extension
builder.Services.AddHomeAutomationApiInjections(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
app.UseCors("CORS_ORIGINS_POLICY");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSignalR();

app.MapControllers();

app.Run();
