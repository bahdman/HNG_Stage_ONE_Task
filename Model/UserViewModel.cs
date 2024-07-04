using System.ComponentModel;

namespace src.Models{
    public class UserViewModel{
        [DisplayName("client_ip")]
        public string IP{get; set;}
        [DisplayName("location")]
        public string Location{get; set;}
        [DisplayName("greeting")]
        public string Greeting{get; set;}
    }
}