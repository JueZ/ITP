using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Akanonda.GameLibrary
{
    [Serializable]
    public enum PlayerStatus
    {
        None,
        Playing,
        Watching,
        Dead,
        Offline
    }
}
