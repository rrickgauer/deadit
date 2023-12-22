using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Configurations;
using Deadit.Lib.Filters;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Repository.Implementations;
using Deadit.Lib.Repository.Other;
using Deadit.Lib.Service.Contracts;
using Deadit.Lib.Service.Implementations;

var builder = WebApplication.CreateBuilder(args);

bool isProduction = true;

#if DEBUG
isProduction = false;
#endif


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
builder.Services.AddSingleton<ITableMapperService, TableMapperService>();
builder.Services.AddSingleton<IErrorMessageService, ErrorMessageService>();
builder.Services.AddSingleton<IResponseService, ResponseService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IErrorMessageRepository, ErrorMessageRepository>();

builder.Services.AddTransient<DatabaseConnection>();

builder.Services.AddScoped<InternalApiAuthFilter>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddControllersWithViews();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
});

builder.Services.AddDistributedMemoryCache();



builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Deadit.Session";
    //options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.IsEssential = true;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.UseSession();

app.Run();
