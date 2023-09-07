using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XspenseCSharp
{
    public struct UserLoginDataContainer
    {
        public List<UserLoginData> data { get; set; }
    }
    public struct UserLoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserUUID { get; set; }
    }
    class CoreDataManager
    {
        public static CoreDataManager shared = new CoreDataManager();
        public UserLoginDataContainer ReadUserDataFromJsonString(string jsonString)
        {
            var myData = JsonConvert.DeserializeObject<UserLoginDataContainer>(jsonString);
            return myData;
        }
        public string WriteUserToJsonData(UserLoginDataContainer myData)
        {
            var jsonString = JsonConvert.SerializeObject(myData);
            return jsonString;
        }
    }
}
