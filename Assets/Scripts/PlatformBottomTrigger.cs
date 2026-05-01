using UnityEngine;

public class PlatformBottomTrigger : MonoBehaviour
{
    private BoxCollider2D triggerCol;
    public BoxCollider2D prefabCol;

    void Start()
    {
        prefabCol = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            prefabCol.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            prefabCol.enabled = true;
        }
    }
}