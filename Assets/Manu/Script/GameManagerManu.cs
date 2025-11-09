using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerManu : MonoBehaviour
{
    public static GameManagerManu Instance;

    bool isJumpscareActive = false;
    [SerializeField] GameObject jumpscareCamera;
    [SerializeField] Camera mainCamera;
    [SerializeField] FinalBossComponent finalBossComponent;
    [SerializeField] ManuPlayerComp manuPlayerComp;
    MusicController musicController;


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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicController = GetComponent<MusicController>();
        PlayAmbianceMusic();
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
        manuPlayerComp.PlayerDeath();
        jumpscareCamera.SetActive(true);
        finalBossComponent.JumpScare();
        isJumpscareActive = true;
        StartCoroutine(GameOver());

    }

    public void PlayAmbianceMusic()
    {
        musicController.PlayAmbianceMusic();
    }

    public void PlayEventMusic()
    {
        musicController.PlayEventMusic();
    }

    public void WinGame()
    {
        StartCoroutine(Win());
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync("WinScene(End)");
    }
    IEnumerator GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync("Menu");
    }
}
