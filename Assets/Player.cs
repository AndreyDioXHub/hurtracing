using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public UnityEvent<PlayerInfo> OnPlayerEndMove;

    [field: SerializeField]
    public PlayerInfo Info { get; private set; }

    [SerializeField]
    private List<GameObject> _controlElements = new List<GameObject>();
    private int _speed = 1;
    private float _frame = 10f;
    private float _delay = 0.01f;

    private Vector3 _dir;
    private Vector3 _col;
    private Vector3 _end;

    private float _cossin45 = 0;

    private bool _gase = true;

    void Start()
    {
        _cossin45 = Mathf.Sqrt(2) / 2;
        _gase = true;
    }

    public void Gase()
    {
        if (_gase)
        {
            _speed++;
            _gase = false;
        }
    }

    public void CheckWhoMove(PlayerInfo info)
    {
        if(Info == info)
        {
            foreach(var ce in _controlElements)
            {
                ce.SetActive(true);
            }
        }
        else
        {
            foreach (var ce in _controlElements)
            {
                ce.SetActive(false);
            }
        }
    }

    public void SetPlayer(PlayerInfo info)
    {
        Info = info;
        Info.Speed = _speed;
    }

    public void MoveForward()
    {
        _dir = Vector2.up;
        _end = new Vector3(transform.position.x, transform.position.y + _speed, transform.position.z);
        _col = new Vector3(0, 0.6f, 0);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + _col, _dir, 0.1f);

        if (hit.collider == null)
        {
            StartCoroutine(MoveCoroutine());
        }
        else
        {
            Debug.Log($"{hit.collider.name}");
        }

    }
    public void MoveLeft()
    {
        _dir = Vector2.left;
        _end = new Vector3(transform.position.x - _speed, transform.position.y , transform.position.z);
        _col = new Vector3(-0.6f, 0, 0);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + _col, _dir, 0.1f);

        if (hit.collider == null)
        {
            StartCoroutine(MoveCoroutine());
        }
        else
        {
            Debug.Log($"{hit.collider.name}");
        }
    }
    public void MoveRight()
    {
        _dir = Vector2.right;
        _end = new Vector3(transform.position.x + _speed, transform.position.y, transform.position.z);
        _col = new Vector3(0.6f, 0, 0);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + _col, _dir, 0.1f);

        if (hit.collider == null)
        {
            StartCoroutine(MoveCoroutine());
        }
        else
        {
            Debug.Log($"{hit.collider.name}");
        }
    }

    public void MoveUpLeft()
    {
        _dir = new Vector3(-_cossin45, _cossin45, 0);

        float x = transform.position.x - _speed * _cossin45;
        float y = transform.position.y + _speed * _cossin45;
        _end = new Vector3(x, y, transform.position.z);
        _col = new Vector3(-0.6f, 0.6f, 0);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + _col, _dir, 0.1f);

        if (hit.collider == null)
        {
            StartCoroutine(MoveCoroutine());
        }
        else
        {
            Debug.Log($"{hit.collider.name}");
        }
    }

    public void MoveUpRight()
    {
        _dir = new Vector3(_cossin45, _cossin45, 0);

        float x = transform.position.x + _speed * _cossin45;
        float y = transform.position.y + _speed * _cossin45;
        _end = new Vector3(x, y, transform.position.z);
        _col = new Vector3(0.6f, 0.6f, 0);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + _col, _dir, 0.1f);

        if (hit.collider == null)
        {
            StartCoroutine(MoveCoroutine());
        }
        else
        {
            Debug.Log($"{hit.collider.name}");
        }
    }

    public IEnumerator MoveCoroutine()
    {
        float dist = Vector3.Distance(transform.position, _end);
        float distcur = dist;
        float step =  dist / _frame; 
        Vector3 prevPosition = transform.position;

        while (distcur > 0)
        {
            yield return new WaitForSeconds(_delay);
            distcur = Vector3.Distance(transform.position, _end);
            
            transform.position = Vector3.MoveTowards(transform.position, _end, step);

            RaycastHit2D hit = Physics2D.Raycast(transform.position + _col, _dir, step);

            if (hit.collider == null)
            {

            }
            else
            {
                if(hit.collider.name == "FinishLine")
                {

                }
                else
                {
                    Debug.Log($"{Info.Name} : {Info.ID} : crash:\n{transform.position}\n{prevPosition}");
                    transform.position = Vector3.MoveTowards(transform.position, prevPosition, step);
                    _speed = 1;
                    distcur = -1;
                }
            }
        }

        //transform.position = _end;
        _gase = true;
        Info.Position = transform.position;
        Info.Speed = _speed;
        OnPlayerEndMove?.Invoke(Info);
        StopCoroutine(MoveCoroutine());
    } 


    public void Crash()
    {
        transform.position = transform.position - _dir;
        _speed = 1;
        StopAllCoroutines();
    }
}

[Serializable]
public class PlayerInfo
{
    public string Name;
    public int ID;
    public float Speed;
    public Vector3 Position;
    public PlayerInfo()
    {
        Name = "";
        ID = 0;
        Speed = 0;
        Position = Vector3.zero;
    }
    public PlayerInfo(string name, int id, float speed, Vector3 position)
    {
        Name = name;
        ID = id;
        Speed = speed;
        Position = position;
    }
}
