using UnityEngine;

public class CollecteColis : MonoBehaviour
{
    [Header("Paramètres Raycast")]
    public float distanceMax = 2.5f;
    public float hauteurOrigine = 1.0f;
    public LayerMask coucheColis = ~0;

    [Header("Affichage sur la tête")]
    public Transform pointTete;
    private GameObject colisVisuelInstancie;

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

        if (colisActuel != null)
        {
            tempsEcouleTransport += Time.deltaTime;
            return;
        }

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
        if (nouveauColis == null) return;

        colisActuel = nouveauColis;
        tempsEcouleTransport = 0f;
        chocsSubis = 0;

        AfficherColisSurTete(nouveauColis);
        Debug.Log($"Colis pris : {colisActuel.nomColis} ({colisActuel.type})");
    }

    private void AfficherColisSurTete(DonneesColis donnees)
    {
        SupprimerColisSurTete();

        if (pointTete == null)
        {
            Debug.LogWarning("PointTete n'est pas assigné sur CollecteColis !");
            return;
        }

        colisVisuelInstancie = GameObject.CreatePrimitive(PrimitiveType.Cube);
        colisVisuelInstancie.transform.SetParent(pointTete, false);
        colisVisuelInstancie.transform.localScale = Vector3.one * 0.45f;

        Collider col = colisVisuelInstancie.GetComponent<Collider>();
        if (col != null) Destroy(col);
    }


    private void SupprimerColisSurTete()
    {
        if (colisVisuelInstancie != null)
        {
            Destroy(colisVisuelInstancie);
            colisVisuelInstancie = null;
        }
    }

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
            colisActuel = null;
            SupprimerColisSurTete();
        }
    }

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
        SupprimerColisSurTete();
    }
}