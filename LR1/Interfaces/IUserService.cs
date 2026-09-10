namespace LR1.Interfaces
{
    public interface IUserService
    {
        IReadOnlyList<User> GetAll();
        User? GetById(int id);
        User Create(string name, string email);
        User? Update(int id, string name, string email);
        bool Delete(int id);
        void Reset();
    }
}
