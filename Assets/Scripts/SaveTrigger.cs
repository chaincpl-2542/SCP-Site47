using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SaveGame();
            gameObject.SetActive(false);
        }
    }
}
