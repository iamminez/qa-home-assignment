using CardValidation.Core.Services;
using CardValidation.Core.Services.Interfaces;
using CardValidation.Infrustructure;

var builder = WebApplication.CreateBuilder(args);

// ? Local function for service registration
void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // Register CardValidation service
    services.AddTransient<ICardValidationService, CardValidationService>();

    // Add MVC filters
    services.AddMvc(options =>
    {
        options.Filters.Add(typeof(CreditCardValidationFilter));
    });
}

// Call ConfigureServices
ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

// ? Required for integration tests
public partial class Program { }