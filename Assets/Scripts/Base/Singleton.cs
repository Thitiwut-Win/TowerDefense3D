using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                _instance = FindObjectOfType<T>();
#pragma warning restore CS0618 // Type or member is obsolete
                if (_instance == null)
                {
                    GameObject gameobject = new GameObject(typeof(T).Name);
                    _instance = gameobject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }
    }
}