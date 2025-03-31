using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    public Button button;
    
    void Start()
    {
        button.onClick.AddListener(StartScene);
    }

    public void StartScene()
    {
        SceneManager.LoadScene("Start Scene");
    }
}
