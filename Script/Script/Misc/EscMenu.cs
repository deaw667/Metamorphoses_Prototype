using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscMenu : MonoBehaviour
{
    public GameObject Esctab;

    private void Start()
    {
        // Initially hide the Esctab
        Esctab.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the visibility of Esctab when Esc key is pressed
            Esctab.SetActive(!Esctab.activeSelf);
        }
    }

    public void HideEscMenu()
    {
        Esctab.SetActive(false);
    }
}