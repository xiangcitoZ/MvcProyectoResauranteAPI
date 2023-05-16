using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Azure;
using MvcProyectoResauranteAPI.Services;
using MvcRepasoSegundoExam.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient(builder.Configuration.GetSection("KeyVault"));
});

SecretClient secretClient =
 builder.Services.BuildServiceProvider().GetService<SecretClient>();
KeyVaultSecret keyVaultSecret = await
 secretClient.GetSecretAsync("StorageAccount");


string azureKeys = keyVaultSecret.Value;

BlobServiceClient blobServiceClient =
    new BlobServiceClient(azureKeys);
builder.Services.AddTransient<BlobServiceClient>(
    x => blobServiceClient);


builder.Services.AddTransient<ServiceApiRestaurante>();
builder.Services.AddControllersWithViews(
     options => options.EnableEndpointRouting = false);
builder.Services.AddTransient<ServiceStorageBlobs>();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});



//SEGURIDAD
builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();


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

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Managed}/{action=Login}/{id?}"
        );
});

app.Run();
