using CookiesUploading;
using OfficeFileUploader;
using PicturesUploader;

var builder = WebApplication.CreateBuilder(args);

// הוספת שירותים
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<UploadingService>(); // רישום השירות UploadingService
builder.Services.AddTransient<PicturesService>();
// Add services to the container
builder.Services.AddControllersWithViews();
// Add your ScreenshotService to the DI container
builder.Services.AddTransient<PicturesService>();
// Add services to the container.
builder.Services.AddControllersWithViews(); // רישום MVC Controllers
builder.Services.AddTransient<FileService>(); // רישום השירות שלך ל-DI

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "office",
    pattern: "Office/AutoUpload",
    defaults: new { controller = "Office", action = "AutoUpload" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
