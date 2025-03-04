using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public GameObject winPopup;
    public GameObject cameraFollow;

    private void Start()
    {
        winPopup.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            winPopup.SetActive(true);
            cameraFollow.GetComponent<CameraFollow>().enabled = false;
            cameraFollow.GetComponent<GameUI>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
    }
}
