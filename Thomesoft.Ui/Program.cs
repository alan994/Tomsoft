using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;
using Tomsoft.BusinessLogic.BusinessLogic;
using Tomsoft.BusinessLogic.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((HostBuilderContext hosting, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration.Enrich.WithExceptionDetails();
    loggerConfiguration.Enrich.WithSpan();
                
    loggerConfiguration.ReadFrom.Configuration(builder.Configuration.GetSection("Serilog"));
    loggerConfiguration.WriteTo.Console();
});

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<GetPaymentTypeForProductRequestValidator>();
        config.AutomaticValidationEnabled = true;
    });


builder.Services.Configure<LuceedConfig>(builder.Configuration.GetSection(LuceedConfig.ConfigName));
builder.Services.AddLuceedApiClient();
builder.Services.AddBusinessLogic();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
