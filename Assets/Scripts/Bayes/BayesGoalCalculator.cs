using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BayesGoalCalculator : MonoBehaviour
{
    // Referencias a UI
    [SerializeField] private Button shootButton; // Botón para disparar
    [SerializeField] private GameObject readyImage; // Imagen inicial del jugador listo para disparar
    [SerializeField] private GameObject goalImage; // Imagen que se muestra si anota gol
    [SerializeField] private GameObject missImage; // Imagen que se muestra si falla
    [SerializeField] private TMP_Text goalProbabilityText; // Texto para la probabilidad de gol
    [SerializeField] private TMP_Text missProbabilityText; // Texto para la probabilidad de fallo

    // Factores iniciales configurables desde el inspector
    [Range(0f, 1f)] public float playerAccuracy = 0.7f; // Probabilidad base de anotar (P(A))
    [Range(0f, 1f)] public float keeperSaveProbability = 0.4f; // Probabilidad de que el portero salve (P(B|A^c))

    private void Start()
    {
        // Configuración inicial: mostrar imagen de "listo" y ocultar las demás
        readyImage.SetActive(true);
        goalImage.SetActive(false);
        missImage.SetActive(false);

        // Vincular botón al evento de disparar
        shootButton.onClick.AddListener(OnShoot);
    }

    private void OnShoot()
    {
        // Calcular las probabilidades
        float goalProbability = CalculateGoalProbability();
        
        // Mostrar las imágenes y textos correspondientes según la probabilidad
        if (goalProbability > 0.5f)
        {
            ShowResult(true, goalProbability);
        }
        else
        {
            ShowResult(false, goalProbability);
        }
    }

    private float CalculateGoalProbability()
    {
        // Probabilidades base
        float probGoal = playerAccuracy; // P(A)
        float probMiss = 1f - probGoal; // Complemento (P(A^c))

        // Probabilidad de disparo dado que no es gol (P(B|A^c))
        float probShotGivenMiss = keeperSaveProbability;

        // Probabilidad total de disparo (P(B))
        float probShot = (probGoal * 1f) + (probMiss * probShotGivenMiss);

        // Teorema de Bayes: probabilidad de gol dado que se dispara (P(A|B))
        float probGoalGivenShot = (probGoal * 1f) / probShot;

        return probGoalGivenShot;
    }

    private void ShowResult(bool isGoal, float goalProbability)
    {
        // Ocultar imagen inicial
        readyImage.SetActive(false);

        // Mostrar imagen correspondiente
        goalImage.SetActive(isGoal);
        missImage.SetActive(!isGoal);

        // Actualizar textos de probabilidades
        float missProbability = 1f - goalProbability;
        goalProbabilityText.text = $"Gol: {goalProbability * 100f:F2}%";
        missProbabilityText.text = $"Fallo: {missProbability * 100f:F2}%";
    }
}
