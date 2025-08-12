using Microsoft.EntityFrameworkCore;
using SolforbTest.Data;
using SolforbTest.Interfaces;
using SolforbTest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<IResourceFactory, SolforbTest.Factories.ResourceFactory>();
builder.Services.AddScoped<ResourceService>();
builder.Services.AddScoped<IMeasurementUnitRepository, MeasurementUnitRepository>();
builder.Services.AddScoped<IMeasurementUnitFactory, SolforbTest.Factories.MeasurementUnitFactory>();
builder.Services.AddScoped<MeasurementUnitService>();
builder.Services.AddScoped<IReceiptDocumentRepository, ReceiptDocumentRepository>();
builder.Services.AddScoped<IReceiptDocumentFactory, SolforbTest.Factories.ReceiptDocumentFactory>();
builder.Services.AddScoped<ReceiptDocumentService>();


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();