using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (PlayerController.SharedInstance.ReadyToBuildBridge || !GameManager.SharedInstance.IsGameStarted) return;

        transform.Translate(Vector3.left * GameManager.SharedInstance.BuildSpeed * Time.deltaTime);
       
        if (transform.position.x < GameManager.SharedInstance.LeftBoundary && CompareTag("Respawn"))
        {
            Destroy(gameObject);
        }
    }
}
