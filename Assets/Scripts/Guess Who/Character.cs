using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public string name;
    public Dictionary<string, bool> characteristics;

    public Character(string name, Dictionary<string, bool> characteristics)
    {
        this.name = name;
        this.characteristics = characteristics;
    }

    // Método para verificar si el personaje coincide con la pregunta y la respuesta
    public bool MatchesQuestion(string question, bool answer)
    {
        // Verifica si el personaje tiene esa característica y si coincide con la respuesta dada
        if (characteristics.ContainsKey(question))
        {
            return characteristics[question] == answer;
        }
        else
        {
            // Si el personaje no tiene esa característica, lo tratamos como si no coincidiera
            return false;
        }
    }
}
