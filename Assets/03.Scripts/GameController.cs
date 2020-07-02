using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static int score;
    public static int highScore = -1;
    public Text scoreUI;
    private void Start()
    {
        score = 0;

    }
    private void FixedUpdate()
    {
        if (highScore < score)
            highScore = score;
        scoreUI.text = score.ToString();
        if(PlayerController.isDie && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("MainScene");
        }
         
    }
}