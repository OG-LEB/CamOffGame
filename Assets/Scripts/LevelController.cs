using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.AI;

public class LevelController : MonoBehaviour
{
    private static LevelController instance;
    public static LevelController GetInstance() { return instance; }
    private void Awake()
    {
        instance = this;
    }
    [Header("Links")]
    [SerializeField] private WindowController windowController;
    [SerializeField] private SoundController soundController;
    [SerializeField] private PlayUIController _PlayUIController;
    [SerializeField] private CameraZoom _CameraZoom;
    [SerializeField] private PauseMenuController pauseMenuController;

    [Space]
    [Header("Camera")]
    [SerializeField] private GameObject MainMenuCamera;
    [SerializeField] private GameObject PlayerCamera;
    [Space]
    [Header("Location Objects")]
    [SerializeField] private GameObject Player;
    [SerializeField] private Transform PlayerSpawnPosition;
    [Space]

    [Header("GameData")]
    [SerializeField] private int FilmObjectsCounter;
    [SerializeField] private int AmountOfFilmObjects;
    [SerializeField] private FilmObject[] FilmObjects;
    [SerializeField] private GameState CurrentGameState;
    [SerializeField] private GameObject VokzalGuy;
    [SerializeField] private int Health;
    [SerializeField] private ScaryLocationSound _ScarySound;
    private bool isGameOver;
    private bool canWeUnlockPauseByTab;
    [SerializeField] private Transform[] VokzalGuySpawnPoints;
    [Space]
    [Header("EndGame")]
    [SerializeField] private GameObject EndGameTrigger;
    [SerializeField] private GameObject EndGameKalitkaWithMaterial;
    [SerializeField] private GameObject BossFightTrigger;
    [SerializeField] private GameObject SpaceText;
    private bool EndCutScene = false;
    [SerializeField] private GameObject BossFightWall;
    [SerializeField] private Transform BossFightVokzalGuyTeleportPosition;
    [SerializeField] private GameObject BossFightCameraEffect;
    [SerializeField] private VideoPlayer _videoPlayer;
    [Space]
    [Header("CameraScars")]
    [SerializeField] private GameObject CameraScar_0;
    [SerializeField] private GameObject CameraScar_0pause;
    [SerializeField] private GameObject CameraScar_0note;
    [SerializeField] private GameObject CameraScar_1;
    [SerializeField] private GameObject CameraScar_1pause;
    [SerializeField] private GameObject CameraScar_1note;
    [Space]
    [Header("Notes")]
    [SerializeField] private bool isNoteOpen = false;
    [SerializeField] private GameObject NoteESign;


