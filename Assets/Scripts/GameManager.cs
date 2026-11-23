using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum EtatPartie
    {
        EnCours,
        Victoire,
        Defaite
    }

    [Header("Paramètres de la partie")]
    [Range(90f, 120f)]
    public float dureeTotale = 90f;
    public int objectifLivraisons = 3;

    [Header("État actuel")]
    public EtatPartie etatActuel = EtatPartie.EnCours;
    public float tempsRestant;
    public int livraisonsEffectuees = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        DemarrerPartie();
    }

    public void DemarrerPartie()
    {
        tempsRestant = dureeTotale;
        livraisonsEffectuees = 0;
        etatActuel = EtatPartie.EnCours;
        Debug.Log($"Partie démarrée ! Temps : {tempsRestant}s | Objectif : {objectifLivraisons} livraisons");
    }

    private void Update()
    {
        if (etatActuel != EtatPartie.EnCours)
            return;

        tempsRestant -= Time.deltaTime;

        if (tempsRestant <= 0f)
        {
            tempsRestant = 0f;
            etatActuel = EtatPartie.Defaite;
            Debug.Log("Temps écoulé ! Défaite.");
        }
    }

    public void AjouterLivraison(int points = 1, float bonusTemps = 0f)
    {
        if (etatActuel != EtatPartie.EnCours)
            return;

        livraisonsEffectuees += points;
        tempsRestant += bonusTemps;

        Debug.Log($"Livraison validée ! Score : {livraisonsEffectuees}/{objectifLivraisons} | Temps : {tempsRestant:F1}s");

        if (livraisonsEffectuees >= objectifLivraisons)
        {
            etatActuel = EtatPartie.Victoire;
            Debug.Log("Objectif atteint ! Victoire !");
        }
    }
}
