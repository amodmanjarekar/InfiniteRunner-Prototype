using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script controls the behaviour of the entire game.
public class GameController : MonoBehaviour
{
    public float runSpeed = 3f; // speed at which obstacles move
    public int obstacleLength = 7; // actual length of an obstacle plane; visible length will be 6
    public enum GameState // states of game progression
    {
        READY, // ready phase to allow player to get used to the controls
        EASY, // slow phase
        HARD, // fast phase
        OVER // game over phase
    };
    public GameState state = GameState.READY; // initial phase set to READY
    public GameObject obstaclePrefab; // an obstacle group; includes a plane and 3 psuedo-random obstacles
    public int score = 0;

    void Start()
    {
        obstacleLength -= 1; // visible length, actual length is more to prevent gaps

        StartCoroutine(PreparationTime()); // ready phase countdown

        // instantiate 5 obstacle groups to simulate world
        for (int num = 5, displacement = 0; num > 0; num -= 1, displacement += obstacleLength)
        {
            Instantiate(obstaclePrefab, new Vector3(0, 0, displacement), Quaternion.identity);
        }
    }

    void Update()
    {
        if (runSpeed > 12f)
        {
            state = GameState.HARD;
        }

        if (state == GameState.EASY)
        {
            runSpeed += Time.deltaTime * 0.1f;
        }

        if (state == GameState.OVER)
        {
            Time.timeScale = 0; // pauses the game at current frame
        }

        // restart the scene
        if ((Input.GetKeyDown(KeyCode.R) || Input.touchCount > 0) && state == GameState.OVER)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;
        }
    }

    void FixedUpdate()
    {
        if (state == GameState.EASY)
        {
            score += 5;
        }
        else if (state == GameState.HARD)
        {
            score += 100;
        }
    }

    // to instantiate a new obstacle group at farthest location (obstacleLength * 3)
    public void GenerateNewObstacle()
    {
        Instantiate(obstaclePrefab, new Vector3(0, 0, obstacleLength * 3), Quaternion.identity);
    }

    IEnumerator PreparationTime()
    {
        yield return new WaitForSeconds(3);
        state = GameState.EASY;
    }
}
