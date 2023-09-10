using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NoteApp.Helpers;
using System.Text;

/// * Note App
/// * Created by Nikola Ilievski
/// * Version: 1.0.0 Stable

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("NoteAppCS");

//Ways to get values from AppSettings (OLD WAY)
var sectionValue = builder.Configuration.GetSection("TestSection").Value;
var sectionValue2 = builder.Configuration.GetValue<string>("TestSection");
var secretKeyFromAppSettings = builder.Configuration.GetSection("NoteAppSettings").GetValue<string>("SecretKey");

//Ways to get values from AppSettings (NEW WAY)
var noteAppSettings = builder.Configuration.GetSection("NoteAppSettings");
var noteAppSettingsObject = noteAppSettings.Get<NoteAppSettings>();

builder.Services.InjectServices();
builder.Services.InjectRepositories();
builder.Services.InjectDbContext(connectionString);

var secretKey = Encoding.ASCII.GetBytes(noteAppSettingsObject.SecretKey);

// CONFIGURE JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();