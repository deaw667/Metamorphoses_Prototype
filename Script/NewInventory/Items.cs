using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite sprite;

    [TextArea]

    [SerializeField]
    private string itemDescription;

    private InventoryManager inventoryManager;
    public Item itemdropped;

    public float speed = 5f; // speed of the item
    private Transform target; // the player's transform

    // Start is called before the first frame update
    void Start()
    {
        // find the player's transform
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // move towards the player
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target.position, step);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("That a Item!!");
        if (other.gameObject.CompareTag("Player"))
        {
            Inventory.instance.AddItem(Instantiate(itemdropped));
            Destroy(gameObject);
        }
    }
}