using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExt;
using UnityExt.Loaders;
using UnityLight.Loggers;
using UnityLight.Pools;

namespace UnityExt.ZScene
{
    public class ZSceneObject : IPoolItem, IDisposable
    {
        private LoaderItem mLoaderItem;
        private string mAniName;
        private float mAniSpeed;
        private WrapMode mWrapMode;
        private float mAniTime;
        private int mAniPlayCount;
        private string mQueueAniName;

        public string LoadDoneShaderName { get; set; }

        public bool Destoried { get; protected set; }
        public string AssetURL { get; protected set; }
        public GameObject Instance { get; protected set; }
        public GameObject MainObject { get; set; }
        public GameObject ParentObject { get; protected set; }
        public Animation Animation { get; protected set; }
        public Animator Animator { get; protected set; }

        public ZSceneObject()
        {
            mAniPlayCount = 0;
            MainObject = new GameObject();
            RelevanceBehaviour Relevance = MainObject.AddComponent<RelevanceBehaviour>();
            Relevance.Instance = null;
            Relevance.MainObject = MainObject;
            Relevance.UpdateSceneObject = true;
            Relevance.SceneObject = this;

            Reset();
        }

        public virtual void Reset()
        {
            Destoried = false;
            ParentObject = null;
            Animation = null;
            AssetURL = null;
            Animator = null;

            if (ObjectDownloadQueue != null)
            {
                ObjectDownloadQueue.Cancel(mLoaderItem);
            }
            mLoaderItem = null;

            ExtUtil.DestroyImmediate(Instance);
            Instance = null;

            if (ZSceneMgr.SceneObjectPoolRoot != null)
            {
                MainObject.transform.parent = ZSceneMgr.SceneObjectPoolRoot.transform;
            }
            else
            {
                MainObject.transform.parent = null;
            }
            MainObject.transform.localPosition = Vector3.zero;
            MainObject.transform.localEulerAngles = Vector3.zero;
            MainObject.transform.localScale = Vector3.one;
            //MainObject.hideFlags = HideFlags.HideInHierarchy;
        }

        public virtual DownloadQueue ObjectDownloadQueue
        {
            get
            {
                XLogger.ErrorFormat("需要重写ObjectDownloadQueue属性提供下载队列！");
                throw new NotImplementedException();
            }
        }

        public Transform transform { get { return MainObject.transform; } }

        public virtual Vector3 Position3
        {
            get
            {
                return MainObject.transform.position;
            }
            set
            {
                MainObject.transform.position = value;
            }
        }

        public void SetParent(GameObject parentObject, bool bResetTrans = true)
        {
            ParentObject = parentObject;
            if (ParentObject == null)
            {
                MainObject.transform.parent = null;
            }
            else
            {
                MainObject.layer = ParentObject.layer;
                MainObject.transform.parent = ParentObject.transform;
                if (bResetTrans)
                {
                    MainObject.transform.localEulerAngles = Vector3.zero;
                    MainObject.transform.localPosition = Vector3.zero;
                    MainObject.transform.localScale = Vector3.one;
                }
                ExtUtil.ChangeGameObjectLayer(MainObject, ParentObject.layer);
            }
        }

        public void LoadAsset(string path)
        {
            if (AssetURL == path) return;
            AssetURL = path;
            
            if (mLoaderItem != null) ObjectDownloadQueue.Cancel(mLoaderItem);
            mLoaderItem = null;
            if (string.IsNullOrEmpty(AssetURL)) return;
            mLoaderItem = ObjectDownloadQueue.Load(AssetURL, OnLoadDoneHandler);
        }

        public void SetAsset(AssetBundle bundle)
        {
            if (bundle != null) SetAsset(bundle.mainAsset);
        }

