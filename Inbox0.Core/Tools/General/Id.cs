using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inbox0.Core.Tools.General
{
    public static class Id
    {
        public static string New { get => Guid.NewGuid().ToString(); }
    }
}
