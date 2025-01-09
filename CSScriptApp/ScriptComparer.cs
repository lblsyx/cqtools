using System;
using System.Collections.Generic;
using System.Text;

namespace CSScriptApp
{
    public class ScriptComparer : IComparer<IScript>
    {
        #region IComparer<IScript> 成员

        public int Compare(IScript x, IScript y)
        {
            if (x.ThreadGroupIndex < y.ThreadGroupIndex)
            {
                return -1;
            }
            else if (x.ThreadGroupIndex == y.ThreadGroupIndex)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        #endregion
    }
}
