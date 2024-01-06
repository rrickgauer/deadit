using Deadit.Lib.Domain.Configurations;
using Deadit.Lib.Filter;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Repository.Implementations;
using Deadit.Lib.Repository.Other;
using Deadit.Lib.Service.Contracts;
using Deadit.Lib.Service.Implementations;
using Deadit.WebGui.Filter;

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
    if (isProduction)
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

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<IBannedCommunityNameService, BannedCommunityNameService>();
builder.Services.AddSingleton<ITableMapperService, TableMapperService>();
builder.Services.AddSingleton<IErrorMessageService, ErrorMessageService>();
builder.Services.AddSingleton<IResponseService, ResponseService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICommunityRepository, CommunityRepository>();
builder.Services.AddScoped<IBannedCommunityNameRepository, BannedCommunityNameRepository>();
builder.Services.AddSingleton<IErrorMessageRepository, ErrorMessageRepository>();

builder.Services.AddTransient<DatabaseConnection>();

builder.Services.AddScoped<InternalApiAuthFilter>();
builder.Services.AddScoped<LoginFirstRedirectFilter>();

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
