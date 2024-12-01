using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private int score = 10;
    [SerializeField] private IntEventChannel scoreChannel;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }

        scoreChannel.Invoke(score);

        Destroy(gameObject);
    }
}
