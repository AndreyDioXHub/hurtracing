using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Players : MonoBehaviour
{
    public static Players Instance;

    public UnityEvent<PlayerInfo> OnPlayerHasChanged;

    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField]
    private List<PlayerInfo> _playersInfo = new List<PlayerInfo>();
    [SerializeField]
    private List<PlayerInfo> _sortedPlayers = new List<PlayerInfo>();
    [SerializeField]
    private float _playerSpace = 2;
    [SerializeField]
    private int _playerID = 0;
    private List<GameObject> _players = new List<GameObject>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void NextPlayer(PlayerInfo info)
    {
        _playerID = _playersInfo.FindIndex(i => i.ID == info.ID);
        _playersInfo[_playerID] = info;

        _playerID++;

        if (_playerID > _playersInfo.Count - 1)
        {
            _playerID = 0;
        }

        _sortedPlayers = CalculateWinersTable();


        UIController.Instance.UpdatePlayerName($"{_sortedPlayers.FindIndex(sp => sp.ID == _playersInfo[_playerID].ID) + 1}: {_playersInfo[_playerID].Name} : {_playersInfo[_playerID].Speed}");

        OnPlayerHasChanged?.Invoke(_playersInfo[_playerID]);
    }

    public List<PlayerInfo> CalculateWinersTable()
    {
        List<PlayerInfo> sortedPlayers = new List<PlayerInfo>();

        foreach (var p in _playersInfo)
        {
            PlayerInfo playerInfo = new PlayerInfo(p.Name, p.ID, p.Speed, p.Position);
            sortedPlayers.Add(playerInfo);
        }

        PlayerInfo temp = new PlayerInfo();

        for (int write = 0; write < sortedPlayers.Count; write++)
        {
            for (int sort = 0; sort < sortedPlayers.Count - 1; sort++)
            {
                if (sortedPlayers[sort].Position.y > sortedPlayers[sort + 1].Position.y)
                {
                    temp = sortedPlayers[sort + 1];
                    sortedPlayers[sort + 1] = sortedPlayers[sort];
                    sortedPlayers[sort] = temp;
                }
            }
        }

        sortedPlayers.Reverse();
        return sortedPlayers;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartPlayers()
    {
        float startposition = _playerSpace * _playersInfo.Count / 2;

        for (int i = 0; i < _playersInfo.Count; i++)
        {
            _playersInfo[i].ID = i;
            _players[i].name = _playersInfo[i].Name;
            Player p = _players[i].GetComponent<Player>();
            _playersInfo[i].Speed = 1;
            p.SetPlayer(_playersInfo[i]);

            float playerposition = -startposition + _playerSpace * i;
            _playersInfo[i].Position = new Vector3(playerposition, 0, 0);
            //_playersInfo[i].Speed = 1;
            _players[i].transform.position = _playersInfo[i].Position;

           /* OnPlayerHasChanged.AddListener(p.CheckWhoMove);
            p.OnPlayerEndMove.AddListener(NextPlayer);*/

            p.CheckWhoMove(_playersInfo[0]);
        }

    }

    [ContextMenu("Spawn players")]
    public void SpawnPlayers()
    {
        float startposition = _playerSpace * _playersInfo.Count/2;

        for (int i=0 ; i< _playersInfo.Count; i++)
        {
            _playersInfo[i].ID = i;
            GameObject playerGO = Instantiate(_playerPrefab);
            playerGO.name = _playersInfo[i].Name;
            Player p = playerGO.GetComponent<Player>();
            _playersInfo[i].Speed = 1;
            p.SetPlayer(_playersInfo[i]);
            _players.Add(playerGO);

            float playerposition = -startposition + _playerSpace * i;
            _playersInfo[i].Position = new Vector3(playerposition, 0, 0);
            playerGO.transform.position = _playersInfo[i].Position;

            OnPlayerHasChanged.AddListener(p.CheckWhoMove);
            p.OnPlayerEndMove.AddListener(NextPlayer);

            p.CheckWhoMove(_playersInfo[0]);
        }

        UIController.Instance.UpdatePlayerName(_playersInfo[0].Name);
        OnPlayerHasChanged?.Invoke(_playersInfo[0]);
        // NextPlayer(Vector3.zero);
    }
}
