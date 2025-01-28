using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class BackGroundMusic : MonoBehaviour
{
    
    public static BackGroundMusic bgmusic;

    private void Awake()
    {
        if (bgmusic != null) // if bgmusic gets assigned a valid value, destroy the game object using bgmusic.
        {
            Destroy(gameObject);
        }
        else  // if bgmusic is not assigned a valid value, do not destroy the game object using bgmusic. 
        {
            bgmusic = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

}
