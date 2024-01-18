using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls the movement of the camera.
public class CameraController : MonoBehaviour
{
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        // follow player x value
        transform.position = new Vector3(player.transform.position.x, 1.5f, -5f);
    }
}
