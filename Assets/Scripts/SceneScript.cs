using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneScript : MonoBehaviour
{
    public void LoadLevel()
    {
        SceneManager.LoadScene("gameplay");
    }
}
