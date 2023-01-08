using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    public static Game Instance;

    public UnityEvent<PlayerInfo> OnPlayerWin;
    [SerializeField]
    private Track _track;
    [SerializeField]
    private Players _players;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        StartGame();

    }
    public void RestartGame()
    {
        _track.ClearTrackPart();
        _track.CreateTrackPart();
        _players.RestartPlayers();
    }

    public void StartGame()
    {
        _track.CreateTrackPart();
        _players.SpawnPlayers();
    }
}
