using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.AllJoyn;
using Newtonsoft.Json;
using PropertyChanged;

namespace ReduxDotNet.Model
{
    public class User
    {
        private UserProfileStates _profileStates;

        public UserProfileStates ProfileStates
        {
            get { return _profileStates; }
            set
            {
                _profileStates = value;
                ProfileState = value.ToString();
            }
        }
        public string ProfileState { get; set; }

        public User Clone()
        {
            return JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(this));
        }
        public User()
        {
            ProfileStates=UserProfileStates.New;
        }
    }
}
