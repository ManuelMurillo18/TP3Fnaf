using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuScript : MonoBehaviour
{
    [SerializeField] string sceneName;
    public void StartGame()
    {
        SceneManager.LoadScene(sceneName); 
    }
}
