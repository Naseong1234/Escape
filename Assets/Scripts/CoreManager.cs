using System.Runtime.CompilerServices;
using UnityEngine;

public class CoreManager : MonoBehaviour
{
    GameManager gameManager;
    bool isCount = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            if (!isCount)
            {
                gameManager.Core_Destruction_Count();
                isCount = true;
                Destroy(gameObject);
            }
            
        }
    }
}
