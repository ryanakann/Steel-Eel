using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DashEvent();
public delegate void ActionEvent(Interactable i);

public class EelController : MonoBehaviour
{
    public static EelController instance;

    public Vector3 mouse_position { get { return GetPointerPosition(); } }
    public DashEvent DashEvent;
    public ActionEvent InteractEvent;

    Camera main;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE_WIN
        ComputerUpdate();
#endif

#if UNITY_STANDALONE_OSX
        ComputerUpdate();
#endif

#if UNITY_ANDROID
        MobileUpdate();
#endif

#if UNITY_IPHONE
        MobileUpdate();
#endif
    }

    void ComputerUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireEvent();
        }
    }

    void MobileUpdate()
    {
        if (Input.touchCount > 0)
        {
            FireEvent();
        }
    }

    void FireEvent()
    {
        Interactable i = Interaction();
        if (i != null)
        {
            InteractEvent(i);
        }
        else
        {
            DashEvent();
        }
    }

    Interactable Interaction()
    {
        RaycastHit2D hit = Physics2D.Raycast(main.ScreenToWorldPoint(mouse_position), Vector2.zero);
        print(hit.point);
        if (hit.collider != null)
        {
            print("We got one");
            return GetComponent<Interactable>();
        }
        print("nah, fam");
        return null;
    }

    Vector2 GetPointerPosition()
    {
#if UNITY_STANDALONE_WIN
        return Input.mousePosition;
#endif

#if UNITY_STANDALONE_OSX
        return Input.mousePosition;
#endif

#if UNITY_ANDROID
        if (Input.touchCount > 0)
            return Input.GetTouch(0).position;
        else return new Vector3(0, 0, -500);
#endif

#if UNITY_IPHONE
        if (Input.touchCount > 0)
            return Input.GetTouch(0).position;
        else return new Vector3(0, 0, -500);
#endif
    }
}
