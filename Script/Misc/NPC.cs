using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    public string interactionText = ""; // text to display when player is near
    public float interactionDistance = 1f; // distance from NPC that player must be within to interact
    public TextMeshProUGUI interactionTextUI; // reference to the TextMesh Pro component
    public GameObject textPanel; // reference to the text panel game object
    public int dialogId;

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
            interactionTextUI.text = interactionText; // set the text on the TextMesh Pro component
            textPanel.SetActive(true); // show the text panel
        }
        else
        {
            textPanel.SetActive(false); // hide the text panel
        }

        // check for F key press
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            InteractWithPlayer();
        }
    }

    void InteractWithPlayer()
    {
        DialogSystem.instance.StartDialog(dialogId);
        // code to handle interaction with player goes here
        Debug.Log("Player interacted with NPC!");
    }
}