using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaySceneChange : MonoBehaviour
{
    public Button button;

    void Start()
    {
        button.onClick.AddListener(GameScene);
    }

    public void GameScene()
    {
        SceneManager.LoadScene("Game Scene");
    }
}
