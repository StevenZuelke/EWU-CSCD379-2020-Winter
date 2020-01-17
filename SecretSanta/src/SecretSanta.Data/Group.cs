using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Data
{
    public class Group:FingerPrintEntityBase
    {
#nullable disable
        public string Name { get; set; }
#nullable enable
        public IList<UserGroup> UserGroups { get; set; }
    }
}
