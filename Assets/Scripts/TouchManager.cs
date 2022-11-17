using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class TouchManager : MonoBehaviour
{
    [SerializeField] private PoolTrail poolTrail;
    private Trail trail;
    [SerializeField]
    private Dictionary<int, Trail> touchIdentifier = new Dictionary<int, Trail>();

    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount != 0)
        {
            var touches = Input.touches;
            foreach(Touch t in touches)
            {
                var id = t.fingerId;
                switch(t.phase)
                {
                    case TouchPhase.Began:
                        var trail = poolTrail.Dequeue();
                        var position = Camera.main.ScreenToWorldPoint(t.position);
                        position.z = 0;
                        trail.transform.position = position;
                        trail.touchId = id;
                        touchIdentifier.Add(id, trail);                        
                        break;

                    case TouchPhase.Ended:
                        touchIdentifier[id].ClickEnd();
                        touchIdentifier.Remove(id);
                        continue;
                }
                touchIdentifier[id].touch = t;
            }
        }
    }
    
}
