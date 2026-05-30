using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeManager : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.isEscape)
        {
            SceneManager.LoadScene("FreedomScene");
        }
    }
}
