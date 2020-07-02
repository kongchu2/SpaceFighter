using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class kimotti : MonoBehaviour
{
    AsyncOperation InGameScene;
    public GameObject tutorialUI;
    private AudioSource myAudio;
    
    private void Start()
    {
        InGameScene = null;
        myAudio = GetComponent<AudioSource>();
        StartCoroutine(MoveScene());
    }
    IEnumerator MoveScene()
    {
        InGameScene = SceneManager.LoadSceneAsync(1);
        InGameScene.allowSceneActivation = false;
        while(!InGameScene.isDone)
        {
            yield return null;
        }
    }
    public void LoadScene()
    {
        LoadingSceneManager.LoadScene("InGameScene");
    }
    private void Update()
    {
        if (tutorialUI.activeSelf && Input.GetMouseButtonDown(0))
        {
            myAudio.Play();
            tutorialUI.SetActive(false);
        }
    }
    public void GameScene()
    {
        Debug.Log("clicked");
        InGameScene.allowSceneActivation = true;
    }
    public void Tutorial()
    {
        tutorialUI.SetActive(true);
    }
}
