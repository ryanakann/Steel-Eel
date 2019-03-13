using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

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

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadLevel(int i)
    {
        SceneManager.LoadScene(i);
        if (i > 0)
        {
            GameManager.instance.StartGame();
        }
    }

    public void LoadMenu()
    {
        FadeController.instance.FadeIn();
        SceneManager.LoadScene(0);
    }

    public int GetLevel()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
