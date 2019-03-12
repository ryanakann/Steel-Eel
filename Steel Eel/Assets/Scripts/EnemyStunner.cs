using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunner : MonoBehaviour, Interactable
{
    private int playerTouches;

    private float interactionCD, CD_timer;

    EnemyController enemy;

    public GameObject circle;

    bool color_flipped;

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
            if (CD_timer <= 0 && !color_flipped)
            {
                color_flipped = true;
                circle.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 0.5f);
            }
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
            color_flipped = false;
            circle.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
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
