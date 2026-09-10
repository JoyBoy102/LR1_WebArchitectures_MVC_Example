using LR1.Controllers;
using LR1.Interfaces;
using LR1.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;
namespace LR1.ApiTests
{
    public class ApiTests: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.Single(d => d.ServiceType == typeof(IUserService));
                    services.Remove(descriptor);
                    services.AddSingleton<IUserService, UserService>();
                });
            });
        }

        [Fact]
        public async Task GetUsers_ReturnsArrayOfUsers()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/users");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var users = await response.Content.ReadFromJsonAsync<List<User>>();
            Assert.NotNull(users);
            Assert.Equal(2, users!.Count);
        }

        [Fact]

        public async Task GetUser_ReturnsAlice()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/users/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var user = await response.Content.ReadFromJsonAsync<User>();
            Assert.NotNull(user);
            Assert.Equal("Alice", user!.Name);
            Assert.Equal("alice@example.com", user.Email);
        }

        [Fact]

        public async Task CreateUser_ReturnsCreated()
        {
            var client = _factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/users", new User { Name = "Charlie", Email = "charlie@example.com" });
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var created = await response.Content.ReadFromJsonAsync<User>();
            Assert.NotNull(created);
            Assert.Equal(3, created!.Id);
            Assert.Equal("Charlie", created.Name);
        }

        [Fact]

        public async Task UpdateUser_ReturnsUpdated()
        {
            var client = _factory.CreateClient();
            var updated = new { name = "Robert", email = "robert@example.com" };

            var response = await client.PutAsJsonAsync("/api/users/2", updated);

            var updatedUser = await response.Content.ReadFromJsonAsync<User>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(updatedUser);
            Assert.Equal("Robert", updatedUser!.Name);
        }

        [Fact]
        public async Task DeleteUser_RemovesAlice()
        {
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync("/api/users/1");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task CreateUser_WithoutName_ReturnsBadRequest()
        {
            var client = _factory.CreateClient();
            var bad = new { name = "", email = "x@example.com" };

            var response = await client.PostAsJsonAsync("/api/users", bad);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
