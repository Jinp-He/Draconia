using UnityEngine;
using TMPro;

public class ChangeAllFont : MonoBehaviour
{
    public TMP_FontAsset newFont;

    public void ChangeAllFonts()
    {
        if (newFont == null)
        {
            Debug.LogError("Please assign a new TMP font in the inspector.");
            return;
        }

        TextMeshProUGUI[] allTextObjects = FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in allTextObjects)
        {
            text.font = newFont;
        }
    }
}