using System;
using System.Collections.Generic;
using System.Text;

namespace GarticPhone.Models
{
    public class User
    {
        public string Username { get; set; }

        public string ImageSource { get; set; }

        public User(string username, string imageSource)
        {
            Username = username;
            ImageSource = imageSource;
        }
    }
}
