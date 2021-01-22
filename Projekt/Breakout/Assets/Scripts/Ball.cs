using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3 direction;
    private bool gameHasStarted;
    private float moveSpeed = 10.0f;
    private GameObject paddle;
    private GameObject deathwall;
    private int counter;
    private float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        counter = 10;
        gameHasStarted = false;
        deathwall = GameObject.Find("Deathwall");
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (gameHasStarted)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }   
    }

    public void startGame(Vector3 startingDirection)
    {
        this.direction = startingDirection;
        counter = 10;
        moveSpeed = 10.0f;
        gameHasStarted = true;
    }

    public void setDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void setPaddle(GameObject obj)
    {
        paddle = obj;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (time > 0.05f)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (counter <= 0)
                {
                    moveSpeed *= 1.05f;
                }
                counter--;
                return;
            }
            if (collision.gameObject == deathwall)
            {
                paddle.GetComponent<Paddle>().loseLife();
                direction = Vector3.zero;
            }

            Vector3 curDir = direction;
            Vector3 panelNormal = collision.contacts[0].normal.normalized;
            if (panelNormal == Vector3.left.normalized || panelNormal == Vector3.right.normalized)
            {
                curDir.x = -curDir.x;
                direction = curDir;
            }
            else
            {
                curDir.y = -curDir.y;
                direction = curDir;
            }

            time = 0.0f;
        }
    }
}
