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
    private List<PlayerInfo> _players = new List<PlayerInfo>();
    [SerializeField]
    private List<PlayerInfo> _sortedPlayers = new List<PlayerInfo>();
    [SerializeField]
    private float _playerSpace = 2;
    [SerializeField]
    private int _playerID = 0;

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
        _playerID = _players.FindIndex(i => i.ID == info.ID);
        _players[_playerID] = info;

        _playerID++;

        if (_playerID > _players.Count - 1)
        {
            _playerID = 0;
        }

        _sortedPlayers = CalculateWinersTable();


        UIController.Instance.UpdatePlayerName($"{_sortedPlayers.FindIndex(sp => sp.ID == _players[_playerID].ID) + 1}: {_players[_playerID].Name} : {_players[_playerID].Speed}");

        OnPlayerHasChanged?.Invoke(_players[_playerID]);
    }

    public List<PlayerInfo> CalculateWinersTable()
    {
        List<PlayerInfo> sortedPlayers = new List<PlayerInfo>();

        foreach (var p in _players)
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

    [ContextMenu("Spawn players")]
    public void SpawnPlayers()
    {
        float startposition = _playerSpace * _players.Count/2;

        for (int i=0 ; i< _players.Count; i++)
        {
            _players[i].ID = i;
            GameObject playerGO = Instantiate(_playerPrefab);
            playerGO.name = _players[i].Name;
            Player p = playerGO.GetComponent<Player>();
            p.SetPlayer(_players[i]);

            float playerposition = -startposition + _playerSpace * i;
            _players[i].Position = new Vector3(playerposition, 0, 0);
            playerGO.transform.position = _players[i].Position;

            OnPlayerHasChanged.AddListener(p.CheckWhoMove);
            p.OnPlayerEndMove.AddListener(NextPlayer);

            p.CheckWhoMove(_players[0]);
        }

        UIController.Instance.UpdatePlayerName(_players[0].Name);
        OnPlayerHasChanged?.Invoke(_players[0]);
        // NextPlayer(Vector3.zero);
    }
}
