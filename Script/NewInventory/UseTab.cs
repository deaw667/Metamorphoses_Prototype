using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseTab : MonoBehaviour
{
    private bool isTabActive = false;

    void Start()
    {
        isTabActive = false;
        gameObject.SetActive(false);
    }

    void Update()
    {
        //CheckOutsideclick();
    }

    private void CheckOutsideclick()
    {
        if (Input.GetMouseButtonDown(0) && !RectTransformUtility.RectangleContainsScreenPoint(gameObject.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
        {
            isTabActive = false;
            gameObject.SetActive(false);
        }
    }
}
