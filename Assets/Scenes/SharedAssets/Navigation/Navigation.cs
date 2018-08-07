using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour {

    private static Navigation m_Instance = null;
    public static bool loadingScene { get; private set; }


    public static void NextScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        int nextSceneIndex = activeScene.buildIndex + 1;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 1; // Because Start scene is index 0
        }
        if (m_Instance != null)
        {
            m_Instance.LoadSceneAtIndex(nextSceneIndex);
        }
        //SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Single);
    }

    public static void PreviousScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        int nextSceneIndex = activeScene.buildIndex - 1;
        if (nextSceneIndex < 1)
        {
            nextSceneIndex = SceneManager.sceneCountInBuildSettings - 1;
        }
        if (m_Instance != null)
        {
            m_Instance.LoadSceneAtIndex(nextSceneIndex);
        }
        //SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Single);
    }
    
    

	// Use this for initialization
	void Awake () {
        m_Instance = this;
	}

    void Start()
    {
        loadingScene = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadSceneAtIndex(int index)
    {
        Debug.Log("LoadSceneAtIndex: " + index);
        NavigationArrow[] arrows = GetComponentsInChildren<NavigationArrow>();
        foreach(NavigationArrow arrow in arrows)
        {
            MeshRenderer renderer = arrow.GetComponent<MeshRenderer>();
            if(renderer != null)
            {
                renderer.material.SetColor("_TintColor", Color.white);
            }
        }
        
        loadingScene = true;

        StartCoroutine(LoadScene(index));
    }

    private IEnumerator LoadScene(int sceneToLoad)
    {
        yield return SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
        loadingScene = false;
        yield return null;
    }
}
