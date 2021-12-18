using System.Collections.Generic;

namespace Movies.Client.Models
{
    public class UserInfoViewModel
    {
        public Dictionary<string, string> _userInfoDictionary { get; private set; } = null;

        public UserInfoViewModel(Dictionary<string, string> userInfoDictionary)
        {
            _userInfoDictionary = userInfoDictionary;
        }
    }
}
