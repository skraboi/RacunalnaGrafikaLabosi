using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class Bspline : MonoBehaviour
{
    /*public int[,] points =
    {
        {0, 0, 0 },
        {0, 10, 5 },
        {10, 10, 10 },
        {10, 0, 5 },

        {0, 0, 20 },
        {0, 10, 25 },
        {10, 10, 30 },
        {10, 0, 35 },

        {0, 0, 40 },
        {0, 10, 45 },
        {10, 10, 50 },
        {10, 0, 55 }
    };*/
    public Vector3[] points = new[] {
        new Vector3(0f, 0f, 0f),
        new Vector3(0f, 10f, 5f),
        new Vector3(10f, 10f, 10f),
        new Vector3(10f, 0f, 5f),

        new Vector3(0f, 0f, 20f),
        new Vector3(0f, 10f, 25f),
        new Vector3(10f, 10f, 30f),
        new Vector3(10f, 0f, 35f),

        new Vector3(0f, 0f, 40f),
        new Vector3(0f, 10f, 45f),
        new Vector3(10f, 10f, 50f),
        new Vector3(10f, 0f, 55f)
    };
    int pointsLen;

    public LineRenderer line;

    public bool drawTangents = false;

    public Camera mainCamera;

    public bool startMovement = false;
    public float movementSpeed = 1.0f;
    private float nextActionTime = 0.1f;
    private float timer = 0.0f;
    private int linePos = 0;
    private int numOfPositions;

    private Vector3 startRotation;
    private Vector3 startDirection;
    private Vector3 endRotation;
    private float degrees;

    Vector3 position;

    private List<Vector3> vectors = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        //izracun broja zadanih tocaka i koliko tocaka ce se racunati
        pointsLen = points.GetLength(0);
        numOfPositions = ((pointsLen - 3) * 100);
        
        //line crta spoj tocaka
        line.positionCount = numOfPositions;
        //line renderer current point position
        int pos = 0;

        //racunanje B splajn krivulje
        for (int i = 0; i < (pointsLen - 3); i++)
        {
            for (int t = 0; t < 100; t++)
            {
                float j = (t / 100f);
                double f1 = (-Mathf.Pow(j, 3.0f) + 3 * Mathf.Pow(j, 2.0f) - 3 * j + 1) / 6.0;
                double f2 = (3 * Mathf.Pow(j, 3.0f) - 6 * Mathf.Pow(j, 2.0f) + 4) / 6.0;
                double f3 = (-3 * Mathf.Pow(j, 3.0f) + 3 * Mathf.Pow(j, 2.0f) + 3 * j + 1) / 6.0;
                double f4 = Mathf.Pow(j, 3.0f) / 6.0;

                Vector3 point = (float)f1 * points[i] + (float)f2 * points[i + 1] + (float)f3 * points[i + 2] + (float)f4 * points[i + 3];

                line.SetPosition(pos, point);
                pos++;

                //vektori

                double t1 = 0.5 * (-Mathf.Pow(j, 2.0f) + 2 * j - 1);
                double t2 = 0.5 * (3 * Mathf.Pow(j, 2.0f) - 4 * j);
                double t3 = 0.5 * (-3 * Mathf.Pow(j, 2.0f) + 2 * j + 1);
                double t4 = 0.5 * (Mathf.Pow(j, 2.0f));

                point = (float)t1 * points[i] + (float)t2 * points[i + 1] + (float)t3 * points[i + 2] + (float)t4 * points[i + 3];

                if (drawTangents)
                {
                    GameObject linetest = new GameObject();
                    LineRenderer lRend = linetest.AddComponent<LineRenderer>();

                    lRend.startWidth = 0.01f;
                    lRend.endWidth = 0.01f;
                    Material lineMat = Resources.Load<Material>("Materials/Default");
                    lRend.material = lineMat;
                    lRend.SetPosition(0, line.GetPosition(pos - 1));
                    lRend.SetPosition(1, line.GetPosition(pos - 1) + point * 0.2f);
                }
                vectors.Add(point);
            }
        }

        startRotation = transform.forward;
        startDirection = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (startMovement && linePos < numOfPositions)
        {
            if (timer > nextActionTime)
            {
                timer = 0.0f;

                this.transform.position = line.GetPosition(linePos);
                //stari način raučunanja rotacije
                //transform.LookAt(transform.position + vectors[linePos]);

                /* početna orijentacija je izračunata u start
                if (linePos == 0)
                {
                    startRotation = transform.forward;
                }
                else
                {
                    startRotation = vectors[linePos - 1];
                }*/

                this.transform.rotation = Quaternion.Euler(startDirection);

                endRotation = vectors[linePos];
                Vector3 rotationVector = Vector3.Cross(startRotation, endRotation);
                //degrees = Vector3.Angle(startRotation, endRotation);
                degrees = Mathf.Acos(Vector3.Dot(startRotation, endRotation) / (Vector3.Magnitude(startRotation) * Vector3.Magnitude(endRotation)));
                degrees = degrees * Mathf.Rad2Deg;

                //provjera ispravnog mnozenja vektora
                if (drawTangents)
                {
                    GameObject linetest = new GameObject();
                    LineRenderer lRend = linetest.AddComponent<LineRenderer>();

                    lRend.startWidth = 0.01f;
                    lRend.endWidth = 0.01f;
                    Material lineMat = Resources.Load<Material>("Materials/DefaultVert");
                    lRend.material = lineMat;
                    lRend.SetPosition(0, transform.position);
                    lRend.SetPosition(1, transform.position + rotationVector * 0.2f);
                }
                
                this.transform.Rotate(rotationVector, degrees);
                
                linePos++;
            }
            timer += (Time.deltaTime * movementSpeed);
        }

        //kamera prati micanje objekta
        position = this.transform.position;
        position.x -= 1f;
        position.y += 1f;
        position.z += 1f;
        mainCamera.transform.position = position;
    }
}
