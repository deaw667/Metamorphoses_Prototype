using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LootItemDrop : MonoBehaviour
{
    public float interactionDistance = 1f; // distance from NPC that player must be within to interact
    public GameObject textPanel; // reference to the text panel game object
    public Item itemloot;
    private bool isPlayerNear = false; // flag to track if player is near

    void Update()
    {
        // check if player is near
        float distanceToPlayer = Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        if (distanceToPlayer <= interactionDistance)
        {
            isPlayerNear = true;
        }
        else
        {
            isPlayerNear = false;
        }

        // show interaction text if player is near
        if (isPlayerNear)
        {
            textPanel.SetActive(true); // show the text panel
        }
        else
        {
            textPanel.SetActive(false); // hide the text panel
        }

        // check for F key press
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            InteractWithPlayer();
        }
    }

    void InteractWithPlayer()
    {
        // code to handle interaction with player goes here
        Inventory.instance.AddItem(Instantiate(itemloot));
        Destroy(gameObject);
    }
}
