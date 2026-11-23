using UnityEngine;

public class CollecteColis : MonoBehaviour
{
    [Header("Paramètres Raycast")]
    public float distanceMax = 2.5f;
    public float hauteurOrigine = 1.0f;
    public LayerMask coucheColis = ~0;

    [Header("État du colis transporté")]
    public DonneesColis colisActuel;
    public float tempsEcouleTransport;
    public int chocsSubis;

    [Header("Bonus actif")]
    public bool bouclierActif = false;

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.etatActuel != GameManager.EtatPartie.EnCours)
            return;

        // Si on transporte un colis, on incrémente son timer interne
        if (colisActuel != null)
        {
            tempsEcouleTransport += Time.deltaTime;
            return;
        }

        // Sinon, on cherche un colis devant avec un Raycast
        Vector3 origine = transform.position + Vector3.up * hauteurOrigine;
        Debug.DrawRay(origine, transform.forward * distanceMax, Color.yellow);

        if (Physics.Raycast(origine, transform.forward, out RaycastHit hit, distanceMax, coucheColis, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.TryGetComponent(out ColisInteractif colis))
            {
                PrendreColis(colis.Ramasser());
            }
        }
    }

    public void PrendreColis(DonneesColis nouveauColis)
    {
        colisActuel = nouveauColis;
        tempsEcouleTransport = 0f;
        chocsSubis = 0;
        Debug.Log($"Colis pris : {colisActuel.nomColis} ({colisActuel.type})");
    }

    // Appelé quand le joueur touche un obstacle
    public void SubirChocObstacle()
    {
        if (bouclierActif)
        {
            bouclierActif = false;
            Debug.Log("Bouclier brisé ! Aucun dégât sur le colis.");
            return;
        }

        if (colisActuel == null) return;

        chocsSubis++;
        Debug.Log($"Choc obstacle ! ({chocsSubis}/{colisActuel.maxChocsObstacles})");

        if (colisActuel.maxChocsObstacles >= 0 && chocsSubis > colisActuel.maxChocsObstacles)
        {
            Debug.Log($"Colis détruit ({colisActuel.nomColis}) ! Trop de chocs.");
            colisActuel = null; // Le colis est perdu
        }
    }

    // Appelé quand le joueur entre dans la zone de livraison
    public void LivrerColis()
    {
        if (colisActuel == null) return;

        int pointsGagnes = colisActuel.pointsLivraison;
        float tempsGagne = 0f;

        if (colisActuel.type == TypeColis.Express && tempsEcouleTransport <= colisActuel.limiteTempsExpress)
        {
            tempsGagne = colisActuel.bonusTemps;
            Debug.Log($"Livraison Express réussie à temps ! +{tempsGagne}s");
        }

        GameManager.Instance.AjouterLivraison(pointsGagnes, tempsGagne);
        colisActuel = null;
    }
}