using UserService.Dtos;

namespace UserService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void CreateNewUser(UserCreatedDto userCreatedDto);
    }
}