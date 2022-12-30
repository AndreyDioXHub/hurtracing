using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private Track _track;
    // Start is called before the first frame update
    void Start()
    {
        //StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        _track.CreateTrackPart();

    }
}
