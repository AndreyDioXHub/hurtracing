using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BezierPath : MonoBehaviour
{
    [SerializeField]
    private int _sigmentsNumber = 20;

    [SerializeField]
    private Transform _p0;
    [SerializeField]
    private Transform _p1;
    [SerializeField]
    private Transform _p2;
    [SerializeField]
    private Transform _p3;

    [Range(0,1)]
    public float t;
    [SerializeField]
    private List<Vector3> _points = new List<Vector3>();
    void Update()
    {
       //transform.position = Bezier.GetPoint(P0.position, P1.position, P2.position, P3.position, t);
        //transform.rotation = Quaternion.LookRotation(Bezier.GetFirstDerivative(P0.position, P1.position, P2.position, P3.position, t));
    }

    [ContextMenu("add points")]
    public void AddPoints()
    {
        Transform[] points = gameObject.GetComponentsInChildren<Transform>();
        _p0 = points[1];
        _p1 = points[2];
        _p2 = points[3];
        _p3 = points[4];
    }

    public List<Vector3> GetPoints(int sigmentsNumber = 20)
    {
        _points = new List<Vector3>();
        Vector3 preveousePoint = _p0.position;
        _points.Add(preveousePoint);

        for (int i = 0; i < sigmentsNumber + 1; i++)
        {
            float paremeter = (float)i / sigmentsNumber;
            _points.Add(Bezier.GetPoint(_p0.position, _p1.position, _p2.position, _p3.position, paremeter));
        }

        return _points;
    }

    private void OnDrawGizmos() 
    {
        Vector3 preveousePoint = _p0.position;

        for (int i = 0; i < _sigmentsNumber + 1; i++) {
            float paremeter = (float)i / _sigmentsNumber;
            Vector3 point = Bezier.GetPoint(_p0.position, _p1.position, _p2.position, _p3.position, paremeter);
            Gizmos.DrawLine(preveousePoint, point);
            preveousePoint = point;
        }

    }

}
