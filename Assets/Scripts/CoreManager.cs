using UnityEngine;

public class CoreManager : MonoBehaviour
{
    bool isCount = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            if (!isCount)
            {
                GameManager.instance.Core_Destruction_Count();
                isCount = true;
                Destroy(gameObject);
            }
            
        }
    }
}
