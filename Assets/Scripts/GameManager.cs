using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private UserInfo _user;
    [SerializeField] private List<World> Worlds;

    public int PhaseIndex = 0;
    public int WorldIndex = 0;
    
    private void Awake() {
        if(FindObjectsOfType<GameManager>().Length > 1) {
            DestroyImmediate(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    private void Start() {

        if (JSONFiles.ContainsJsonFile("user"))
        {
            var tempUser = JSONFiles.LoadJsonFile<UserInfo>("user");
            if (tempUser.WorldData.Count > 0)
            {
                _user = tempUser;
            }
        }
        else {
            JSONFiles.CreateJsonFile("user");
        }
    }
    
    public void SetWorld(int indexWorld){ WorldIndex = indexWorld;}

    public List<World> GetWorlds() {
        return _user.WorldData;
    }
    
    public string GetNextPhaseId(){ return _user.WorldData[WorldIndex].Phases[PhaseIndex+1].PhaseId; }

    #region GAME

    public void PauseGame() {
        foreach(Piece piece in FindObjectsOfType(typeof(Piece))) {
            piece.CanPlay = false;
        }
    }

    public void ResumeGame() {
        foreach (Piece piece in FindObjectsOfType(typeof(Piece))) {
            piece.CanPlay = true;
        }
    }

    public void NextLevel(){
        PhaseIndex++;
        LoadScreen.instance.LoadSceneAsync(_user.WorldData[WorldIndex].Phases[PhaseIndex].PhaseId);
    }

    public void CompleteLevel() {
        _user.WorldData[WorldIndex].Phases[PhaseIndex].Completed = true;
        JSONFiles.SaveJsonFile("user", _user);
    }

    public bool LastLevel(){ return PhaseIndex >= _user.WorldData[WorldIndex].Phases.Count - 1;}

    #endregion
}

[Serializable]
public class UserInfo {
    public List<World> WorldData;

    public UserInfo(List<World> worldData) {
        WorldData = worldData;
    }
}

[Serializable]
public class World {
    public string Name;
    public List<Phase> Phases;
}

[Serializable]
public class Phase {
    public string PhaseId;
    public string Name;
    public bool Completed;
}