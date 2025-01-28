using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

   // Sorry for the long script, I'll have comments throughout explaining anything new/different since there is a lot of repetition throughout the script.
   // The code is sadly incomplete as there are a bunch of stuff missing for this assignment. 

public class Behaviours : MonoBehaviour
{
    public GameObject Seeker;
    public GameObject Target; // This set of GameObjects are for prefabs that will be reference throughout the script.
    public GameObject Hazard;

    private GameObject respawn1;
    private GameObject respawn2; // Since original prefabs cannot be destroyed in the game scene, this set is used to make copies which can be destroyed.
    private GameObject respawn3; 

    [SerializeField] private Transform bottomRightBorder;  // Used to create two border GameObjects which will help regulate how far a game object can move within the Game Scene. 
    [SerializeField] private Transform topLeftBorder;

    [SerializeField] public float moveSpeed; // Movement speed of the Seeker Object. 
    [SerializeField] public float rotationSpeed; // Rotation speed of Seeker's orientation.  

    private bool isSeeking = false; 
    private bool isFleeing = false; // These bools all start as a false value and are used to help regulate a lot of functions in this code.  
    private bool isArriving = false;
    private bool isAvoiding = false;

    void Start()
    {
        
    }
    
    void Update()
    {
        SceneReset(); // When called, the SceneReset function will activate. 

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            StartSeeking();       // When the number Keypad 1 is clicked, the StartSeeking function will be called. 
        }

        if (isSeeking && respawn1 != null && respawn2 != null)
        {
            Seek();          // If isSeeking is set to true and both game objects are present, the Seek function will be called.
        }

