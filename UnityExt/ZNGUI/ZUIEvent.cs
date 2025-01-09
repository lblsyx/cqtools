using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Events;

namespace UnityExt.ZNGUI
{
    public class ZUIEvent : ZEvent
    {
        public const string UI_LOAD_START = "UILoadStart";

        public const string UI_LOAD_PROGRESS = "UILoadProgress";

        public const string UI_LOAD_DONE = "UILoadDone";

        public object Data1;
        public object Data2;
        public object Data3;
        public object Data4;
        public object Data5;

        public IZDialog UIDialog;

        public ZUIEvent(string type, IZDialog oDialog)
            : this(type, oDialog, null, null, null, null, null)
        {
        }

        public ZUIEvent(string type, IZDialog oDialog, object oData1)
            : this(type, oDialog, oData1, null, null, null, null)
        {
        }

        public ZUIEvent(string type, IZDialog oDialog, object oData1, object oData2)
            : this(type, oDialog, oData1, oData2, null, null, null)
        {
        }

        public ZUIEvent(string type, IZDialog oDialog, object oData1, object oData2, object oData3)
            : this(type, oDialog, oData1, oData2, oData3, null, null)
        {
        }

        public ZUIEvent(string type, IZDialog oDialog, object oData1, object oData2, object oData3, object oData4)
            : this(type, oDialog, oData1, oData2, oData3, oData4, null)
        {
        }

        public ZUIEvent(string type, IZDialog oDialog, object oData1, object oData2, object oData3, object oData4, object oData5)
            : base(type)
        {
            Data1 = oData1;
            Data2 = oData2;
            Data3 = oData3;
            Data4 = oData4;
            Data5 = oData5;
            UIDialog = oDialog;
        }
    }
}
