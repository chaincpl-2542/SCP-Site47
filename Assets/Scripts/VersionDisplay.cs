using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VersionDisplay : MonoBehaviour
{
    public TextMeshProUGUI versionText; // Assign this in the Inspector

    void Start()
    {
        if (versionText != null)
        {
            versionText.text = $"Version: {Application.version}";
        }
    }
}
