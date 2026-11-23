using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ZoneLivraison : MonoBehaviour
{
    private void Awake()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CollecteColis joueur))
        {
            if (joueur.colisActuel != null)
            {
                joueur.LivrerColis();
            }
        }
    }
}