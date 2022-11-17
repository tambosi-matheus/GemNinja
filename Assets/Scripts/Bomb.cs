using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bomb : MonoBehaviour
{
    private InGameManager manager;

    // Initialization variables
    private Rigidbody2D rb;
    private Vector2 rotation;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() => manager = InGameManager.Instance;


    private void OnEnable()
    {
        AudioManager.Instance.Play(audioSource, "SpawnBomb");
        UpdateBody();
    }

    private void UpdateBody()
    {
        // Set position
        var newpos = new Vector2(
            Random.Range(WaveGenerator.spawnMinMax.x, WaveGenerator.spawnMinMax.y), -5);
        transform.position = newpos;
        rb.velocity = Vector3.zero;

        // Set rotation
        transform.rotation = Quaternion.identity;
        rb.angularVelocity = 0;
        rotation = new Vector2(Random.value * 200, Random.value * 200);

        // Set force
        var force = Random.Range(250, 400);
        var final = WaveGenerator.aim - (Vector2)transform.position;
        rb.AddForce(final.normalized * force);
        rb.AddTorque(5f);
    }

    private void FixedUpdate()
    {
        transform.Rotate(rotation * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.transform.parent.name == "Player")
            manager.OnBombTouched(this);
        else
            manager.OnBombTouched(this, true);
    }
}
