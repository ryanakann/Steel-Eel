﻿using System.Collections;
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

        foreach (Transform t in transform)
        {
            //t.gameObject.AddComponent<EelSlice>().body = this;
            children.Add(t.gameObject);
        }

        lr.positionCount = children.Count;
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
}
