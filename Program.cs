// using TodoApi;
// using Microsoft.EntityFrameworkCore;

// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<ToDoDbContext>(
//     options => options.UseMySql(
//         builder.Configuration.GetConnectionString("ToDoDB"),
//         ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ToDoDB"))));

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll", policy =>
//     {
//         policy.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader();
//     });
// });


// builder.Services.AddEndpointsApiExplorer();

// var app = builder.Build();

// app.UseCors("AllowAll");

// app.MapGet("/", () => "my apliction running");

// app.MapGet("/items", (ToDoDbContext db) => Results.Ok(db.Items.ToList()));

// app.MapPost("/items", (Item newItem, ToDoDbContext db) =>
// {
//     db.Items.Add(newItem);
//     db.SaveChanges();
//     return Results.Created($"/items/{newItem.Id}", newItem);
// });

// app.MapPut("/items/{id}", (int id, Item updatedItem, ToDoDbContext db) =>
// {
//     var existingItem = db.Items.Find(id);
//     if (existingItem == null)
//     {
//         return Results.NotFound();
//     }
//     if(updatedItem.Name!=null)
//         existingItem.Name = updatedItem.Name;
//     existingItem.IsComplete = updatedItem.IsComplete;
//     db.SaveChanges();
//     return Results.Ok(existingItem);
// });

// app.MapDelete("/items/{id}", (int id, ToDoDbContext db) =>
// {
//     var item = db.Items.Find(id);
//     if (item == null)
//     {
//         return Results.NotFound();
//     }
//     db.Items.Remove(item);
//     db.SaveChanges();
//     return Results.NoContent();
// });

// app.Run();



using TodoApi;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

// שמירת מחרוזת החיבור במשתנה כדי למנוע קריאה כפולה
// var connectionString = builder.Configuration.GetConnectionString("ToDoDB");
var connectionString = Environment.GetEnvironmentVariable("ToDoDB");
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


// builder.Services.AddDbContext<ToDoDbContext>(options =>
    // options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// הוספת Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");

// הפעלת Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "my application running");

app.MapGet("/items", (ToDoDbContext db) => Results.Ok(db.Items.ToList()));

app.MapPost("/items", (Item newItem, ToDoDbContext db) =>
{
    newItem.IsComplete=false;
    db.Items.Add(newItem);
    db.SaveChanges();
    return Results.Created($"/items/{newItem.Id}", newItem);
});

app.MapPut("/items/{id}", (int id, Item updatedItem, ToDoDbContext db) =>
{
    var existingItem = db.Items.Find(id);
    if (existingItem == null)
    {
        return Results.NotFound();
    }
    if (updatedItem.Name != null)
        existingItem.Name = updatedItem.Name;
    existingItem.IsComplete = updatedItem.IsComplete;
    db.SaveChanges();
    return Results.Ok(existingItem);
});

app.MapDelete("/items/{id}", (int id, ToDoDbContext db) =>
{
    var item = db.Items.Find(id);
    if (item == null)
    {
        return Results.NotFound();
    }
    db.Items.Remove(item);
    db.SaveChanges();
    return Results.NoContent();
});

app.Run();
