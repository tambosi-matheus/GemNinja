using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    [SerializeField] private GameObject Menu, Main, End;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private EndScreenManager endScreenManager;
    public enum GameStates { Menu, Main, EndScreen };
    public GameStates gameState = GameStates.Menu;
    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
        WaveGenerator.SetAim
            (new Vector2( // Spawn
        Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * 0.05f, 0)).x,
        Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * 0.95f, 0)).x),
        Camera.main.ScreenToWorldPoint(
            new Vector2( // Aim
        Camera.main.pixelWidth / 2, Camera.main.pixelHeight * 1.2f)));
    }
    void Start()
    {
        Menu.SetActive(true);
        Main.SetActive(false);
        End.SetActive(false);
    }

    public void GoToMain() => StartCoroutine(ChangeGameState(GameStates.Main));    

    public void GoToMenu() => StartCoroutine(ChangeGameState(GameStates.Menu));    

    public void GoToEnd() => StartCoroutine(ChangeGameState(GameStates.EndScreen));
    

    public IEnumerator ChangeGameState(GameStates newState)
    {
        if(gameState == newState) yield break;
        var pastState = gameState;
        gameState = newState;
        switch (pastState)
        {
            case GameStates.Menu: yield return StartCoroutine(OnMenuExit()); break;
            case GameStates.Main: yield return StartCoroutine(OnMainExit()); break;
            case GameStates.EndScreen: yield return StartCoroutine(OnEndScreenExit()); break;
        }
        switch (newState)
        {
            case GameStates.Menu: yield return StartCoroutine(OnMenuEnter()); break;
            case GameStates.Main: yield return StartCoroutine(OnMainEnter()); break;
            case GameStates.EndScreen: yield return StartCoroutine(OnEndScreenEnter()); break;
        }
    }

    private IEnumerator OnMenuEnter()
    {
        Menu.SetActive(true);
        yield return StartCoroutine(menuManager.PlayAnimation("MenuIn"));
    }

    private IEnumerator OnMenuExit()
    {
        yield return StartCoroutine(menuManager.PlayAnimation("MenuOut"));
        Menu.SetActive(false);
    }

    private IEnumerator OnMainEnter()
    {
        Main.SetActive(true);
        yield break;
    }

    private IEnumerator OnMainExit()
    {
        Time.timeScale = 0.1f;
        yield return new WaitUntil(InGameManager.Instance.waitGameOver);
        Time.timeScale = 1f;
        Main.SetActive(false);
        yield break;
    }

    private IEnumerator OnEndScreenEnter()
    {
        End.SetActive(true);
        yield return StartCoroutine(endScreenManager.PlayAnimation("MenuIn"));
    }

    private IEnumerator OnEndScreenExit()
    {
        yield return StartCoroutine(endScreenManager.PlayAnimation("MenuOut"));
        End.SetActive(false);
    }        
}
