using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BukiApi.Data
{
    public class Startup
    {

        public AppSettings AppSettings { get; set; }

        public Startup(IOptions<AppSettings> appSettings) { 
            AppSettings = appSettings.Value;
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(AppSettings.ConnectionString, new MySqlServerVersion(new Version(8, 0, 21))));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme =  JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppSettings.JWT_Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            }   
    }
}
