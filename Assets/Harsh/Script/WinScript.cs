using System.Collections;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    [SerializeField] float delayBeforeChange = 3f;
    [SerializeField] string sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(delayBeforeChange);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
    }

}
