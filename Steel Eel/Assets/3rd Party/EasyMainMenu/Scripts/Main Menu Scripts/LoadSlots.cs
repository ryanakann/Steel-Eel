using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSlots : MonoBehaviour
{
    public List<GameObject> levels = new List<GameObject>(); 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ResetProgress()
    {
        Loader.instance.ResetProgress();
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].SetActive(i < (Loader.instance.lastLevel - 1));
        }
    }
}
