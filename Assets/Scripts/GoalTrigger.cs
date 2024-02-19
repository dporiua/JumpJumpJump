using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public GameObject winPopup; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            winPopup.SetActive(true); 
            
        }
    }
}
