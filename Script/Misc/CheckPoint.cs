using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("That a Item!!");
        if (other.gameObject.CompareTag("Player"))
        {
            TutorialSystem.instance.PlayerWalked();
            Destroy(gameObject);
        }
    }
}
