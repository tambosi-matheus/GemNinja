using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBomb : MonoBehaviour
{
    public static PoolBomb Instance;

    private Queue<Bomb> bombs = new Queue<Bomb>();

    [SerializeField] private Transform parent;
    [SerializeField] private GameObject prefab;


    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    public void Enqueue(Bomb bomb)
    {
        bomb.gameObject.SetActive(false);
        bombs.Enqueue(bomb);
    }


    public void DequeueWave(int wave)
    {
        var howManyBombs = WaveGenerator.GenerateBombs(wave);
        for (int i = 0; i < howManyBombs; i++)
        {
            Bomb bomb;
            bombs.TryDequeue(out bomb);
            if (bomb == null)
                bomb = Create();
            Awake(bomb, Random.value * 5f);
        }
    }

    private IEnumerator Awake(Bomb gem, float time)
    {
        yield return new WaitForSeconds(time);
        gem.gameObject.SetActive(true);
    }

    public Bomb Create()
    {
        var obj = Instantiate(prefab, parent);
        var bomb = obj.GetComponent<Bomb>();
        return bomb;
    }

    public void EnqueueAll()
    {
        StopAllCoroutines();
        for (int i = 0; i < parent.childCount; i++)
        {
            var bomb = parent.GetChild(i).GetComponent<Bomb>();
            if (bomb.isActiveAndEnabled)
                Enqueue(bomb);
        }
    }
}
