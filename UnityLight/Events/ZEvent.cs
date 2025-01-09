using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Events
{
    public class ZEvent
    {
        public const string CLICK = "Click";
        public const string RIGHT_CLICK = "RightClick";
        public const string DOUBLE_CLICK = "DoubleClick";
        public const string RIGHT_DOUBLE_CLICK = "RightDoubleClick";
        public const string MOUSE_UP = "MouseUp";
        public const string MOUSE_DOWN = "MouseDown";
        public const string MOUSE_OVER = "MouseOver";
        public const string MOUSE_MOVE = "MouseMove";
        public const string MOUSE_OUT = "MouseOut";
        public const string MOUSE_ENTER = "MouseEnter";
        public const string MOUSE_LEAVE = "MouseLeave";
        public const string RIGHT_MOUSE_UP = "RightMouseUp";
        public const string RIGHT_MOUSE_DOWN = "RightMouseDown";
        public const string ROLL_OVER = "RollOver";
        public const string ROLL_OUT = "RollOut";
        public const string ACTIVE = "Active";
        public const string DEACTIVE = "Deactive";
        public const string SUBMIT = "Submit";
        public const string SELECTED_CHANGE = "SelectedChange";

        public const string CHANGED = "Changed";
        public const string COMPLETE = "Complete";

        //public string Type { get; set; }
        public object Target { get; set; }
        public object CurrentTarget { get; set; }
        public ZEvent()
        {
        }
    }
}
