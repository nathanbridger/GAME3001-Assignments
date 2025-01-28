using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    public Button button;

    void Start()
    {
        // adds the PlayScene function as an onClick option for the button.
        button.onClick.AddListener(PlayScene);
    }

    public void PlayScene()
    {
        Debug.Log(" Button pressed ");
        Debug.Log(" Entering new scene "); 

        // Checks the build profile's scene list and switches scene to PlayScene if it's there
        SceneManager.LoadScene("PlayScene");
    }
}
