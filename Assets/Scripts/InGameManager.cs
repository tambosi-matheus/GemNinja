using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    // Singleton
    public static InGameManager Instance;    

    // Wave
    [SerializeField] private PoolGem poolGem;
    [SerializeField] private PoolBomb poolBomb;
    [SerializeField] private List<Gem>[] waveArray;
    private int[] gemsDestroyed = new int[6];
    private Func<bool> isWaveFinished, waitNewWave;
    public Func<bool> waitGameOver;


    // Lifes
    [System.Serializable]
    private class Life
    {
        public GameObject obj;
        [NonSerialized] public bool isAlive = false;
        [NonSerialized] public Image img;
    }
    [SerializeField] private Life[] lifes;
    [SerializeField] private Color colorLifeNormal, colorLifeDead;

    // General
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private Animator gameOverAnimator;
    [SerializeField] private TextMeshPro newWaveText;
    [SerializeField] private Animator newWaveAnimator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshPro scoreText, maxScoreText;
    [SerializeField] private GameObject ready;
    private Animator scoreTextAnim;
    private int m_score;
    public int score
    {
        get
        {
            return m_score;
        }
        set
        {
            m_score = value;
            if(scoreTextAnim.isActiveAndEnabled) scoreTextAnim.Play("Scale");
            scoreText.SetText(score.ToString());
        }
    }

    private Coroutine spawnCoroutine;


    private void Awake()
    {
        Instance = this;
        Array.ForEach(lifes, l => l.img = l.obj.GetComponent<Image>());
        isWaveFinished = () => Array.Find(waveArray, l => l.Count != 0) == null;
        waitNewWave = () => newWaveAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        waitGameOver = () => gameOverAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        scoreTextAnim = scoreText.GetComponent<Animator>();
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Gems"),LayerMask.NameToLayer("Gems"), true);
    }

    private void Start()
    {
        poolGem = PoolGem.Instance;
        poolBomb = PoolBomb.Instance;
    }

    private void OnEnable()
    {
        // Set initial and recurring game state
        spawnCoroutine = StartCoroutine(SpawnWaves());
        gameOverText.SetActive(false);
        SetLifeObjects();
    }

    private void OnDisable()
    {
        PlayerData.Instance.OnEndRound(score, gemsDestroyed);
        gemsDestroyed = new int[WaveGenerator.gemValue.Length];
        poolGem.EnqueueAll();
        poolBomb.EnqueueAll();
        score = 0;
        StopAllCoroutines();
    }

    public IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(3f);
        int wave = 0;
        while (true)
        {
            wave++;
            SetLifeObjects();
            newWaveText.SetText("Wave " + wave.ToString());
            newWaveAnimator.Play("NewWave");

            yield return new WaitUntil(waitNewWave);
            waveArray = poolGem.DequeueWave(wave);
            poolBomb.DequeueWave(wave);

            yield return new WaitUntil(isWaveFinished);
            yield return new WaitForSecondsRealtime(3f);
        }
    }

    #region Gem Manager
    public void OnGemTouched(Gem gem)
    {
        AudioManager.Instance.Play(audioSource, "Break");
        var gemScore = Int16.Parse(gem.gameObject.name);
        AddOnGemsDestroyed(gemScore);
        score += gemScore;

        RemoveFromGemArray(gem);
        poolGem.Enqueue(gem);
    }

    public void OnGemTouched(Gem gem, bool bottomCollider)
    {
        RemoveFromGemArray(gem);
        poolGem.Enqueue(gem);
        RemoveLife();
    }

    private void AddOnGemsDestroyed(int value)
    {
        switch(value)
        {
            case 1: gemsDestroyed[0] += 1; break;
            case 5: gemsDestroyed[1] += 1; break;
            case 10: gemsDestroyed[2] += 1; break;
            case 25: gemsDestroyed[3] += 1; break;
            case 50: gemsDestroyed[4] += 1; break;
            case 100: gemsDestroyed[5] += 1; break;
        }
    }

    private void RemoveFromGemArray(Gem gem)
    {
        foreach(List<Gem> l in waveArray)
        {
            if (l.Count == 0) continue;
            if (l[0].name.Equals(gem.name))
            {
                l.Remove(gem);               
                break;
            }
        }
    }

    #endregion
    
    private void RemoveLife()
    {
        foreach(Life l in lifes)
        {
            if (l.isAlive)
            {
                l.isAlive = false;
                l.img.color = colorLifeDead;
                break;
            }
        }
        if(Array.Find(lifes, l => l.isAlive) == null)
            GameOver();               
    }

    public void OnBombTouched(Bomb bomb)
    {
        GameOver();
        poolBomb.Enqueue(bomb);
    }

    public void OnBombTouched(Bomb bomb, bool bottomCollider)
    {
        poolBomb.Enqueue(bomb);
    }

    private void SetLifeObjects()
    {
        foreach(Life l in lifes)
        {
            l.isAlive = true;
        }
        Array.ForEach(lifes, l => l.isAlive = true);
        Array.ForEach(lifes, l => l.img.color = colorLifeNormal);
    }

    private void GameOver()
    {
        gameOverText.SetActive(true);
        GameManager.Instance.GoToEnd();
    }
    
}
