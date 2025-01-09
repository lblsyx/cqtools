using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight;

namespace UnityExt.ZNGUI.Interfaces
{
    public interface IZUISpriteAnimation : IZUIObject
    {
        object CallbackArg { get; set; }

        Callback<object> OnCallback { get; set; }

        int Fps{ get ; set; }

        int Frames { get; }

        string NamePrefix { get; set; }

        bool Loop { get; set; }

        bool IsPlaying { get; }

        void Play();

        void Pause();

        void ResetToBeginning();

        void RebuildSpriteList();
    }
}
