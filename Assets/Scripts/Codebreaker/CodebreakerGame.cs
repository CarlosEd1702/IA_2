using System.Collections.Generic;
using UnityEngine;

public class PatternGuessingAI : MonoBehaviour
{
    private List<Color[]> possiblePatterns;
    private Color[] currentGuess;
    private Color[] playerPattern;

    private List<Color> availableColors;
    private int maxAttempts = 10;
    private int currentAttempt;

    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        // Define los colores disponibles
        availableColors = new List<Color> {
            Color.red, Color.blue, Color.green, Color.yellow,
            Color.black, Color.white, new Color(1f, 0.5f, 0) // Naranja
        };

        // Genera todas las combinaciones posibles (permite repetición de colores)
        possiblePatterns = GenerateAllPossiblePatterns(availableColors, 4);

        // Define el patrón secreto establecido por el jugador (puedes configurarlo manualmente para pruebas)
        playerPattern = new Color[] { Color.red, Color.green, Color.blue, Color.yellow };

        currentAttempt = 0;

        StartGuessing();
    }

    void StartGuessing()
    {
        while (currentAttempt < maxAttempts && possiblePatterns.Count > 0)
        {
            // Toma el primer patrón de la lista como conjetura
            currentGuess = possiblePatterns[0];
            Debug.Log($"Intento {currentAttempt + 1}: {FormatPattern(currentGuess)}");

            // Calcula retroalimentación
            (int correctPosition, int correctColor) = GetFeedback(currentGuess, playerPattern);
            Debug.Log($"Feedback: {correctPosition} posición correcta(s), {correctColor} color(es) correcto(s) en posición incorrecta");

            // Si la conjetura es correcta, termina
            if (correctPosition == 4)
            {
                Debug.Log($"¡La IA ha adivinado el patrón en {currentAttempt + 1} intentos!");
                break;
            }

            // Filtra los patrones posibles basándose en la retroalimentación
            FilterPatterns(currentGuess, correctPosition, correctColor);

            currentAttempt++;
        }

        if (currentAttempt >= maxAttempts)
        {
            Debug.Log("La IA no pudo adivinar el patrón en el límite de intentos.");
        }
    }

    List<Color[]> GenerateAllPossiblePatterns(List<Color> colors, int patternLength)
    {
        List<Color[]> patterns = new List<Color[]>();
        GenerateCombinations(new Color[patternLength], 0, colors, patterns);
        return patterns;
    }

    void GenerateCombinations(Color[] currentPattern, int position, List<Color> colors, List<Color[]> patterns)
    {
        if (position == currentPattern.Length)
        {
            patterns.Add((Color[])currentPattern.Clone());
            return;
        }

        foreach (Color color in colors)
        {
            currentPattern[position] = color;
            GenerateCombinations(currentPattern, position + 1, colors, patterns);
        }
    }

    (int, int) GetFeedback(Color[] guess, Color[] target)
    {
        int correctPosition = 0;
        int correctColor = 0;

        bool[] usedInTarget = new bool[target.Length];
        bool[] usedInGuess = new bool[guess.Length];

        // Comprueba posiciones correctas
        for (int i = 0; i < guess.Length; i++)
        {
            if (guess[i] == target[i])
            {
                correctPosition++;
                usedInTarget[i] = true;
                usedInGuess[i] = true;
            }
        }

        // Comprueba colores correctos en posiciones incorrectas
        for (int i = 0; i < guess.Length; i++)
        {
            if (usedInGuess[i]) continue;

            for (int j = 0; j < target.Length; j++)
            {
                if (!usedInTarget[j] && guess[i] == target[j])
                {
                    correctColor++;
                    usedInTarget[j] = true;
                    break;
                }
            }
        }

        return (correctPosition, correctColor);
    }

    void FilterPatterns(Color[] guess, int correctPosition, int correctColor)
    {
        possiblePatterns.RemoveAll(pattern =>
        {
            (int pos, int col) = GetFeedback(pattern, guess);
            return pos != correctPosition || col != correctColor;
        });
    }

    string FormatPattern(Color[] pattern)
    {
        string formatted = "";
        foreach (Color color in pattern)
        {
            if (color == Color.red) formatted += "Rojo ";
            else if (color == Color.blue) formatted += "Azul ";
            else if (color == Color.green) formatted += "Verde ";
            else if (color == Color.yellow) formatted += "Amarillo ";
            else if (color == Color.black) formatted += "Negro ";
            else if (color == Color.white) formatted += "Blanco ";
            else formatted += "Naranja ";
        }
        return formatted.Trim();
    }
}
