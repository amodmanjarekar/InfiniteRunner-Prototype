using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles the movement of the Player
public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    enum MoveState // states of player movement
    {
        STABLE,
        MOVING
    }
    MoveState moveState = MoveState.STABLE;
    int newX; // move until this value is reached
    int moveDir; // -1 => move left ; 1 => move right

    bool isGrounded;
    public float jumpForce = 400f;
    public float moreGravity = 1.8f; // for extra gravity

    Rigidbody rb;
    GameController gc;

    // for mobile controls
    Touch touch;
    Vector2 touchStart, touchEnd;
    bool swipeRight = false, swipeLeft = false, swipeUp = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gc = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        // get mobile swipe direction
        if (Input.touchCount > 0)
        {
            Debug.Log("Touch Detected");
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                touchEnd = touch.position;
                if (touchStart != touchEnd)
                {
                    double alpha = CalculateSwipeAngle();

                    if (alpha < 45.0)
                    {
                        if (touchEnd.x > touchStart.x)
                        {
                            swipeRight = true;
                        }
                        else if (touchEnd.x < touchStart.x)
                        {
                            swipeLeft = true;
                        }
                    }
                    else
                    {
                        if (touchEnd.y > touchStart.y)
                        {
                            swipeUp = true;
                        }
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (moveState == MoveState.MOVING)
        {
            // while state is MOVING, constantly add velocity in direction of movement
            rb.velocity = new Vector3(moveDir * moveSpeed, rb.velocity.y, rb.velocity.z);
            // stop after reaching desired x value
            if ((moveDir == -1 && transform.position.x <= newX) || (moveDir == 1 && transform.position.x >= newX))
            {
                moveDir = 0;
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
                transform.position = new Vector3((int)transform.position.x, transform.position.y, transform.position.z); // shave off excess movement
                moveState = MoveState.STABLE;
            }
        }

        // left/right movement
        // move only if x is within bounds and player is STABLE
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || swipeLeft) && transform.position.x > -0.1f && moveState == MoveState.STABLE)
        {
            moveState = MoveState.MOVING;
            newX = (int)Math.Round(transform.position.x) - 1;
            moveDir = -1;
            swipeLeft = false;
        }
        if ((Input.GetKeyDown(KeyCode.RightArrow) || swipeRight) && transform.position.x < 0.1f && moveState == MoveState.STABLE)
        {
            moveState = MoveState.MOVING;
            newX = (int)Math.Round(transform.position.x) + 1;
            moveDir = 1;
            swipeRight = false;
        }

        rb.AddForce(moreGravity * rb.mass * Physics.gravity); // extra gravity for snappier jumps

        // jumping movement
        if (transform.position.y > 0.46f)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
        if ((Input.GetKeyDown(KeyCode.Space) || swipeUp) && isGrounded)
        {
            rb.AddForce(jumpForce * Vector3.up);
        }
    }

    // set game over when player collides with an obstacle
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            gc.state = GameController.GameState.OVER;
        }
    }

    double CalculateSwipeAngle()
    {
        double angle;
        float x, y;
        x = Math.Abs(touchEnd.x - touchStart.x);
        y = Math.Abs(touchEnd.y - touchStart.y);
        angle = Math.Atan(y / x);

        return angle * 180 / Math.PI;
    }
}
