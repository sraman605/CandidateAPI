using CandidateAPI.Data;
using CandidateAPI.Repositories;
using CandidateAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace CandidateAPI.Config
{
    public static class DIConfig
    {
        public static WebApplicationBuilder ConfigDI(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
            builder.Services.AddScoped<ICandidateService, CandidateService>();
            /*using in memory caching for simplicity.
            however, we can use tools lie Redis once we scale up. */ 
            builder.Services.AddMemoryCache();
            return builder;

        }
    }
}
