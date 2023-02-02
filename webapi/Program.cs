using webapi.Security;
using webapi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication("Api").AddScheme<ApiAuthenticationOptions, ApiAuthenticationHandler>("Api", null);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = string.Empty;
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "healthify api");
    });
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();