using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles behavoiur of an individual obstacle group.
public class ObstacleGroup : MonoBehaviour
{

    GameController gc;

    // array of 3 values; each value denotes obstacle on the left, center and right in sequence
    // 0 denotes an empty space, 1 denotes a jumping obstacle, 2 denotes a wall obstacle
    int[] obstacles;
    public GameObject obstacleJump;
    public GameObject obstacleWall;

    void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();

        if (gc.state == GameController.GameState.READY)
        {
            obstacles = new int[] { 0, 0, 0 }; // only instantiate empty planes during ready state
        }
        else if (gc.state == GameController.GameState.EASY || gc.state == GameController.GameState.HARD)
        {
            obstacles = GenerateRandomObstacleArray(); // generate random obstacles
        }


        for (int index = 0; index < 3; index++)
        {
            if (obstacles[index] == 1) // ijumping obstacle
            {
                Instantiate(obstacleJump, new Vector3(index - 1, 0.1f, transform.position.z + 2.5f), Quaternion.identity, transform);
            }
            else if (obstacles[index] == 2) // wall obstacle
            {
                Instantiate(obstacleWall, new Vector3(index - 1, 1.15f, transform.position.z + 2.5f), Quaternion.identity, transform);
            }
            else // no obstacle (empty space)
            {
                continue;
            }
        }
    }

    void Update()
    {
        transform.Translate(-1 * gc.runSpeed * Time.deltaTime * Vector3.forward); // continously move obstacle group backwards
        // once obstacle group goes out of player's visible scope, instantiate a new one and destroy self
        if (transform.position.z < -12f)
        {
            gc.GenerateNewObstacle();
            Destroy(gameObject);
        }
    }

    int[] GenerateRandomObstacleArray()
    {
        int[] obstacles = { -1, -1, -1 };
        obstacles[Random.Range(0, 2)] = 0; // ensure at least one empty space

        for (int i = 0; i < 3; i++)
        {
            if (obstacles[i] != 0)
            {
                obstacles[i] = Random.Range(0, 3);
            }
        }

        return obstacles;
    }
}
