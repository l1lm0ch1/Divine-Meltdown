using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public PlayerState playerState;
    public ProgressState progressState;

    public GameData()
    {
        playerState = new PlayerState();
        progressState = new ProgressState();
    }
}

[System.Serializable]
public class PlayerState
{
    public float[] position = new float[3];
    public int health = 3;
    public int pinsCollected = 0;
    public int sceneIndex = 1;
}

[System.Serializable]
public class ProgressState
{
    public bool bossDefeated = false;
    public List<string> completedPuzzles = new List<string>();
    public List<string> openDoors = new List<string>();
    public List<string> watchedCutscenes = new List<string>();
}