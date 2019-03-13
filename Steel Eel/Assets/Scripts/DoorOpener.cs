using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour, Interactable
{
    private int playerTouches;

    public GameObject circle, door;

    bool interacted;

    private void Start()
    {
        circle = transform.GetChild(0).gameObject;
        door = transform.parent.GetChild(1).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerTouches > 0 && !interacted)
        {
            circle.SetActive(true);
        }
        else
        {
            circle.SetActive(false);
        }
    }

    public void Interact()
    {
        if (playerTouches > 0 && !interacted)
        {
            circle.SetActive(false);
            interacted = true;
            door.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            playerTouches++;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            playerTouches--;
    }
}
