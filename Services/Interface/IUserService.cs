using Elibrary.Dtos;

namespace Elibrary.Services.Interface
{

    public interface IUserService
    {

        void AddUser(InsertUserDto userDto);
    }

}