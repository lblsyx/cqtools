using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityExt.ZNGUI.Interfaces
{
    public interface IZGroup : IZUIObject
    {
        int GroupID { get; }
        bool CanBeNone { get; set; }
        int SelectedIndex { get; set; }
        IZToggle SelectedItem { get; set; }
    }
}