    public enum GameState { playing, pause }
    //public int GetFillObjectsAmount() { return FilmObjects.Length; }
    public bool GetPauseState()
    {
        if (CurrentGameState == GameState.pause)
            return true;
        else
            return false;
    }
    public int GetFilmObjectsCount() { return FilmObjectsCounter; }
    private void Start()
    {
        GameAwake();
        AmountOfFilmObjects = FilmObjects.Length;
        UpdateFilmObjectsCounter();
        NoteESign.SetActive(false);
    }
    private void GameAwake()
    {
        VokzalGuy.SetActive(false);
        windowController.MainMenuSetState(true);
        Player.SetActive(false);
        MainMenuCamera.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1f;

        FilmObjectsCounter = 0;
        foreach (var obj in FilmObjects)
        {
            obj.Restart();
        }
        LocationScanSystem.GetInstance().Restart();
        Health = 2;
        //VokzalGuySpawned = false;
        CameraScar_0.SetActive(false);
        CameraScar_0pause.SetActive(false);
        CameraScar_1.SetActive(false);
        CameraScar_1pause.SetActive(false);
        CameraScar_0note.SetActive(false);
        CameraScar_1note.SetActive(false);
        isGameOver = true;
        NoteDataScript.GetInstance().Restart();
        CurrentGameState = GameState.pause;
        _CameraZoom.Restart();
        EndGameTrigger.SetActive(false);
        BossFightTrigger.SetActive(false);
        EndCutScene = false;
        soundController.PlayMainMenuSoundTrack();
        Player.GetComponent<CameraShootingScript>().Restart();
        EndGameKalitkaWithMaterial.GetComponent<MeshRenderer>().materials[1].SetFloat("_Scale", 1);
        BossFightWall.SetActive(false);
        soundController.SetupDefaultChase();
        BossFightCameraEffect.SetActive(false);
    }
    private void Update()
    {
        if (CurrentGameState == GameState.playing)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                PauseButton();
            }
            RaycastHit hit;
            Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.forward * 5, Color.white);
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, 5))
            {
                if (hit.transform.CompareTag("Note"))
                {
                    float distanceToNote = Vector3.Distance(PlayerCamera.transform.position, hit.transform.position);
                    if (distanceToNote <= 5)
                    {
                        NoteESign.SetActive(true);
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            hit.transform.GetComponent<Note>().Open();
                        }
                    }
                    else
                    {
                        NoteESign.SetActive(false);
                    }
                }
                else
                {
                    NoteESign.SetActive(false);
                }
            }
            else
            {
                NoteESign.SetActive(false);
            }
            if (Input.mouseScrollDelta.y > 0.1f)
            {
                _CameraZoom.ZoomUp();
            }
            else if (Input.mouseScrollDelta.y < -0.1f)
            {
                _CameraZoom.ZoomDown();
            }

        }
        else if (CurrentGameState == GameState.pause && isNoteOpen && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)))
        {
            CloseNote();
        }
        else if (CurrentGameState == GameState.pause && Input.GetKeyDown(KeyCode.Tab) && canWeUnlockPauseByTab)
        {
            PauseButton();
        }
        if (CurrentGameState == GameState.pause && EndCutScene)
        {
            //if (Input.GetKeyDown(KeyCode.Space) && _videoPlayer.frame > 120)
            if (Input.GetKeyDown(KeyCode.Space) && SpaceText.activeSelf)
            {
                //_videoPlayer.Stop();
                EndCutScene = false;
                windowController.EndGameWinSetActiveState(false);
                RestartGame();
                Debug.Log("Stop");
            }
            if (_videoPlayer.frame > 120)
            {
                SpaceText.SetActive(true);
            }
            if (!_videoPlayer.isPlaying && _videoPlayer.frame > 60)
            {
                //_videoPlayer.Stop();
                EndCutScene = false;
                windowController.EndGameWinSetActiveState(false);
                RestartGame();
                Debug.Log("End");
            }
        }
    }
    public void PauseButton()
    {
        if (!isGameOver)
        {
            //pause = !pause;
            if (CurrentGameState == GameState.playing)
            {
                CurrentGameState = GameState.pause;
            }
            else
            {
                CurrentGameState = GameState.playing;
            }
            soundController.PlayButtonSound();

            switch (CurrentGameState)
            {
                case GameState.playing:
                    {
                        windowController.PlayWindowSetActiveState(true);
                        windowController.PauseWindowSetActiveState(false);
                        Time.timeScale = 1f;
                        soundController.UnpauseSounds();
                        Cursor.lockState = CursorLockMode.Locked;
                        canWeUnlockPauseByTab = false;
                        break;
                    }
                case GameState.pause:
                    {
                        windowController.PlayWindowSetActiveState(false);
                        windowController.PauseWindowSetActiveState(true);
                        Time.timeScale = 0f;
                        Cursor.lockState = CursorLockMode.Confined;
                        soundController.PauseSounds();
                        canWeUnlockPauseByTab = true;
                        pauseMenuController.OpenPauseWindow();

                        break;
                    }
            }
        }
    }
    public void PlayButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        UpdateFilmObjectsCounter();
        MainMenuCamera.SetActive(false);
        Player.transform.position = PlayerSpawnPosition.position;
        Player.transform.rotation = PlayerSpawnPosition.rotation;
        Player.SetActive(true);
        windowController.MainMenuSetState(false);
        windowController.PlayWindowSetActiveState(true);
        soundController.PlayButtonSound();
        //_PlayerCameraRotation.ResetAxis();
        Player.GetComponent<FirstPersonController>().ResetMouse();
        //Location sounds
        CurrentGameState = GameState.playing;
        isGameOver = false;
        soundController.StopMainMenuSoundTrack();
        soundController.StartExploreSound();
    }
    public void RestartGame()
    {
        soundController.PlayButtonSound();
        CurrentGameState = GameState.pause;
        windowController.PlayWindowSetActiveState(false);
        windowController.PauseWindowSetActiveState(false);
        windowController.GameOverSetActiveState(false);
        soundController.PauseSounds();
        soundController.EndGameCutScene();
        soundController.Restart();
        //Player.GetComponent<PlayerMovement>().RestartStamina();
        Player.GetComponent<FirstPersonController>().RestartStamina();
        GameAwake();

    }
    public void EmptyButton()
    {
        soundController.PlayButtonSound();
    }
    private void UpdateFilmObjectsCounter()
    {
        _PlayUIController.UpdateObjectsCounterData(FilmObjectsCounter, AmountOfFilmObjects);
    }
    public void NewFilmObject()
    {
        FilmObjectsCounter++;
        if (FilmObjectsCounter == 1)
        {
            SpawnVokzalGuy();
        }
        soundController.PlayCollectSound();
        if (FilmObjectsCounter >= AmountOfFilmObjects)
        {
            _PlayUIController.UpdateObjectsCounterToFull();
        }
        else
        {
            UpdateFilmObjectsCounter();
        }
    }
    public void SpawnVokzalGuy()
    {
        if (!VokzalGuy.activeSelf)
        {
            VokzalGuy.transform.position = VokzalGuySpawnPoints[Random.Range(0, VokzalGuySpawnPoints.Length)].position;
            VokzalGuy.SetActive(true);
            VokzalGuy.GetComponent<VokzalGuyScript>().StartMotion();
            //Debug.Log("Spawned vokzalGuy");
            VokzalGuy.GetComponent<VokzalGuyScript>().Restart();
        }
    }
    public bool GetVokzalGuySeePlayerState() { return VokzalGuy.GetComponent<VokzalGuyScript>().GetSeePlayerState(); }

    public void SpawnScarySound(Vector3 position)
    {
        _ScarySound.transform.position = position;
        _ScarySound.Play();
    }
    public void TeleportVokzalGuy(Vector3 position)
    {
        if (!VokzalGuy.activeSelf)
        {
            VokzalGuy.SetActive(true);
        }
        NavMeshAgent agent = VokzalGuy.GetComponent<NavMeshAgent>();
        agent.gameObject.SetActive(false);
        VokzalGuy.transform.position = position;
        agent.gameObject.SetActive(true);
        VokzalGuy.GetComponent<VokzalGuyScript>().TeleportationPlayerDetection();
    }

    public void HitPlayer()
    {
        //Player.GetComponent<PlayerMovement>().Hit();
        Player.GetComponent<FirstPersonController>().Hit();
        switch (Health)
        {
            case 2:
                {
                    CameraScar_0.SetActive(true);
                    CameraScar_0pause.SetActive(true);
                    CameraScar_0note.SetActive(true);
                    soundController.PlayGlassBreakSound();
                    break;
                }
            case 1:
                {
                    CameraScar_1.SetActive(true);
                    CameraScar_1pause.SetActive(true);
                    CameraScar_1note.SetActive(true);
                    soundController.PlayGlassBreakSound();
                    break;
                }
            case 0:
                {
                    GameOver();
                    soundController.PlayGlassBreakSound();
                    return;
                }
        }
        Health--;
    }
    public void GameOver()
    {
        CurrentGameState = GameState.pause;
        windowController.PlayWindowSetActiveState(false);
        windowController.PauseWindowSetActiveState(false);
        windowController.GameOverSetActiveState(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        soundController.PauseSounds();
        isGameOver = true;
        soundController.DisablePlaySoundTrack();
        VokzalGuy.SetActive(false);
    }
    public void OpenNote()
    {
        windowController.PlayWindowSetActiveState(false);
        windowController.PauseWindowSetActiveState(false);
        windowController.NoteWinSetActiveState(true);
        CurrentGameState = GameState.pause;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        isNoteOpen = true;
        soundController.PlayNoteSound();
        soundController.PauseSounds();
    }
    public void CloseNote()
    {
        windowController.NoteWinSetActiveState(false);
        windowController.PlayWindowSetActiveState(true);
        CurrentGameState = GameState.playing;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        isNoteOpen = false;
        soundController.PlayNoteSound();
        soundController.UnpauseSounds();
    }
    public Transform GetPlayerTransform() { return Player.transform; }
    public void TurnOnEndGameTrigger()
    {
        if (!EndGameTrigger.activeSelf)
        {
            EndGameTrigger.SetActive(true);
            EndGameKalitkaWithMaterial.GetComponent<MeshRenderer>().materials[1].SetFloat("_Scale", 1.1f);
            BossFightTrigger.SetActive(true);

        }
    }
    public void EndGame()
    {
        Debug.Log("EndGame Cutscene");
        VokzalGuy.SetActive(false);
        Player.SetActive(false);
        MainMenuCamera.SetActive(true);
        CurrentGameState = GameState.pause;
        windowController.PlayWindowSetActiveState(false);
        windowController.PauseWindowSetActiveState(false);
        windowController.EndGameWinSetActiveState(true);

        //Play CutScene
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "EndGameCutScene.mp4");
        _videoPlayer.url = videoPath;
        _videoPlayer.Play();

        EndCutScene = true;
        SpaceText.SetActive(false);
        soundController.PauseSounds();
        soundController.EndGameCutScene();
    }
    public void LoadNocLip() { SceneManager.LoadScene(1); }
    public void BossFight(bool isChasing)
    {
        if (!isChasing)
        {
            BossFightWall.SetActive(true);
            TeleportVokzalGuy(BossFightVokzalGuyTeleportPosition.position);
        }
        soundController.SetupBossFightChase();
        BossFightCameraEffect.SetActive(true);
        _PlayUIController.BossFightText();
        Debug.Log("BossFightTrigger");
    }

    //Test
    public void SetSevenFilmObjects()
    {
        FilmObjectsCounter = 7;
    }
}
