using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Events;

namespace UnityExt.ZScene
{
    public class ZSceneEvent : ZEvent
    {
        public const string SCENE_CHANGED = "SceneChanged";
        public const string SCENE_CHANGING = "SceneChanging";

        public ZSceneEvent() : base()
        {
        }
    }
}
