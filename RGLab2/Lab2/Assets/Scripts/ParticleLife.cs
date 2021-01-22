using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLife : MonoBehaviour
{

    public float lifetime;
    public Vector3 direction;
    private float time;
    private float currLerp;
    private Vector3 currPos;
    private float moveTime;

    // Start is called before the first frame update
    void Start()
    {
        //direction = Vector3.forward;
        //lifetime = 3.0f;
        time = 0.0f;
        currPos = transform.position;
        GetComponent<Renderer>().material.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        time += Time.deltaTime;

        if (time >= lifetime - 1.0f)
        {
            currLerp = 1 - (lifetime - time);
            GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.red, currLerp);
        }
        moveTime = time / lifetime;
        transform.position = Vector3.Lerp(currPos, (currPos + direction * lifetime * 4), moveTime);

        Vector3 startsize = new Vector3(1, 1, 1);
        Vector3 endsize = new Vector3(3,3,3);

        //transform.localScale(Vector3.Lerp(startsize, endsize, moveTime));

        if (time > lifetime)
        {
            Destroy(this.gameObject);
        }
    }
}
