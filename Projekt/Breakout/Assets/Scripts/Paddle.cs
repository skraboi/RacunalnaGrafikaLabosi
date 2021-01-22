using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paddle : MonoBehaviour
{
    private float TopPaddleHalfLength;
    private bool gameHasStarted;
    private int numOfLives = 3;
    private int numOfEnemies = 0;
    private GameObject ui;

    private GameObject ball;
    Vector3 ballPos;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Tipke A i D za pomicanje palice, spacebar za lansiranje loptice.");

        ball = GameObject.Find("Ball"); //pronalazi objekt loptice
        ui = GameObject.FindGameObjectWithTag("UI");

        TopPaddleHalfLength = Mathf.Abs((transform.right * transform.localScale.x / 2.0f).x);
        gameHasStarted = false;

        ball.GetComponent<Ball>().setPaddle(this.gameObject);

        Object heart = Resources.Load("life");
        Vector2 corner = new Vector2();
        corner.y = -25;
        for (int i = 0; i < numOfLives; i++)
        {
            GameObject newLife = (GameObject)Instantiate(heart);
            newLife.transform.SetParent(ui.transform);
            newLife.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            newLife.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            corner.x = 25 + i * 50;
            newLife.GetComponent<RectTransform>().anchoredPosition = corner;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameHasStarted) //igra nije pocela
        {
            ballPos = this.transform.position;
            ballPos.y = -4f;
            ball.transform.position = ballPos;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                direction = Vector3.up;
                direction.x = 0.7f;
                ball.GetComponent<Ball>().startGame(direction);
                gameHasStarted = true;
            }
        }

        if (numOfEnemies <= 0)
        {
            ui.transform.GetChild(0).gameObject.SetActive(true);
            ball.GetComponent<Ball>().setDirection(Vector3.zero);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //prvo učitaj raycast centra ploče koju udari
        //raycast centar objekta s točkom sudara
        //izračunaj udaljenost od centra plohedo centra sudara
        //podijeli na područja

        if (collision.contacts[0].normal == Vector3.down) //Loptica je udarila gornju povrsinu palice. Vector je down zato sto racuna u kojem 
        {
            float TopPaddleCenter = transform.position.x;

            float collisionPoint = this.GetComponent<Collider>().ClosestPointOnBounds(collision.transform.position).x;
            float distance = collisionPoint - TopPaddleCenter; //udaljenost sudara od sredista ploce
            float distanceRelative = (distance / TopPaddleHalfLength); //udaljenost kao postotak duljine strane

            if (Mathf.Abs(distanceRelative) < 0.1f) //loptica je udarila "sredinu" palice
            {
                ball.GetComponent<Ball>().setDirection(Vector3.up);
            }
            else if (Mathf.Abs(distanceRelative) > 0.8f) //loptica je udarila "rub" palice
            {
                direction = Vector3.up;
                if (distance < 0)
                {
                    direction.x = -1.0f;
                } else
                {
                    direction.x = 1.0f;
                }
                ball.GetComponent<Ball>().setDirection(direction);
            }
            else //loptica je udarila izmedju centra i ruba
            {
                float lerpAmount = ((Mathf.Abs(distanceRelative) - 0.1f) / (0.8f - 0.1f)); //postotak = (x-a)/(b-a)

                Vector3 sideDir = Vector3.up;
                if (distance < 0)
                {
                    sideDir.x = -1.0f;
                }
                else
                {
                    sideDir.x = 1.0f;
                }

                direction = Vector3.Lerp(Vector3.up, sideDir, lerpAmount);

                ball.GetComponent<Ball>().setDirection(direction);
            }
        }
    }

    public void loseLife()
    {
        numOfLives--;
        gameHasStarted = false;
        Destroy(ui.transform.GetChild(ui.transform.childCount - 1).gameObject);

        if (numOfLives <= 0)
        {
            Destroy(ball);
            gameHasStarted = true;
        }
    }

    public void addEnemies(int i)
    {
        numOfEnemies += i;
    }
}
