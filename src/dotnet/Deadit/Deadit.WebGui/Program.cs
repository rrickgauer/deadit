using Deadit.Lib.Domain.Configurations;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Filter;
using Deadit.WebGui;
using System.Reflection;

bool isProduction = true;

#if DEBUG
isProduction = false;
#endif


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
    options.Filters.Add<ValidationErrorFilter>();
})

// https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-8.0#disable-automatic-400-response
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
    options.SuppressMapClientErrors = true;
})

.AddJsonOptions(options =>
{
    if (!isProduction)
    {
        options.JsonSerializerOptions.WriteIndented = true;
    }
});


// session management
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Deadit.Session";
    //options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.IsEssential = true;
});

#region - Dependency Injection -

if (isProduction)
{
    builder.Services.AddSingleton<IConfigs, ConfigurationProduction>();
}
else
{
    builder.Services.AddSingleton<IConfigs, ConfigurationDev>();
}

List<Assembly?> assemblies = new()
{
    Assembly.GetAssembly(typeof(IConfigs)),
    Assembly.GetExecutingAssembly(),
};

foreach (var assembly in assemblies)
{
    if (assembly == null)
    {
        continue;
    }

    WebGuiSetup.InjectServicesIntoAssembly(builder.Services, InjectionProject.Always | InjectionProject.WebGui, assembly);
}


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

#endregion



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.UseSession();

app.Run();
