using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    NullValueHandling = NullValueHandling.Ignore
};
builder.Services.AddRazorPages();
builder.Services.AddAuthentication("CookieAuthentication")
.AddCookie("CookieAuthentication", options =>
{
    options.LoginPath = "/Login";
    options.ExpireTimeSpan = TimeSpan.FromHours(23).Add(TimeSpan.FromMinutes(50));
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
