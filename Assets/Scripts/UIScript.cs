using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This script controls the behaviour of the UI
public class UIScript : MonoBehaviour
{
    GameController gc;
    TextMeshProUGUI scoreUI;
    public GameObject gameOverLabel; // initially disabled/inactive

    void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        scoreUI = GameObject.Find("Score").GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        scoreUI.text = "Score: " + gc.score;

        if (gc.state == GameController.GameState.OVER)
        {
            gameOverLabel.SetActive(true);
        }
    }
}
