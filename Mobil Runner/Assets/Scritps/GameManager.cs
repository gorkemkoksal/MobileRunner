using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] Vector3 camOffSet;

    [SerializeField] TextMeshProUGUI greenScreenCount;
    [SerializeField] GameObject player;
    [SerializeField] GameObject greenScreen;
    [SerializeField] GameObject miniPlayer;
    [SerializeField] GameObject mutant;
    float greenScreenSpeed = 0.5f;
    float currentPosition;
    float targetPosition = 1f;
    [SerializeField] Transform greenScreensMover;
    List<GameObject> greenScreensToDestroy=new List<GameObject>();
    [SerializeField] Animator transitionAnimController;
    float transitionTime = 1f;

    [SerializeField] AnimationCurve greenScreenSpeedCurve;

    public float GreenScreenCount;
    public bool isLevelEnded;
    public bool isStartedAgain;
    Coroutine someCoroutine;
    bool safeCoroutineRunning;
    bool level1Passed;

    PlayerMovement playerMovement;
    PAnimController pAnimController;
    Camera mainCam;
    float playerPosition;
    float level1EndPoint = 135f;
    private void Awake()
    {
        ManageSingleton();
    }
    void ManageSingleton()
    {
        int instanceCount = FindObjectsOfType(GetType()).Length;
        if (instanceCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
            DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        if (mainCam == null)
            mainCam = Camera.main;
        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
        }
        //if (pAnimController == null)
        //    pAnimController = GetComponentInChildren<PAnimController>();
    }
    void Update()
    {
        Debug.Log(level1Passed);
        GreenScreenCountWriter();

        LeveLEnd(level1EndPoint);
    }
    void LeveLEnd(float levelEndPoint)
    {
        playerPosition = player.transform.position.z;
        if (playerPosition < levelEndPoint) { return; }

        isLevelEnded = true;
        playerMovement.enabled = false;
        mainCam.transform.position = player.transform.position + camOffSet;
        mainCam.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));

        someCoroutine = StartCoroutine(SettingTheGreenScreen((int)GreenScreenCount));
    }
    IEnumerator SettingTheGreenScreen(int numberOfGreenScreens)
    {
        if (safeCoroutineRunning) yield break;
        safeCoroutineRunning = true;

        float greenScreensForThisLevel;
        if (numberOfGreenScreens > 16)
        {
            greenScreensForThisLevel = 16;         
        }
        else
        {
            greenScreensForThisLevel = numberOfGreenScreens;
        }

        for (int i = 0; i < greenScreensForThisLevel * 0.25; i++)
        {
            for (int j = 0; j < greenScreensForThisLevel * 0.25; j++)
            {
                var greenScreens = Instantiate(greenScreen, new Vector3((i * 0.75f) - 1.1f, (j * 0.75f) + (0.75f / 2), player.transform.position.z - 1f), Quaternion.identity);
                greenScreens.transform.SetParent(greenScreensMover);
                greenScreensToDestroy.Add(greenScreens);
                GreenScreenCount--;
                GreenScreenCountWriter();
                //  greenScreensList.Add(greenScreen);
                yield return new WaitForSeconds(0.1f);
            }
        }
        if (numberOfGreenScreens < 16)
        {
          //StartCoroutine(ReloadScene);         /////////////////////////////////////////////////
            yield break;
        }
        currentPosition = 0f;
        targetPosition = 1f;
        yield return new WaitForSeconds(1f);
        while (currentPosition != targetPosition)
        {
            BigGreenScreenMover(Vector3.zero,2);
            yield return new WaitForSeconds(0.005f);
        }
        if (!level1Passed)
        {
            PlayerExchangeRunnerToMini();
        }
        else
        {
            PlayerExchangeRunnerToMutant();
        }

        currentPosition = 0f;

        while (currentPosition != targetPosition)
        {
            BigGreenScreenMover(new Vector3(0,0,2),0);
            yield return new WaitForSeconds(0.005f);
        }
        if (!level1Passed)
        {
          yield return new WaitForSeconds(1.75f);
           miniPlayer.transform.Rotate(0, 180f, 0);
            level1Passed = true;
        }
        else
        {
            yield return new WaitForSeconds(5f);
        }
        // Quaternion rotGoal = Quaternion.LookRotation(new Vector3(0, -180, 0).normalized);
        //miniPlayer.transform.rotation = Quaternion.Slerp(miniPlayer.transform.rotation, rotGoal, 0.1f);

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            GreenScreenCount = 0;

            LoadNextScene(1);
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex == 2)
                greenScreenCount.gameObject.SetActive(false);
            else
                greenScreenCount.gameObject.SetActive(true);
            StartCoroutine(LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }
    void PlayerExchangeRunnerToMutant()
    {
        mutant.SetActive(true);
        mutant.transform.position = player.transform.position;
        player.SetActive(false);
    }
    void PlayerExchangeRunnerToMini()
    {
        miniPlayer.SetActive(true);
        miniPlayer.transform.position = player.transform.position;
        player.SetActive(false);
    }
    void PlayerExchangeMiniToRunner()
    {
        miniPlayer.SetActive(false);
        player.SetActive(true);
        player.transform.position = Vector3.zero;
    }
    void BigGreenScreenMover(Vector3 beginning, float targetZ)
    {
        currentPosition = Mathf.MoveTowards(currentPosition, targetPosition, greenScreenSpeed * Time.deltaTime);
        greenScreensMover.transform.position = Vector3.Lerp(beginning, new Vector3(0, 0, targetZ), greenScreenSpeedCurve.Evaluate(currentPosition));
    }
    void GreenScreenCountWriter()
    {
        greenScreenCount.text = GreenScreenCount.ToString();
    }
    IEnumerator LoadNextScene(int levelIndex)
    {
        transitionAnimController.SetTrigger("transitionStart");

        yield return new WaitForSeconds(transitionTime);
        
        mainCam.transform.position = new Vector3(0, 7, -3);
        mainCam.transform.rotation = Quaternion.Euler(new Vector3(40, 0, 0));

        PlayerExchangeMiniToRunner();
     //   playerMovement.isRunning = false;  // olmadi en son dene baske yontemler
        foreach(GameObject g in greenScreensToDestroy)
        {
            Destroy(g);
        }
        isLevelEnded = false;
        SceneManager.LoadScene(levelIndex);
        playerMovement.enabled = true;
      //  playerMovement.isRunning = false;
        playerMovement.firstTouch = true;
        StopAllCoroutines();
        safeCoroutineRunning = false;

        //pAnimController.IsLevelStarted();
    }
    //IEnumerator ReloadScene()
    //{

    //    GreenScreenCount = 0;
    //    transitionAnimController.SetTrigger("transitionStart");

    //    yield return new WaitForSeconds(transitionTime);

    //    mainCam.transform.position = new Vector3(0, 7, -3);
    //    mainCam.transform.rotation = Quaternion.Euler(new Vector3(40, 0, 0));

    //    foreach (GameObject g in greenScreensToDestroy)
    //    {
    //        Destroy(g);
    //    }
    //    isLevelEnded = false;
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //    playerMovement.enabled = true;
    //    //  playerMovement.isRunning = false;
    //    playerMovement.firstTouch = true;
    //    StopAllCoroutines();
    //    safeCoroutineRunning = false;
    //}
}
