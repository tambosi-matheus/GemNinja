using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }
    public int currentScore { get; private set; } = 0;
    public int maxScore { get; private set; } = 0;
    public int[] gemsDestroyed { get; private set; } = new int[6];

    public bool audioOn { get; private set; } = true;

    private void Awake()
    {
        Instance = this;
    }

    public void OnEndRound(int finalScore, int[] _gemsDestroyed)
    {
        currentScore = finalScore;
        gemsDestroyed = _gemsDestroyed;
        if (finalScore > maxScore)
            maxScore = finalScore;
    }

    public void UpdateAudio(bool on)
    {
        audioOn = on;
        if (audioOn)
            AudioManager.Instance.Play("Background Music");
        else
            AudioManager.Instance.StopAll();
    }
}