        public void SetAsset(UnityEngine.Object cls)
        {
            OnPreSetAsset(Instance, cls, false);

            if (Instance != null)
            {
                GameObject.DestroyImmediate(Instance);
                Animation = null;
                Animator = null;
                Instance = null;
            }

            RelevanceBehaviour Relevance;
            if (cls != null)
            {
                Instance = ExtUtil.InstantiateBundleAsset(cls);
                Instance.transform.parent = MainObject.transform;
                Instance.transform.localPosition = Vector3.zero;
                Instance.transform.localEulerAngles = Vector3.zero;
                Instance.transform.localScale = Vector3.one;
                Instance.name = Instance.name.Replace("(Clone)", string.Empty);

                Relevance = Instance.AddComponent<RelevanceBehaviour>();
                Relevance.Instance = Instance;
                Relevance.MainObject = MainObject;
                Relevance.SceneObject = this;
                Relevance.UpdateSceneObject = false;

                Animation = Instance.GetComponent<Animation>();
                Animator = Instance.GetComponent<Animator>();
            }

            Relevance = MainObject.GetComponent<RelevanceBehaviour>();
            if (Relevance != null) Relevance.Instance = Instance;

            if (ParentObject != null) ExtUtil.ChangeGameObjectLayer(MainObject, ParentObject.layer);

            if (string.IsNullOrEmpty(LoadDoneShaderName) == false)
            {
                //ExtUtil.ChangeGameObjectShader(MainObject, LoadDoneShaderName);
                ExtUtil.ChangeGameObjectShader(MainObject);
            }

            OnAfterSetAsset(Instance, cls, false);
        }

        public void SetInstance(GameObject ins, bool bDestory)
        {
            OnPreSetAsset(Instance, ins, true);

            if (Instance != null && bDestory)
            {
                GameObject.DestroyImmediate(Instance);
            }
            Animation = null;
            Animator = null;
            Instance = ins;

            RelevanceBehaviour Relevance;
            if (Instance != null)
            {
                Instance.transform.parent = MainObject.transform;
                Instance.transform.localPosition = Vector3.zero;
                Instance.transform.localEulerAngles = Vector3.zero;
                Instance.transform.localScale = Vector3.one;
                Instance.name = Instance.name.Replace("(Clone)", string.Empty);

                Relevance = Instance.GetComponent<RelevanceBehaviour>();
                if (Relevance == null) Relevance = Instance.AddComponent<RelevanceBehaviour>();
                Relevance.Instance = Instance;
                Relevance.MainObject = MainObject;
                Relevance.SceneObject = this;

                Animation = Instance.GetComponent<Animation>();
                Animator = Instance.GetComponent<Animator>();
            }

            Relevance = MainObject.GetComponent<RelevanceBehaviour>();
            if (Relevance != null) Relevance.Instance = Instance;

            if (ParentObject != null) ExtUtil.ChangeGameObjectLayer(MainObject, ParentObject.layer);

            OnAfterSetAsset(Instance, ins, true);
        }

        protected virtual void OnPreSetAsset(GameObject oldIns, UnityEngine.Object cls, bool bIns)
        {
        }

        protected virtual void OnAfterSetAsset(GameObject newIns, UnityEngine.Object cls, bool bIns)
        {
        }

        public bool ContainsAni(string aniName)
        {
            if (Animation == null && Animator == null) return false;

            if (Animation != null)
            {
                AnimationClip oAnimationClip = Animation.GetClip(aniName);
                if (oAnimationClip == null) return false;
                return true;
            }
            else
            {
                AnimatorOverrideController ctrl = Animator.runtimeAnimatorController as AnimatorOverrideController;
                AnimationClip oAnimationClip = ctrl[aniName];
                if (oAnimationClip == null) return false;
                return true;
            }
        }

        public bool Playing
        {
            get
            {
                if (Animation == null && Animator == null) return false;
                if (Animation != null)
                    return Animation.isPlaying;
                else
                {
                    return true;
                }
            }
        }

        public bool IsPlaying(string name)
        {
            if (Animation == null && Animator == null) return false;
            if (Animation != null)
                return Animation.IsPlaying(name);
            else
            {
                bool bl = Animator.GetCurrentAnimatorStateInfo(0).IsName(name);
                return bl;
            }
        }

        public float GetAnimationLength(string aniName)
        {
            if (Animation == null && Animator == null) return 0;
            if (Animation != null)
            {
                AnimationClip oAnimationClip = Animation.GetClip(aniName);
                if (oAnimationClip == null) return 0;
                return oAnimationClip.length;
            }
            else
            {
                AnimatorOverrideController ctrl = Animator.runtimeAnimatorController as AnimatorOverrideController;
                AnimationClip oAnimationClip = ctrl[aniName];
                if (oAnimationClip == null) return 0;
                return oAnimationClip.length;
            }
        }

