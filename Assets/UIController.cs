using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField]
    private TextMeshProUGUI _playerName;

    [SerializeField]
    private GameObject _finishPanel;
    [SerializeField]
    private TextMeshProUGUI _finishText;

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

    public void UpdatePlayerName(string name)
    {
        _playerName.text = name;
    }

    public void ShowFinishPanel(PlayerInfo info)
    {
        _finishPanel.SetActive(true);
        _finishText.text = $"Player: {info.Name} - win!";
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
