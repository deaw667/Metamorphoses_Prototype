using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class LevelMove_Ref : MonoBehaviour
{
    public int sceneBuildIndex;

    private void Start()
    {
        //GameDataManager.instance.LoadGame();
    }
 
    // Level move zoned enter, if collider is a player
    // Move game to another scene
    private void OnTriggerEnter2D(Collider2D other) 
    {
        print("Trigger Entered");
        
        // Could use other.GetComponent<Player>() to see if the game object has a Player component
        // Tags work too. Maybe some players have different script components?
        if(other.tag == "Player") {
            // Player entered, so move level
            print("Switching Scene to " + sceneBuildIndex);
            
            // Save game before loading new scene
            GameDataManager.instance.SaveGame();
            
            // Load new scene
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
            
            // Load game after scene has been loaded
            GameDataManager.instance.LoadGame();
            
            other.transform.position += new Vector3(-2, 0, 0);
        }
    }
}