using UnityEngine;

// <summary>
// Static instance base class for MonoBehaviour singletons.

namespace Game.Utilities
{
    // This class provides a static instance of the MonoBehaviour, allowing easy access to it.
    // It also handles the destruction of the instance on application quit.
    public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            if (Instance != null)
            {
                Instance = null;
                Destroy(gameObject);
            }
        }
    }


    // <summary>
    // Singleton base class for MonoBehaviour singletons that ensures only one instance exists in the scene.
    public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            base.Awake();
        }
    }

    // <summary>
    // Singleton base class for MonoBehaviour singletons that persists across scene loads.
    public abstract class DontDestroySingleton<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }

}