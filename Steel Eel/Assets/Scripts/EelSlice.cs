using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelSlice : MonoBehaviour
{
    public EelBody body;

    // Start is called before the first frame update
    void Start()
    {
        if (!body)
            body = GetComponentInParent<EelBody>();
    }

    #region COLLISIONS
    private void OnTriggerEnter2D(Collider2D collider)
    {
        body.TriggerEnter(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        body.TriggerExit(collider);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        body.CollisionEnter(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        body.CollisionExit(collision);
    }
    #endregion
}
