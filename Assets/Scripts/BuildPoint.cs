using UnityEngine;

public class BuildPoint : MonoBehaviour
{
    private PlatformController _groundController;
    // Start is called before the first frame update
    void Start()
    {
        _groundController = GetComponentInParent<PlatformController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(_groundController.BuildInProgress) return;

        if (other.CompareTag("Bridge"))
        {
            _groundController.SkipPlatform = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bridge"))
        {
            _groundController.SkipPlatform = true;
        }
    }
}
