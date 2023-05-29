using UserTests.Models.Requests;

namespace UserTests.Utils
{
    public class UserGenerator
    {

        public GetAllUsersRequests GetAllUsersRequests(int pageNumber, RegisterUserRequest filter)
        {
            return new GetAllUsersRequests()
            {
                PageNumber = pageNumber,
                Filter = filter,
            };
        }


        public RegisterUserRequest GenerateRegisterUserRequest(string name, string surname)
        {
            return new RegisterUserRequest()
            {
                Name = name,
                Surname = surname,
            };
        }

        public string GetRandomString(int size)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, size).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string GetRandomDigits(int size)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, size).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string GetRandomSpecialChars (int size)
        {
            Random random = new Random();
            const string chars = "_!'^+%&/()=?_*-+,,:;|<>,";
            return new string(Enumerable.Repeat(chars, size).Select(s => s[random.Next(s.Length)]).ToArray());
        }


    }
}
