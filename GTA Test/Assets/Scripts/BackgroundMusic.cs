using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed when switching scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }
}
