using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameKit : MonoBehaviour
{

    public static GameKit instance;
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
    }
}
