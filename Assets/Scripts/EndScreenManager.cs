using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro score, newMax, 
        gem1, gem5, gem10, gem25, gem50, gem100;
    [SerializeField] private Animator animator;
    public PlayerData data;
    Func<bool> animIdle;

    private void Start()
    {
        animIdle = () => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        data = PlayerData.Instance;
    }
    public IEnumerator AnimIn()
    {
        transform.position = Vector3.zero;
        animator.Play("MenuIn");
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public IEnumerator PlayAnimation(string name)
    {
        transform.position = Vector3.zero;
        animator.Play(name);
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
    }


    private void OnEnable()
    {
        score.SetText(data.currentScore.ToString());
        if (data.maxScore == data.currentScore)
            newMax.enabled = true;
        else
            newMax.enabled = false;
        var gems = data.gemsDestroyed;
        gem1.SetText(gems[0].ToString());
        gem5.SetText(gems[1].ToString());
        gem10.SetText(gems[2].ToString());
        gem25.SetText(gems[3].ToString());
        gem50.SetText(gems[4].ToString());
        gem100.SetText(gems[5].ToString());

    }
}
