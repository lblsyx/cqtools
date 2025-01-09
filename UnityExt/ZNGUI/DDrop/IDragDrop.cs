using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityExt.ZNGUI.Interfaces;

namespace UnityExt.ZNGUI.DDrop
{
    public interface IDragDrop
    {
        string GroupName { get; }

        bool ClickDrag { get; }

        bool ManualDrop { get; }

        bool LockCenter { get; }

        bool ReviseInStage { get; }

        bool ShowAtOnce { get; }

        bool OnlyDragPos { get; }

        ZEventObject Trigger { get; }

        object DragData { get; }

        bool UpdateDragIcon(IZSprite sprite);

        void OnGroupDragCallback(IDragDrop iDragger, object oDragData);

        void OnGroupDropCallback(IDragDrop iDragger, object oDragData);

        void OnDragCallback();

        void OnDragingCallback();

        void OnDropHitCallback(IDragDrop iDragger, object oDragData);

        void OnDropCallback(IDragDrop iHitIDragDrop);
    }
}
