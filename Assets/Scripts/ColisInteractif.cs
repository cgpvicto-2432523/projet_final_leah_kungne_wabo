using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColisInteractif : MonoBehaviour
{
    [Header("Données du colis (ScriptableObject)")]
    public DonneesColis donnees;

    public DonneesColis Ramasser()
    {
        if (donnees == null)
        {
            Debug.LogWarning($"Aucun DonneesColis assigné sur {gameObject.name} !");
            return null;
        }

        gameObject.SetActive(false);
        return donnees;
    }
}