        public void Play(string aniName, float speed, WrapMode wrapMode)
        {
            Play(aniName, speed, wrapMode, 0f, null);
        }

        public void Play(string aniName, float speed, WrapMode wrapMode, float time)
        {
            Play(aniName, speed, wrapMode, time, null);
        }

        public void Play(string aniName, float speed, WrapMode wrapMode, string queuedAniName)
        {
            Play(aniName, speed, wrapMode, 0f, queuedAniName);
        }

        public void Play(string aniName, float speed, WrapMode wrapMode, float time, string queuedAniName)
        {
            mAniPlayCount = 0;
            mAniName = aniName;
            mAniSpeed = speed;
            mWrapMode = wrapMode;
            mAniTime = time;
            mQueueAniName = queuedAniName;
            if (Animation == null && Animator == null) return;
            if (Animation != null)
            {
                AnimationState aniState = Animation[aniName];
                if (aniState == null) return;

                aniState.speed = speed;
                aniState.wrapMode = wrapMode;
                aniState.normalizedTime = time;
                Animation.Play(aniName);
                //XLogger.DebugFormat("播放{0}动作", aniName);
                if (string.IsNullOrEmpty(queuedAniName) == false) Animation.PlayQueued(queuedAniName);
            }
            else
            {
                Animator.speed = speed;
                Animator.Play(Animator.StringToHash(aniName), 0, time);
            }
        }

        public void CrossFade(string aniName, float speed, WrapMode wrapMode)
        {
            CrossFade(aniName, speed, wrapMode, null);
        }

        public void CrossFade(string aniName, float speed, WrapMode wrapMode, string queuedAniName)
        {
            mAniTime = 0;
            mAniPlayCount = 0;
            mAniName = aniName;
            mAniSpeed = speed;
            mWrapMode = wrapMode;
            mQueueAniName = queuedAniName;

            if (Animation == null && Animator == null) return;

            if (Animation != null)
            {
                AnimationState aniState = Animation[aniName];
                if (aniState == null) return;

                aniState.speed = speed;

                aniState.wrapMode = wrapMode;
                Animation.CrossFade(aniName);
                //XLogger.DebugFormat("播放{0}动作", aniName);
                if (string.IsNullOrEmpty(queuedAniName) == false) Animation.CrossFadeQueued(queuedAniName);
            }
            else
            {
                Animator.speed = speed;
                Animator.CrossFade(Animator.StringToHash(aniName), 0.2f, 0);
            }
        }

        private void OnLoadDoneHandler(URLLoader loader, bool success, string errMsg)
        {
            if (success)
            {
                if (mLoaderItem != null && mLoaderItem.URL == loader.URL && loader.DataBundle != null) SetAsset(loader.DataBundle.mainAsset);
                else XLogger.ErrorFormat("场景对象加载地址不符，未设置显示对象资源!");
            }
            else
            {
                XLogger.ErrorFormat("场景对象加载失败!{0}", errMsg);
            }

            Play(mAniName, mAniSpeed, mWrapMode, mAniTime, mQueueAniName);

            OnAssetLoadDone(loader, success);

            mLoaderItem = null;
        }

        protected virtual void OnAssetLoadDone(URLLoader loader, bool success)
        {
        }

        public virtual void Update()
        {
            if (Animation == null) return;
            AnimationState aniState = Animation[mAniName];
            if (aniState == null) return;
            int temp = (int)(aniState.time / aniState.length);
            if (temp != mAniPlayCount)
            {
                mAniPlayCount = temp;
                OnAnimationAgain(mAniName);
            }
        }

        protected virtual void OnAnimationAgain(string sAniName)
        {

        }

        public virtual void Dispose()
        {
            if (ObjectDownloadQueue != null) ObjectDownloadQueue.Cancel(mLoaderItem);
            ExtUtil.DestroyImmediate(MainObject);
            ParentObject = null;
            mLoaderItem = null;
            Animation = null;
            Instance = null;
            Destoried = true;
        }
    }
}
