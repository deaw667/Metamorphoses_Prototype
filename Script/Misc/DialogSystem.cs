using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.Events;

[System.Serializable]
public class DialogNode
{
    public string dialogText;
    public int id;
    public DialogChoice[] choices;
    public string talkerName;
    public int talkerIconId;
}

[System.Serializable]
public class DialogChoice
{
    public string choiceText;
    public int nextNodeId;
    public UnityEvent onChoose;
}

public class DialogSystem : MonoBehaviour
{

    #region singleton
    public static DialogSystem instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    public Text dialogTextUI;
    public Button[] choiceButtons;
    public Button endDialogButton;
    public GameObject dialogPanel;
    public GameObject TalkerPanel;
    public Image talkerIconUI;
    public Text talkerNameUI;
    public Sprite[] talkerIcons;

    public List<DialogNode> dialogs = new List<DialogNode>();

    private int currentNodeIndex = 0;

    void Start()
    {
        // Initialize the dialog system
        dialogPanel.SetActive(false);
        endDialogButton.onClick.AddListener(EndDialog);
        foreach (Button button in choiceButtons)
        {
            button.onClick.AddListener(() => OnChoiceSelected(button));
        }
        //StartDialog(0);
    }

    public void StartDialog(int dialogIndex)
    {
        currentNodeIndex = dialogIndex;
        dialogPanel.SetActive(true);
        DisplayDialog();
    }

    private void EndDialog()
    {
        dialogPanel.SetActive(false);
    }

    private void DisplayDialog()
    {
        DialogNode currentNode = dialogs[currentNodeIndex];
        talkerNameUI.text = currentNode.talkerName; // Set the talker name
        talkerIconUI.sprite = talkerIcons[currentNode.talkerIconId]; // Set the talker icon

        // Toggle the TalkerPanel based on the talker name
        TalkerPanel.SetActive(!string.IsNullOrEmpty(currentNode.talkerName));

        bool hasChoices = currentNode.choices.Length > 0;
        bool hasNextNode = currentNode.choices.Any(choice => choice.nextNodeId != 0);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < currentNode.choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<Text>().text = currentNode.choices[i].choiceText;
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }

        endDialogButton.gameObject.SetActive(!hasChoices && !hasNextNode);

        // Start the coroutine to display the dialog text one character at a time
        StartCoroutine(DisplayDialogText(currentNode.dialogText));
    }

    private IEnumerator<float> DisplayDialogText(string text)
    {
        dialogTextUI.text = "";
        foreach (char c in text)
        {
            dialogTextUI.text += c;
            yield return 0.05f;
        }
    }

    private void OnChoiceSelected(Button button)
    {
        int choiceIndex = Array.IndexOf(choiceButtons, button);
        DialogNode currentNode = dialogs[currentNodeIndex];
        if (choiceIndex < currentNode.choices.Length)
        {
            int nextNodeId = currentNode.choices[choiceIndex].nextNodeId;
            currentNodeIndex = dialogs.FindIndex(node => node.id == nextNodeId);
            DisplayDialog();

            // Call the onChoose event
            currentNode.choices[choiceIndex].onChoose.Invoke();
        }
        else
        {
            EndDialog();
        }
    }
}