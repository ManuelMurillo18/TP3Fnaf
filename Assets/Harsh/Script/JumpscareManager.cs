using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpscareManager : MonoBehaviour
{
    public static JumpscareManager Instance;

    bool isJumpscareActive = false;
    [SerializeField] GameObject jumpscareCamera;
    [SerializeField] Camera mainCamera;
    [SerializeField] PatrolAnimatronicComponent patrolAnimatronicComponent;
    [SerializeField] PlayerComponent playerComponent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateJumpscare()
    {
        if (isJumpscareActive)
            return;
        mainCamera.enabled = false;
        playerComponent.PlayerDeath();
        jumpscareCamera.SetActive(true);
        patrolAnimatronicComponent.JumpScare();
        isJumpscareActive = true;
        StartCoroutine(GameOver());

    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync("Menu");
    }
}
