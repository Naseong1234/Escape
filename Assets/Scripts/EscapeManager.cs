using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeManager : MonoBehaviour
{
    GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (gameManager.isEscape)
        {
            SceneManager.LoadScene("FreedomScene");
        }
    }
}
