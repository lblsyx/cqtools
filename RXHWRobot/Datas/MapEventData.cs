using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Exts;

namespace RXHWRobot.Datas
{
    public class MapEventData
    {
        /**
         * 数据ID
         */
        public int id;
        /**
         * 事件ID
         */
        public int eventID;
        /**
         * 事件名称
         */
        public string eventName = string.Empty;
        /**
         * 事件参数
         */
        public string paramStr = string.Empty;
        /**
         * 事件实例说明/注释
         */
        public string summary = string.Empty;
        /**
         * 事件点格子坐标(Point对象)列表
         */
        public List<Point> gridList = new List<Point>();

        public MapEventData clone()
        {
            MapEventData med = new MapEventData();
            med.id = id;
            med.paramStr = paramStr;
            med.eventID = eventID;
            med.eventName = eventName;
            foreach (Point point in gridList)
            {
                med.gridList.Add(point.clone());
            }
            return med;
        }

        public void addCellXY(int cx, int cy)
        {
            if (hasCellXY(cx, cy) == false)
            {
                gridList.Add(new Point(cx, cy));
            }
        }

        public void removeCellXY(int cx, int cy)
        {
            for (int i = 0; i < gridList.Count; i++)
            {
                Point point = gridList[i];
                if (point.X == cx && point.Y == cy)
                {
                    gridList.RemoveAt(i);
                    return;
                }
            }
        }

        public bool hasCellXY(int cx, int cy)
        {
            foreach (Point point in gridList)
            {
                if (point.X == cx && point.Y == cy)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
