using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Data
{
    public class Group:FingerPrintEntityBase
    {
#nullable disable //Null check suppression
        public string Name { get; set; }
        public IList<UserGroup> UserGroups { get; set; }
#nullable enable

    }
}
