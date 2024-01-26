using UnityEngine;

namespace catHomerun.Utils
{
    public abstract class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
    {
        protected static bool isQuitting = false;
        protected static T _instance = null;

        protected bool isValidSingleton = false;

        protected virtual void OnApplicationQuit() => isQuitting = true;

        protected virtual void OnDestroy()
        {
            if (isValidSingleton)
                _instance = null;
        }
    }

    public abstract class SingletonSceneSelfInstancing<T> : SingletonBase<T> where T : SingletonSceneSelfInstancing<T>
    {
        /// <summary>
        /// The singleton instance of the SelfInstancingSingleton.
        /// If no instance exists, a new one will be created as a component of a new GameObject
        /// </summary>
        public static T instance
        {
            get
            {
                if (isQuitting)
                    return null;
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                    if (_instance == null)
                    {
                        var loader = new GameObject(typeof(T).FullName);
                        _instance = loader.AddComponent<T>();
                    }
                    _instance.isValidSingleton = true;
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = (T)this;
                isValidSingleton = true;
            }
            else if (_instance != this)
                Destroy(this);
        }
    }

    public abstract class SingletonGlobalSelfInstancing<T> : SingletonSceneSelfInstancing<T> where T : SingletonGlobalSelfInstancing<T>
    {
        protected override void Awake()
        {
            base.Awake();

            if (!isValidSingleton)
                return;

            DontDestroyOnLoad(this);
        }
    }

    public abstract class SingletonScene<T> : SingletonBase<T> where T : SingletonScene<T>
    {
        public static T instance
        {
            get
            {
                if (isQuitting)
                    return null;
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                    if (_instance == null)
                        return null;

                    _instance.isValidSingleton = true;
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = (T)this;
                isValidSingleton = true;
            }
            else if (_instance != this)
                Destroy(this);
        }
    }

    public abstract class SingletonGlobal<T> : SingletonScene<T> where T : SingletonGlobal<T>
    {
        protected override void Awake()
        {
            base.Awake();

            if (!isValidSingleton)
                return;

            DontDestroyOnLoad(this);
        }
    }
}