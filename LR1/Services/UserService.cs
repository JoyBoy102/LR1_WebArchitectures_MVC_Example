using LR1.Interfaces;

namespace LR1.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users;
        private int _nextId;

        public UserService()
        {
            _users = new List<User>
            {
                new() { Id = 1, Name = "Alice", Email = "alice@example.com" },
                new() { Id = 2, Name = "Bob",   Email = "bob@example.com" }
            };
            _nextId = 3;
        }

        public IReadOnlyList<User> GetAll() => _users;

        public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);

        public User Create(string name, string email)
        {
            var user = new User { Id = _nextId++, Name = name, Email = email };
            _users.Add(user);
            return user;
        }

        public User? Update(int id, string name, string email)
        {
            var user = GetById(id);
            if (user is null) return null;

            user.Name = name;
            user.Email = email;
            return user;
        }

        public bool Delete(int id)
        {
            var user = GetById(id);
            if (user is null) return false;

            _users.Remove(user);
            return true;
        }

        public void Reset()
        {
            _users.Clear();
            _users.Add(new User { Id = 1, Name = "Alice", Email = "alice@example.com" });
            _users.Add(new User { Id = 2, Name = "Bob", Email = "bob@example.com" });
            _nextId = 3;
        }
    }
}
