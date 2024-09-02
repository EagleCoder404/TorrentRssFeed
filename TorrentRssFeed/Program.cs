using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TorrentRssFeed.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services
	.AddIdentityApiEndpoints<AppUser>()
	.AddEntityFrameworkStores<TorrentRssFeedDbContext>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TorrentRssFeedDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

var app = builder.Build();

app.MapIdentityApi<AppUser>();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

//if (app.Environment.IsDevelopment())
//{
//	app.UseSwagger();
//	app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
