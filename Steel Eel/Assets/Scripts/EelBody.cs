using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EelBody : MonoBehaviour
{
    EelMovement mover;
    LineRenderer lr;

    List<GameObject> children = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    void Setup()
    {
        mover = GetComponentInChildren<EelMovement>();
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;

        foreach (Transform t in transform)
        {
            t.gameObject.AddComponent<EelSlice>().body = this;
            children.Add(t.gameObject);
        }

        lr.positionCount = children.Count;

        EelController.instance.InteractEvent += Interact;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLine();
    }

    void UpdateLine()
    {
        for (int i = 0; i < children.Count; i++)
        {
            lr.SetPosition(i, children[i].transform.position);
        }
    }

    void Interact(Interactable i)
    {
        print("Interaaaact");
    }

    #region COLLISION
    public void TriggerEnter(Collider2D collider)
    {
       
    }

    public void TriggerExit(Collider2D collider)
    {

    }

    public void CollisionEnter(Collision2D collision)
    {

    }

    public void CollisionExit(Collision2D collision)
    {

    }
    #endregion
}
