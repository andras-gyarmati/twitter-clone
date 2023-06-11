using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace TwitterClone;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        const string corsPolicy = "twitter-clone-cors-policy";

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddRouting(options => { options.LowercaseUrls = true; });
        builder.Services.AddControllers(mvcOptions => mvcOptions.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer())));
        builder.Services.AddAuthorization();
        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<IUserContext, UserContext>();

        // Authentication
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        builder.Services.Configure<JwtSettings>(jwtSettings);
        var key = Encoding.ASCII.GetBytes(jwtSettings.Get<JwtSettings>().Key);
        builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtSettings.Get<JwtSettings>().Issuer,
                    ValidAudience = jwtSettings.Get<JwtSettings>().Audience
                };
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // builder.Services.AddDbContext<TwitterCloneDbContext>(options => options.UseSqlite());
        builder.Services.AddDbContext<TwitterCloneDbContext>();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(corsPolicy, corsPolicyBuilder =>
            {
                corsPolicyBuilder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Content-Disposition");
            });
        });
        var app = builder.Build();
        app.UseCors(corsPolicy);
        app.UseAuthorization();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    private class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            return value == null ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }

    private static string GenerateKey(int size)
    {
        var rng = new RNGCryptoServiceProvider();
        var bytes = new byte[size];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}
