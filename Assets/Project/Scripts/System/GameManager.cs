using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameData gameData;
    private PlayerController playerController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (gameData == null)
            return;

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject == null)
            return;

        playerController = playerObject.GetComponent<PlayerController>();
        if (playerController == null)
            return;

        ApplyPlayerState();
    }

    private void ApplyPlayerState()
    {
        PlayerState state = gameData.playerState;
        playerController.health = state.health;
        playerController.pinsCollected = state.pinsCollected;
        playerController.transform.position = new Vector3(
            state.position[0],
            state.position[1],
            state.position[2]
        );
    }

    public void StartNewGame()
    {
        gameData = new GameData();
        SaveSystem.SaveGame(gameData);
        SceneManager.LoadScene("Tutorial");
    }

    public void ContinueGame()
    {
        gameData = SaveSystem.LoadGame();

        if (gameData == null)
        {
            StartNewGame();
            return;
        }

        SceneManager.LoadScene(gameData.playerState.sceneIndex);
    }

    public void SaveGame()
    {
        if (gameData == null || playerController == null)
            return;

        gameData.playerState.health = playerController.health;
        gameData.playerState.pinsCollected = playerController.pinsCollected;
        gameData.playerState.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        gameData.playerState.position[0] = playerController.transform.position.x;
        gameData.playerState.position[1] = playerController.transform.position.y;
        gameData.playerState.position[2] = playerController.transform.position.z;

        SaveSystem.SaveGame(gameData);
    }

    public void CompletePuzzle(string puzzleId)
    {
        if (gameData.progressState.completedPuzzles.Contains(puzzleId))
            return;

        gameData.progressState.completedPuzzles.Add(puzzleId);
        SaveGame();
    }

    public void OpenDoor(string doorId)
    {
        if (gameData.progressState.openDoors.Contains(doorId))
            return;

        gameData.progressState.openDoors.Add(doorId);
        SaveGame();
    }

    public void WatchCutscene(string cutsceneId)
    {
        if (gameData.progressState.watchedCutscenes.Contains(cutsceneId))
            return;

        gameData.progressState.watchedCutscenes.Add(cutsceneId);
        SaveGame();
    }

    public void DefeatBoss()
    {
        gameData.progressState.bossDefeated = true;
        SaveGame();
    }

    public bool IsPuzzleComplete(string puzzleId) =>
        gameData?.progressState.completedPuzzles.Contains(puzzleId) ?? false;

    public bool IsDoorOpen(string doorId) =>
        gameData?.progressState.openDoors.Contains(doorId) ?? false;

    public bool IsCutsceneWatched(string cutsceneId) =>
        gameData?.progressState.watchedCutscenes.Contains(cutsceneId) ?? false;

    public bool IsBossDefeated() =>
        gameData?.progressState.bossDefeated ?? false;

    public bool HasSaveFile() =>
        SaveSystem.SaveExists();
}