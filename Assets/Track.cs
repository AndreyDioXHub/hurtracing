using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _patterns = new List<GameObject>();
    [SerializeField]
    private GameObject _finishLine;
    [SerializeField]
    private GameObject _block;

    [SerializeField]
    private float _trackLenght = 20;
    [SerializeField]
    private float _trackWidth = 5;

    [SerializeField]
    private float _endposition = 0;

    [SerializeField]
    private int _step = 0;
    [SerializeField]
    private int _prevIndex = 0;

    [SerializeField]
    private List<GameObject> _trackParts = new List<GameObject>();


    [ContextMenu("Create track part")]
    public void CreateTrackPart()
    {
        _step = 0;
        _prevIndex = 0;
        _endposition = 0;

        int iteration = 0;

        while(iteration < _trackLenght)
        {
            List<GameObject> suitspatterns = new List<GameObject>();

            for (int i = 0; i < _patterns.Count; i++)
            {
                Transform[] points = _patterns[i].GetComponentsInChildren<Transform>();
                float startposition = points[1].position.x;

                if (startposition == _endposition)
                {
                    suitspatterns.Add(_patterns[i]);
                }
            }

            int randomindexsub = Random.Range(0, suitspatterns.Count - 1);

            if (_prevIndex == randomindexsub)
            {
                while (_prevIndex == randomindexsub)
                {
                    randomindexsub = Random.Range(0, suitspatterns.Count - 1);
                }
            }

            _prevIndex = randomindexsub;

            GameObject patternsub = Instantiate(suitspatterns[randomindexsub]);

            Debug.Log($"{suitspatterns.Count}: {randomindexsub}");

            Transform[] pointssub = patternsub.GetComponentsInChildren<Transform>();
            _endposition = pointssub[4].position.x;

            List<Vector3> bezierpath = patternsub.GetComponent<BezierPath>().GetPoints(40);

            foreach (var bezierpoint in bezierpath)
            {
                GameObject block = Instantiate(_block);
                block.transform.position = bezierpoint;
                block.transform.SetParent(patternsub.transform);
            }

            GameObject patternsubsecond = Instantiate(patternsub);

            patternsub.transform.position = new Vector3(_trackWidth / 2, _step * 10, 0);
            patternsubsecond.transform.position = new Vector3(-_trackWidth / 2, _step * 10, 0);

            GameObject trackPart = new GameObject();
            trackPart.name = $"trackPart ({_step})";
            patternsub.transform.SetParent(trackPart.transform);
            patternsubsecond.transform.SetParent(trackPart.transform);
            _trackParts.Add(trackPart);
            _step++;
            iteration++;

            //Debug.Log($"{p.name}: {ch[1].position.x} : {ch[4].position.x}");
        }

        GameObject finishLine = Instantiate(_finishLine);

        finishLine.transform.position = new Vector3(_endposition, _trackLenght*10, 0);
        finishLine.transform.localScale = new Vector3(_trackWidth, 0.5f, 0.5f);
        finishLine.name = "FinishLine";
    }

    [ContextMenu("Clear track part")]
    public void ClearTrackPart()
    {
        _step = 0;
        _prevIndex = 0;
        _endposition = 0;

        foreach (var tp in _trackParts)
        {
            Destroy(tp);
        }

        _trackParts = null;
        _trackParts = new List<GameObject>();
    }
}
