using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTrail : MonoBehaviour
{
    public static PoolTrail Instance;

    [SerializeField] private Queue<Trail> trails = new Queue<Trail>();
    [SerializeField] private GameObject parent;
    private AudioSource parentAudioSource;
    [SerializeField] private GameObject prefab;


    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        parentAudioSource = parent.GetComponent<AudioSource>();
    }

    public void Enqueue(Trail trail)
    {
        trail.GetComponent<TrailRenderer>().SetPositions(Array.Empty<Vector3>());
        trail.gameObject.SetActive(false);
        trails.Enqueue(trail);
    }


    public Trail Dequeue()
    {
        if (trails.Count == 0)
            trails.Enqueue(Create());
        var trail = trails.Dequeue();
        trail.gameObject.SetActive(true);
        return trail;
    }

    public Trail Create()
    {
        var obj = Instantiate(prefab, parent.transform);
        var script = obj.GetComponent<Trail>();
        script.audioSource = parentAudioSource;
        obj.SetActive(true);

        return script;
    }
}
