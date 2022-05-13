using DEVTRACKR.API.Persistence;
using DEVTRACKR.API.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DevTrackRCs");
//builder.Services.AddDbContext<DevTrackRContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddDbContext<DevTrackRContext>(o => o.UseInMemoryDatabase(connectionString));

builder.Services.AddScoped<IPackageRepository,PackageRepository>();

var sendGridApikey = builder.Configuration.GetSection("SendGridApiKey").Value;

builder.Services.AddSendGrid(o => o.ApiKey = sendGridApikey);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => {
   o.SwaggerDoc("v1", new OpenApiInfo{
        Title = "DevTrackR.API",
        Version = "v1",
        Contact = new OpenApiContact {
            Name = "Luan Santos",
            Email = "luancosmefilho@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/-luansantos-/")
        }
    });

    var xmlPath = Path.Combine(AppContext.BaseDirectory, "DevTrackR.API.xml"); //DEVTRACK.API
    o.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
