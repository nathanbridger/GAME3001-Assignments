using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackGroundMusic : MonoBehaviour
{
    public static BackGroundMusic bgmusic;

    private void Awake()
    {
        if (bgmusic != null) 
        {
            Destroy(gameObject);
        }
        else  
        {
            bgmusic = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
