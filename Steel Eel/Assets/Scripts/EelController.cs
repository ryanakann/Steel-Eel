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
    int layermask;

    public bool can_input;

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

        FadeController.instance.FadeInStartedEvent += delegate { can_input = false; };
        FadeController.instance.FadeInCompletedEvent += delegate { can_input = true; };
        FadeController.instance.FadeOutStartedEvent += delegate { can_input = false; };

        can_input = true;

        layermask = 1 << 11;
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
        print(i);
        if (i != null)
        {
            InteractEvent?.Invoke(i);
        }
        else if (can_input)
        {
            DashEvent?.Invoke();
        }
    }

    Interactable Interaction()
    {
        RaycastHit2D hit = Physics2D.Raycast(main.ScreenToWorldPoint(GetPointerPosition(true)), Vector2.zero, 1f, layermask);

        if (hit)
        {
            return hit.collider.GetComponent<Interactable>();
        }
        return null;
    }

    Vector3 GetPointerPosition(bool pass = false)
    {
        if (!can_input && !pass)
            return new Vector3(0, 0, -500);

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
