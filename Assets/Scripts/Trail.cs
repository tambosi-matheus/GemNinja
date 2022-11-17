using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trail : MonoBehaviour
{
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private EdgeCollider2D coll;
    public bool clickActive, playingSound;
    public AudioSource audioSource;
    public float length;
    public float touchId;
    public Touch touch;

    public void ClickEnd() => StartCoroutine(WaitToQueue());

    private void OnEnable()
    {
        playingSound = false;
        clickActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!clickActive) return;
        var touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        touchPosition.z = 0;
        transform.position = touchPosition;
            
        if (GameManager.Instance.gameState == GameManager.GameStates.Main)
            SetTrailCollider();
        else
            coll.enabled = false;
    }
    
    private void SetTrailCollider()
    {
        var points = new Vector2[trail.positionCount];

        
        coll.enabled = true;
        for (int i = 0; i < trail.positionCount; i++)
            points[i] = trail.GetPosition(i);
        coll.points = points;
        coll.offset = -transform.position;
        if (GetTrailLenght(points) > 4f && !playingSound)
        {
            playingSound = true;
            StartCoroutine(PlaySound());
        }
    }

    private float GetTrailLenght(Vector2[] points)
    {        
        length = 0f;
        for(int i = 0; i < trail.positionCount -1; i++)        
            length = Vector2.Distance(points[i], points[i + 1]);        
        return length;
    }

    private IEnumerator PlaySound()
    {
        AudioManager.Instance.Play(audioSource, "Swing");
        yield return new WaitForSeconds(0.5f);
        playingSound = false;
    }
    
    private IEnumerator WaitToQueue()
    {
        clickActive = false;
        yield return new WaitForSeconds(trail.time);
        PoolTrail.Instance.Enqueue(this);
    }
}
