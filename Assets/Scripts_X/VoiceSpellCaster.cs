using UnityEngine;

public class VoiceSpellCaster : MonoBehaviour
{
    public GameObject fireCube;
    public GameObject iceCube;
    public GameObject grabCube;

    // Called from Wit.ai using the "String[] values" pattern
    public void SpellCast(string[] values)
    {
        if (values == null || values.Length == 0) return;

        string keyword = values[0].ToLower();
        Debug.Log("Voice command received: " + keyword);

        switch (keyword)
        {
            case "ice":
                iceCube.SetActive(true);
                break;

            case "fire":
                fireCube.SetActive(true);
                break;

            case "grab":
                grabCube.SetActive(true);
                break;

            default:
                Debug.Log("Unrecognized keyword: " + keyword);
                break;
        }
    }
}
