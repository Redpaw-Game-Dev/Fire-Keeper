using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDAnimation : MonoBehaviour
{

    public List<Vector2> _points;
    private int _currentIndex = 0;
    public float _animSpeed = 0.5f;
    public bool _reverse = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AnimCoroutine());
    }

    IEnumerator AnimCoroutine()
    {
        for (float i = 0; ; i += Time.deltaTime * _animSpeed)
        {
            if (!_reverse)
            {
                if (_currentIndex < _points.Count - 1)
                {
                    transform.position = Vector3.Lerp(new Vector3(_points[_currentIndex].x, _points[_currentIndex].y, transform.position.z), new Vector3(_points[_currentIndex + 1].x, _points[_currentIndex + 1].y, transform.position.z), i);
                    if (i >= 1)
                    {
                        i = 0;
                        _currentIndex++;
                    }
                }
                else
                {
                    transform.position = Vector3.Lerp(new Vector3(_points[_currentIndex].x, _points[_currentIndex].y, transform.position.z), new Vector3(_points[0].x, _points[0].y, transform.position.z), i);
                    if (i >= 1)
                    {
                        i = 0;
                        _currentIndex = 0;
                    }
                }
            }
            else
            {
                if (_currentIndex > 0)
                {
                    transform.position = Vector3.Lerp(new Vector3(_points[_currentIndex].x, _points[_currentIndex].y, transform.position.z), new Vector3(_points[_currentIndex - 1].x, _points[_currentIndex - 1].y, transform.position.z), i);
                    if (i >= 1)
                    {
                        i = 0;
                        _currentIndex--;
                    }
                }
                else
                {
                    transform.position = Vector3.Lerp(new Vector3(_points[_currentIndex].x, _points[_currentIndex].y, transform.position.z), new Vector3(_points[_points.Count - 1].x, _points[_points.Count - 1].y, transform.position.z), i);
                    if (i >= 1)
                    {
                        i = 0;
                        _currentIndex =  _points.Count - 1;
                    }
                }
            }
            yield return null;
        }
    }
}