        // Similar to comments for seeking stuff above.
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            StartFleeing();
        }

        if (isFleeing && respawn1 != null && respawn2 != null)
        {
            Flee(); 
        }

        // Similar to comments for seeking stuff above.
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            StartArriving(); 
        }

        if (isArriving && respawn1 != null && respawn2 != null)
        {
            Arrive(); 
        }

        // Similar to comments for seeking stuff above. 
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            StartAvoiding();
        }

        if (isAvoiding && respawn1 != null && respawn2 != null)
        {
            Avoid();
        }
    }

    void SceneReset()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            // When the number Keypad 0 is clicked, the program will reset the current scene as if first initiated. 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);  
        } 
    }

    void StartSeeking()
    {
            
            if(isSeeking)
            {
                StopSeeking();  // When the bool isSeeking is true, the StopSeeking function will be called. 
            }
            
            if (respawn1 != null)
            {
                Destroy(respawn1);
            }
            if (respawn2 != null)
            {                       // Whenever the StartSeeking function is called, if there are game objects, they will be destroyed before new ones appear.  
                Destroy(respawn2);
            }
            if (respawn3 != null)
            {
                Destroy(respawn3);
            }

            // Spawns in game objects upon function call.
            respawn1 = Instantiate(Seeker, new Vector2(-10.0f, 0.0f), Quaternion.identity);
            respawn2 = Instantiate(Target, new Vector2(Random.Range(-18.0f, -2.0f), Random.Range(-4.0f, 4.0f)), Quaternion.identity);

            isSeeking = true;
            isFleeing = false;  // When the function is called, the current value of all these bools are set.  
            isArriving = false;
            isAvoiding = false;

        Debug.Log(" Seeking Mode Activated "); // Fun message when the function is called.
        
    }

    void StopSeeking()  // A function whose purpose is to force the isSeeking bool to become false. 
    {
        isSeeking = false;
    }



    void Seek()
    {
        // These 3 lines will translate the current position of the respawn1 object to move towards the current position of the respawn2 object. 
        Vector3 direction = respawn1.transform.position - respawn2.transform.position; 
        direction.Normalize();
        respawn1.transform.position -= direction * moveSpeed * Time.fixedDeltaTime;

        // So long as the current position of the game object is not (0, 0, 0), the object will rotate it's orientation in the direction it is moving. 
        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);
        }

        Debug.DrawLine(respawn1.transform.position, respawn1.transform.position - direction * 2.0f); // Draws a line that moves with the respawn1 game object. 
    }

    // Rest of the script is pretty much similar as the StartSeeking, StopSeeking, and Seek functions.
    // The only difference, which is explained, is commented down in the Flee function. 

    void StartFleeing()
    {
            if(isFleeing)
            {
              StopFleeing();
            }

            if (respawn1 != null)
            {
                Destroy(respawn1);
            }
            if (respawn2 != null)
            {
                Destroy(respawn2);
            }
        if (respawn3 != null)
        {
            Destroy(respawn3);
        }

        respawn1 = Instantiate(Seeker, new Vector2(-10.0f, 0.0f), Quaternion.identity);
            respawn2 = Instantiate(Target, new Vector2(Random.Range(-18.0f, -2.0f), Random.Range(-4.0f, 4.0f)), Quaternion.identity);

            isSeeking = false;
            isFleeing = true;
            isArriving = false;
            isAvoiding = false;

            Debug.Log(" Fleeing Mode Activated "); 
    }

    void StopFleeing()
    {
        isFleeing = false; 
    }

    void Flee()
    {
        Vector3 direction = respawn1.transform.position - respawn2.transform.position;
        direction.Normalize();
        respawn1.transform.position += direction * moveSpeed * Time.fixedDeltaTime;

        if(direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime); 
        }

        // These Ifs and Else/Ifs are used to control any movement restrictions for the respawn1 game object. 
        // Basically, if the x or y values of respawn1 ever meet the values for the 2 borders in place, the respawn1 game object will stop moving in the direction of x and/or y. 
        
        if(respawn1.transform.position.x < topLeftBorder.position.x)
        {
            respawn1.transform.position = new Vector3(topLeftBorder.position.x, respawn1.transform.position.y, respawn1.transform.position.z);
        }
        else if(respawn1.transform.position.x > bottomRightBorder.position.x)
        {
            respawn1.transform.position = new Vector3(bottomRightBorder.position.x, respawn1.transform.position.y, respawn1.transform.position.z);
        }

        if(respawn1.transform.position.y > topLeftBorder.position.y)
        {
            respawn1.transform.position = new Vector3(respawn1.transform.position.x, topLeftBorder.position.y, respawn1.transform.position.z);
        }
        else if(respawn1.transform.position.y < bottomRightBorder.position.y)
        {
            respawn1.transform.position = new Vector3(respawn1.transform.position.x, bottomRightBorder.position.y, respawn1.transform.position.z);
        }

        Debug.DrawLine(respawn1.transform.position, respawn1.transform.position + direction * 2.0f);

    }

    void StartArriving()
    {
           if (isArriving)
           {
              StopArriving(); 
           }
        
            if (respawn1 != null)
            {
                Destroy(respawn1);
            }
            if (respawn2 != null)
            {
                Destroy(respawn2);
            }
            if (respawn3 != null)
            {
                Destroy(respawn3);
            }

            respawn1 = Instantiate(Seeker, new Vector2(-10.0f, 0.0f), Quaternion.identity);
            respawn2 = Instantiate(Target, new Vector2(Random.Range(-18.0f, -2.0f), Random.Range(-4.0f, 4.0f)), Quaternion.identity);

            isSeeking = false;
            isArriving = true;
            isFleeing = false;
            isAvoiding = false;

            Debug.Log(" Arrivial Mode Activated ");
    }

    void StopArriving()
    {
        isArriving = false; 
    }

    void Arrive()
    {
        Vector3 direction = respawn1.transform.position - respawn2.transform.position;
        direction.Normalize();
        respawn1.transform.position -= direction * moveSpeed * Time.fixedDeltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);
        }

        Debug.DrawLine(respawn1.transform.position, respawn1.transform.position - direction * 2.0f);
    }

    void StartAvoiding()
    {
            if(isAvoiding)
            {
                StopAvoiding();
            }
        
            if (respawn1 != null)
            {
                Destroy(respawn1);
            }
            if (respawn2 != null)
            {
                Destroy(respawn2);
            }
            if (respawn3 != null)
            {
                Destroy(respawn3); 
            }

            respawn1 = Instantiate(Seeker, new Vector2(-10.0f, 0.0f), Quaternion.identity);
            respawn2 = Instantiate(Target, new Vector2(Random.Range(-18.0f, -2.0f), Random.Range(-4.0f, 4.0f)), Quaternion.identity);

            Vector2 midpoint = (respawn1.transform.position + respawn2.transform.position) / 2;

            respawn3 = Instantiate(Hazard, midpoint, Quaternion.identity);

            isSeeking = false;
            isArriving = false;
            isFleeing = false;
            isAvoiding = true;

            Debug.Log(" Avoidance Mode Activated "); 
        
    }

    void StopAvoiding()
    {
        isAvoiding = false;
    }

    void Avoid()
    {
        Vector3 direction = respawn1.transform.position - respawn2.transform.position;
        direction.Normalize();
        respawn1.transform.position -= direction * moveSpeed * Time.fixedDeltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);
        }

        Debug.DrawLine(respawn1.transform.position, respawn1.transform.position - direction * 2.0f);
    }

}
