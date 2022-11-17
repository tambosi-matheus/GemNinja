using System;
using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    public static MenuManager Instance;
    [SerializeField] SpriteRenderer audioSprite;
    [SerializeField] Sprite[] audioSprites;
    private bool audioOn = true;

    private void Awake() => Instance = this;
    

    public IEnumerator PlayAnimation(string name)
    {
        animator.Play(name);
        yield return new WaitForSecondsRealtime
            (animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public void ChangeAudioState()
    {
        audioOn = !audioOn;
        if (audioOn) audioSprite.sprite = audioSprites[0];
        else audioSprite.sprite = audioSprites[1];
        PlayerData.Instance.UpdateAudio(audioOn);

    }
}
