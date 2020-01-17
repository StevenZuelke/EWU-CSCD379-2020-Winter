using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Data
{
    //Inherit from FingerPrintEntityBase
    public class User:FingerPrintEntityBase
    {
        public string FirstName { get => _FirstName; set => _FirstName = value ?? throw new ArgumentNullException(nameof(FirstName)); }
        private string _FirstName = string.Empty;
        public string LastName { get => _LastName; set => _LastName = value ?? throw new ArgumentNullException(nameof(LastName)); }
        private string _LastName = string.Empty;
        public IList<UserGroup> UserGroups { get; set; }
        //Gifts read and write
        public ICollection<Gift> Gifts { get; set; }
        //Nullable Santa Property
        public User? Santa { get; set; }
    }
}
