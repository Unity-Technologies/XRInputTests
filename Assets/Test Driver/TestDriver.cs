using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Start Scene must be Scene 0 in the build settings
// To run a test in playmode, load both the start scene and a test scene.  The test scene should be the active scene.
//
public class TestDriver : MonoBehaviour {

    public Text SceneNameText;
    public Text SceneDescriptionText;

    private int m_CurrentSceneIndex {get { return SceneManager.GetActiveScene().buildIndex; } set { } }

    private bool m_FlagMoveToNextScene;
    private bool m_FlagMoveToPreviousScene;

    private InputMobileForSceneChange m_InputMobileForSceneChange;

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        m_InputMobileForSceneChange = new InputMobileForSceneChange();
    }

    private void OnEnable()
    {
        if (m_CurrentSceneIndex == 0 && SceneManager.sceneCountInBuildSettings > 1) { LoadNextScene(); }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        UpdateInput();

        if (m_FlagMoveToNextScene) { LoadNextScene(); }
        if (m_FlagMoveToPreviousScene) { LoadPreviousScene(); }
    }

    public void RunTest(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    private void LoadNextScene()
    {
        int nextLevel = (m_CurrentSceneIndex + 1) % (SceneManager.sceneCountInBuildSettings);
        if (nextLevel == 0) { nextLevel = 1; }
        
        RunTest(nextLevel);
    }

    private void LoadPreviousScene()
    {
        int previousLevel = m_CurrentSceneIndex - 1;
        if (previousLevel < 1) { previousLevel = SceneManager.sceneCountInBuildSettings - 1; }

        RunTest(previousLevel);
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        yield return SceneManager.LoadSceneAsync(sceneIndex);
    }

    void OnSceneLoaded(Scene newlyLoadedScene, LoadSceneMode loadSceneMode)
    {
        MoveCameraToAnchor();
        UpdateInstructionsUI();
    }

    private void MoveCameraToAnchor()
    {
        GameObject anchor = GameObject.FindGameObjectWithTag("CameraAnchor");
        if (anchor)
        {
            transform.position = anchor.transform.position;
        }
    }

    private void UpdateInstructionsUI()
    {
        SceneNameText.text = m_CurrentSceneIndex + " / " + (SceneManager.sceneCountInBuildSettings-1) + " - " + SceneManager.GetActiveScene().name;

        GameObject SceneDescriptor = GameObject.FindGameObjectWithTag("TestInstructions");

        if (SceneDescriptor != null)
        {
            SceneDescriptionText.text = SceneDescriptor.GetComponent<TestInstructions>().GetFullString();
        }
        else
        {
            SceneDescriptionText.text = "No Scene Descriptive Text Found";
        }
    }

    private void UpdateInput()
    {
        m_FlagMoveToNextScene = m_FlagMoveToPreviousScene = false;

        if (Application.isMobilePlatform)
        {
            m_InputMobileForSceneChange.UpdateInputMobile(ref m_FlagMoveToNextScene, ref m_FlagMoveToPreviousScene);
        }
        else if (Application.isConsolePlatform)
        {
            m_FlagMoveToNextScene = Input.GetAxis("Right Trigger") > 0.5f;
            m_FlagMoveToPreviousScene = Input.GetAxis("Left Trigger") > 0.5f;
        }
        else
        {
            m_FlagMoveToNextScene = Input.GetKeyDown(KeyCode.RightArrow);
            m_FlagMoveToPreviousScene = Input.GetKeyDown(KeyCode.LeftArrow);
        }
    }

    
}
