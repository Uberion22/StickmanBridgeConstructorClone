using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider2D;

    private Vector3 _startPos;
    private float _repeatWidth;

    void Start()
    {
        _startPos = transform.position;
        _repeatWidth = _boxCollider2D.size.x * transform.lossyScale.x / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < _startPos.x - _repeatWidth)
        {
            transform.position = _startPos;
        }
    }
}
