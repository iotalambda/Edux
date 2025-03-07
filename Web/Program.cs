using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Edux.Web.Components;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Radzen;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddServicesFromAssemblies([typeof(Program).Assembly]);

builder
    .Services
    .AddSingleton<IChatCompletionService>(sp =>
    {
        var apiKey = configuration["OpenAi:ApiKey"] ?? throw new Exception("OpenAI API key not provided in configuration.");
        return new OpenAIChatCompletionService("gpt-3.5-turbo", apiKey);
    })
    .AddTransient(sp =>
    {
        var kernel = new Kernel(sp);
        kernel.ImportPluginFromType<Edux.Web.Stuff.Rare.EduxEventHandlerBuilderPlugin>();
        return kernel;
    });

builder.Configuration
    .AddAzureKeyVault(
        new SecretClient(
            new Uri(configuration["KeyVault:Uri"] ?? throw new Exception("Key Vault uri not provided in configuration.")),
            new DefaultAzureCredential(),
            new SecretClientOptions().WithCustomDomainSupport()),
        new AzureKeyVaultConfigurationOptions());

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddRadzenComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
