using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private enum Functions
    {GoToMenu,
    GoToMain,
    Audio};

    [SerializeField] private Functions selectedFunction;
    private Dictionary<Functions, System.Action> function;

    private GameManager manager;

    private void Awake()
    {
        function = new Dictionary<Functions, System.Action>()
        {{ Functions.GoToMenu, GoToMenu },
        { Functions.GoToMain, GoToMain },
        { Functions.Audio, ChangeAudio} };
    }

    private void Start()
    {

        manager = GameManager.Instance;
    }

    private void Update()
    {
        if (Input.touchCount != 0)
        {
            var touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Ended)
            {
                var touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0;
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector3.forward);

                if (hit.collider != null && hit.collider.gameObject == gameObject)                
                    ExecuteFunction();                
            }
        }

        
    }

    public void ExecuteFunction() => function[selectedFunction].Invoke();

    private void GoToMenu() => manager.GoToMenu(); 

    private void GoToMain() => manager.GoToMain();

    private void ChangeAudio() => MenuManager.Instance.ChangeAudioState();
}