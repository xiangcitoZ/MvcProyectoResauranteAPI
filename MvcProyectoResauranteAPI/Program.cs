using Azure.Storage.Blobs;
using MvcProyectoResauranteAPI.Helpers;
using MvcProyectoResauranteAPI.Services;
using MvcRepasoSegundoExam.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<HelperPathProvider>();

// Add services to the container.
string azureKeys =
    builder.Configuration.GetValue<string>
    ("AzureKeys:StorageAccount");
BlobServiceClient blobServiceClient =
    new BlobServiceClient(azureKeys);
builder.Services.AddTransient<BlobServiceClient>(
    x => blobServiceClient);


builder.Services.AddTransient<ServiceApiRestaurante>();
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<ServiceStorageBlobs>();

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
    pattern: "{controller=Mesa}/{action=Mesa}/{id?}");

app.Run();
