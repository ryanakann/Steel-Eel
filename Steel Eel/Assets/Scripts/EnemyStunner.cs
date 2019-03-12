using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunner : MonoBehaviour, Interactable
{
    private int playerTouches;

    private float interactionCD, CD_timer;

    EnemyController enemy;

    public GameObject circle;

    private void Start()
    {
        enemy = GetComponentInParent<EnemyController>();
        interactionCD = enemy.stunDuration;

        circle = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTouches > 0)
        {
            circle.SetActive(true);
        }
        else
        {
            circle.SetActive(false);
        }

        if (CD_timer > 0)
        {
            CD_timer -= Time.deltaTime;
        }
    }

    public void Interact()
    {
        if (CD_timer <= 0 && playerTouches > 0)
        {
            CD_timer = interactionCD;
            enemy.Stun();
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
