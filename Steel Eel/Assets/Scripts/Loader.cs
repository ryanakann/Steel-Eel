using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public static Loader instance;

    public int lastLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        lastLevel = PlayerPrefs.GetInt("Level", 1);
    }

    public void UnlockLevel(int level)
    {
        if (level > lastLevel)
        {
            lastLevel = level;
            PlayerPrefs.SetInt("Score", lastLevel);
        }
    }

    public void ResetProgress()
    {
        lastLevel = 1;
        PlayerPrefs.SetInt("Score", 1);
    }
}
