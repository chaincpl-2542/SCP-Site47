
using UnityEngine;

public class GuideTrigger : MonoBehaviour
{
    public GuideTextController.GuideEnum GuideType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AvtiveGuide();
        }
    }

    public void AvtiveGuide()
    {
        GuideTextController.Instance.ShowGuide(GuideType);
        gameObject.SetActive(false);
    }
}
