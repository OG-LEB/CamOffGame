using UnityEngine;

public class WindowController : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private GameObject PlayWindow;
    [SerializeField] private GameObject PauseWindow;
    [SerializeField] private GameObject MainMenuWindow;
    [SerializeField] private GameObject GameOverWindow;
    [SerializeField] private GameObject NoteWindow;
    [SerializeField] private GameObject EndGameWindow;
    private void Start()
    {
        PlayWindow.SetActive(false);
        PauseWindow.SetActive(false);
        MainMenuWindow.SetActive(false);
        GameOverWindow.SetActive(false);
        NoteWindow.SetActive(false);
        EndGameWindow.SetActive(false);
    }
    public void MainMenuSetState(bool state) 
    {
        MainMenuWindow.SetActive(state);
    }
    public void PlayWindowSetActiveState(bool state) 
    { 
        PlayWindow.SetActive(state); 
    }
    public void PauseWindowSetActiveState(bool state) 
    { 
        PauseWindow.SetActive(state); 
    }
    public void GameOverSetActiveState(bool state)
    {
        GameOverWindow.SetActive(state);
    }
    public void NoteWinSetActiveState(bool state)
    {
        NoteWindow.SetActive(state);
    }
    public void EndGameWinSetActiveState(bool state)
    {
        EndGameWindow.SetActive(state);
    }
}
