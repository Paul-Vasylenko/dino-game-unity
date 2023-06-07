using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverScreen : MonoBehaviour
{
    public void restartButton()
    {
        SceneManager.LoadScene("Level1");
    }
}
