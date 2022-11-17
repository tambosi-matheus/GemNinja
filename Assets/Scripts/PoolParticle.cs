using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolParticle : MonoBehaviour
{
    public static PoolParticle Instance;

    [SerializeField] private Queue<ParticleSystem> particles = new Queue<ParticleSystem>();
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform parent;
    [SerializeField] private Sprite[] images;
    [SerializeField] private Color[] color;


    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    public void Enqueue(ParticleSystem particle)
    {
        particle.gameObject.SetActive(false);
        particles.Enqueue(particle);
    }


    public ParticleSystem Dequeue(Vector2 pos, int value)
    {
        ParticleSystem particle = null;
        int arrayPos = -1;
        for (int i = 0; i < WaveGenerator.gemValue.Length; i++)
        {
            if (WaveGenerator.gemValue[i] == value)
            { arrayPos = i; break; }         
        }
        if (particles.Count == 0)                    
            particle = Create();        
        else
            particle = particles.Dequeue();
        particle.gameObject.SetActive(true);
        particle.transform.position = pos;
        particle.textureSheetAnimation.SetSprite(0, images[arrayPos]);
        return particle;
    }

    public ParticleSystem Create()
    {
        var obj = Instantiate(prefab, parent);  
        obj.SetActive(true);
        var particle = obj.GetComponent<ParticleSystem>();        

        return particle;
    }
}
