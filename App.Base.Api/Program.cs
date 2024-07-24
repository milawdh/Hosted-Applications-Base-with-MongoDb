using Base.WebFrameWork.BootStrapers.Base;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
#region Cutomize Container

builder.Services.AppendStartupConfigs(builder.Configuration);
builder.Services.AddSwaggerExamples();
builder.Services.AddSwaggerExamplesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
#endregion
builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();


    app.UseSwaggerUI(c =>
    {
        c.DisplayRequestDuration();
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BASE");
    });
}
app.UseHsts();
app.UseHttpsRedirection();

//Authentication
app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler("/Error");
app.UseElmah();

app.MapControllers();

app.Run();
