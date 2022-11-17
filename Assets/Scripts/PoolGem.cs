using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolGem : MonoBehaviour
{
    public static PoolGem Instance;

    [SerializeField] private Dictionary<string, List<Gem>> gems =
        new Dictionary<string, List<Gem>>();
    private int[] gemValue = WaveGenerator.gemValue;

    [SerializeField] private Transform gemsParent;
    [SerializeField] private Material[] gemMaterials;
    [SerializeField] private GameObject[] gemPrefabs;


    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < gemValue.Length; i++)
            gems.Add(gemValue[i].ToString(), new List<Gem>());
    }

    public void Enqueue(Gem gem)
    {
        gem.gameObject.SetActive(false);
        var list = gems[gem.name];
        list.Add(gem);
    }


    public List<Gem>[] DequeueWave(int wave)
    {
        var pointsArray = WaveGenerator.GenerateWave(wave);
        var gemArray = new List<Gem>[6] { 
            new List<Gem>(), new List<Gem>(), new List<Gem>(),
            new List<Gem>(),new List<Gem>(),new List<Gem>()};
        for (int i = 0; i < pointsArray.Length; i++)
        {
            var gemsToSpawn = pointsArray[i];
            for (int j = 0; j < gemsToSpawn; j++)
            {
                var list = gems[gemValue[i].ToString()];
                Gem gem = null;
                if (list.Count == 0) gem = Create(i);
                else
                {
                    gem = list[Random.Range(0, list.Count)];
                    list.Remove(gem);
                }
                StartCoroutine(AwakeGem(gem, Random.Range(0.1f, 5f)));    
                gemArray[i].Add(gem);
            }
        }
        return gemArray;
    }

    private IEnumerator AwakeGem(Gem gem, float time)
    {
        yield return new WaitForSeconds(time);
        gem.gameObject.SetActive(true);
    }

    public Gem Create(int arrayPos)
    {
        // Get a random prefab gem and material
        var mat = gemMaterials[arrayPos];
        var gem = gemPrefabs[Random.Range(0, gemPrefabs.Length)];


        // Create gem
        var obj = Instantiate(gem, gemsParent);
        obj.name = gemValue[arrayPos].ToString();
        obj.GetComponent<Renderer>().material = mat;
        var gemScript = obj.GetComponent<Gem>();
        obj.SetActive(false);

        return gemScript;
    }

    private void ChangeColor(Gem gem)
    {
        var mat = gemMaterials[Random.Range(0, gemMaterials.Length)];
        gem.GetComponent<Renderer>().material = mat;
    }

    public void EnqueueAll()
    {
        StopAllCoroutines();
        for (int i = 0; i < gemsParent.childCount; i++)
        {
            var gem = gemsParent.GetChild(i).GetComponent<Gem>();
            if (gem.isActiveAndEnabled)
                Enqueue(gem);
        }
    }
}
