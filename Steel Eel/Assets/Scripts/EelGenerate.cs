using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelGenerate : MonoBehaviour
{

    public float radius = -1;
    private float offset;

    public int segments = 5;

    private GameObject last_slice;

    // Start is called before the first frame update
    void Start()
    {
        GameObject child = transform.GetChild(0).gameObject;

        if (radius == -1)
        {
            radius = child.GetComponent<CircleCollider2D>().radius;
        }

        radius *= 2;
        offset = radius;
        last_slice = child;

        while (segments > 0)
        {
            GenerateSlice(child);
            segments--;
        }

        child.AddComponent<EelMovement>();
    }

    void GenerateSlice(GameObject child)
    {
        GameObject slice = Instantiate(child, transform);
        slice.transform.Translate(new Vector2(0, -offset));

        last_slice.GetComponent<HingeJoint2D>().connectedBody = slice.GetComponent<Rigidbody2D>();
        slice.GetComponent<HingeJoint2D>().connectedBody = last_slice.GetComponent<Rigidbody2D>();

        last_slice = slice;
        offset += radius;
    }
}
