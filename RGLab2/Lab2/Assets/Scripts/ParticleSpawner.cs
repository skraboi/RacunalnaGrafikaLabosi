using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    private float time;
    private float spawntime;
    private Object path;
    private GameObject particle;
    private Vector3 direction;
    private float lifetime;

    private Vector3 start;
    private Vector3 stop;
    private float movetime;
    private float movepos;

    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
        path = Resources.Load("ParticleBalloon");
        start = transform.position;
        stop = start + Vector3.up * 4;
        movetime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        spawntime = Random.Range(0.05f, 0.5f);
        //spawntime = 0.05f;
        time += Time.deltaTime;
        movetime += Time.deltaTime;
        if (time >= spawntime)
        {
            time = 0.0f;
            particle = (GameObject)Instantiate(path);
            particle.transform.position = this.transform.position;

            direction = Vector3.forward;
            direction.x += Random.Range(-0.3f, 0.3f);
            direction.y += Random.Range(-0.3f, 0.3f);
            particle.GetComponent<ParticleLife>().direction = direction;

            lifetime = 3.0f + Random.Range(-1.0f, 2.0f);
            particle.GetComponent<ParticleLife>().lifetime = lifetime;

            particle.transform.LookAt(Camera.main.transform);
        }

        movepos = movetime / 3.0f;
        movepos = movepos * movepos * (3.0f - 2.0f * movepos);

        transform.position = Vector3.Lerp(start, stop, movepos);
        
        if (movetime > 3.0f)
        {
            movetime = 0.0f;
            Vector3 temp = start;
            start = stop;
            stop = temp;
        }
    }
}
