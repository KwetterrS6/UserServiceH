using System.Threading.Tasks;
using UserService.Dtos;

namespace UserService.SyncDataServices.Http
{
    public interface IKweetDataClient
    {
        Task SendUserToKweet(UserReadDto user); 
    }
}