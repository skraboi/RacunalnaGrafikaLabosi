using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int numOfLives = 1;
    private Transform child;

    private GameObject paddle;
    
    // Start is called before the first frame update
    void Start()
    {
        child = transform.GetChild(1);

        setColor();

        paddle = GameObject.Find("Paddle");
        paddle.GetComponent<Paddle>().addEnemies(1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void setColor()
    {
        if (numOfLives == 1)
        {
            child.transform.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else if (numOfLives == 2)
        {
            child.transform.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
        else if (numOfLives >= 3)
        {
            child.transform.GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ball")
        {
            numOfLives--;
            setColor();
        }
    }

    private void LateUpdate()
    {
        if (numOfLives <= 0)
        {
            paddle.GetComponent<Paddle>().addEnemies(-1);
            Destroy(this.gameObject);
        }
    }
}
