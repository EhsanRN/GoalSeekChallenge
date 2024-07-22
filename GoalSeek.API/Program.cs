using GoalSeek.API.Repositories;

namespace GoalSeek.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                            .AddJsonOptions(options =>
                                {
                                    options.JsonSerializerOptions.WriteIndented = true;
                                })
                            .ConfigureApiBehaviorOptions(options =>
                                {
                                    options.SuppressModelStateInvalidFilter = true;
                                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IGoalSeekRepository, GoalSeekRepository>(p =>
            {
                var logger = p.GetRequiredService<ILogger<GoalSeekRepository>>();
                return new GoalSeekRepository(logger);
            });

            var app = builder.Build();

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
    }
}
