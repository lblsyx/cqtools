using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Exts;

namespace UnityExt.Chunks.MapChunks
{
    public class EventData
    {
        // 事件类型
        public int EventType;
        // 事件ID
        public int EventID;
        // 格子列表
        public Point[] GridList;
        // 事件名字
        public string EventName;
        // 事件参数 （当前事件对应的触发行为）
        public string EventParams;
        // 
        public string EventSummary;
    }

}
