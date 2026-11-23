using UnityEngine;

public enum TypeColis
{
    Standard,
    Express,
    Special
}

[CreateAssetMenu(fileName = "NouveauColis", menuName = "Livraison/Donnees Colis")]
public class DonneesColis : ScriptableObject
{
    public string nomColis = "Colis Standard";
    public TypeColis type = TypeColis.Standard;

    [Header("Récompenses")]
    public int pointsLivraison = 1;
    public float bonusTemps = 0f;

    [Header("Contraintes")]
    public float limiteTempsExpress = 20f;
    public int maxChocsObstacles = -1;
}