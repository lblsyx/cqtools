using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.ZScene
{
    public class RelevanceBehaviour : MonoBehaviour
    {
        public GameObject Instance { get; set; }

        public GameObject MainObject { get; set; }

        public ZSceneObject SceneObject { get; set; }

        public bool UpdateSceneObject;

        public void Update()
        {
            if (UpdateSceneObject && SceneObject != null) SceneObject.Update();
        }
    }
}
