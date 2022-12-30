using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private PlayerInfo _info = new PlayerInfo();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SwitchCam(PlayerInfo info)
    {
        _info = info;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _info.Position, _speed * _info.Speed * Time.deltaTime);
    }
}
