using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GuessWhoGame : MonoBehaviour
{
    public List<Character> characters; // Lista de personajes
    public TextMeshProUGUI questionHeaderText; // Texto para mostrar "Questions" o "Select a Character"
    public TextMeshProUGUI questionText; // Campo en la UI para mostrar la pregunta

    private List<Character> remainingCharacters; // Personajes restantes
    private Character playerSelectedCharacter;  // Personaje elegido por el jugador
    //private int questionIndex = 0;        // Índice de la pregunta actual
    private bool isPlayerCharacterSelected = false; // Verifica si el jugador ya seleccionó un personaje
    private List<string> availableQuestions; // Lista de preguntas que aún no se han 
    private string currentQuestion; // Variable para almacenar la pregunta actual
    
    // Preguntas posibles (características)
    private string[] questions = new string[] 
    { 
        "Is it a human?", 
        "Is it a robot?", 
        "Is it an animal?", 
        "Is it a villain?", 
        "Does it wear a cape?", 
        "Is it a superhero?", 
        "Is it from a cartoon?", 
        "Is it a classic character?", 
        "Is it a toy?", 
        "Is it from a movie?",
        "Is it from a videogame?",
        "Is it from a kids show?",
        "Is it from a book?",
        "Has green skin?",
        "Has yellow skin?",
        "Can you eat it?",
        "Can you capture monsters with it?"
    };

    void Start()
    {
        // Crear los personajes
        characters = new List<Character>()
        {
            new Character("Batman", new Dictionary<string, bool> { {"Is it a human?", true}, {"Is it a robot?", false}, {"Is it an animal?", false}, {"Is it a villain?", false}, {"Does it wear a cape?", true}, {"Is it a superhero?", true} }),
            new Character("Angry Bird", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Is it an animal?", true}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Captain America", new Dictionary<string, bool> { {"Is it a human?", true}, {"Is it a robot?", false}, {"Is it an animal?", false}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Charles Chaplin", new Dictionary<string, bool> { {"Is it a human?", true}, {"Is it a robot?", false}, {"Is it an animal?", false}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", true} }),
            new Character("Darth Vader", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Is it an animal?", false}, {"Is it a villain?", true}, {"Does it wear a cape?", true}, {"Is it a superhero?", false} }),
            new Character("Eve", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", true}, {"Is it an animal?", false}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Furby", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Is it an animal?", true}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Minion", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Is it an animal?", false}, {"Is it a villain?", true}, {"Does it wear a cape?", false}, {"Is it a superhero?", false}, {"Is it from a movie", true}, {"Has it yellow skin", true} }),
            new Character("Mushroom", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Is it an animal?", false}, {"Is it from a videogame?", true}, {"Can you eat it?", true}, {"Is it a superhero?", false} }),
            new Character("Pacman", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Has yellow skin?", false}, {"Is it from a videogame", true}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Pokeball", new Dictionary<string, bool> { {"Can you capture monsters with it?", true}, {"Is it a robot?", false}, {"Is it an animal?", false}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("R2D2", new Dictionary<string, bool> { {"Is it from a movie?", true}, {"Is it a robot?", true}, {"Is it an animal?", false}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Scream", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", true}, {"Is it an animal?", true}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Sponge Bob", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Is it an animal?", true}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Tamagotchi", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Is it an animal?", true}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Tele Tubby", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Is it an animal?", true}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Wall-E", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Is it an animal?", true}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Super Mario", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Is it an animal?", true}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Ninja Turtle", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Is it an animal?", true}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Yoda", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Is it an animal?", true}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Pinocchio", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Is it an animal?", true}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Little Red Riding Hood", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Is it an animal?", true}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Big Bad Wolf", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Is it an animal?", true}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
            new Character("Little Mermaid", new Dictionary<string, bool> { {"Is it a human?", false}, {"Is it a robot?", false}, {"Is it an animal?", true}, {"Is it a villain?", false}, {"Does it wear a cape?", false}, {"Is it a superhero?", false} }),
        };
        
        // Inicializar la lista de preguntas disponibles con una copia de las preguntas originales
        availableQuestions = new List<string>(questions);
        
        // Inicializar la lista de personajes restantes
        remainingCharacters = new List<Character>(characters);

        // Configurar el texto inicial como "Select a Character"
        questionHeaderText.text = "Select a Character";
    }

    // Método para que el jugador elija un personaje
    public void SelectCharacter(string characterName)
    {
        playerSelectedCharacter = characters.Find(c => c.name == characterName);

        if (playerSelectedCharacter != null)
        {
            isPlayerCharacterSelected = true;
            Debug.Log($"You have selected: {playerSelectedCharacter.name}");

            // Cambiar el encabezado a "Questions" cuando el personaje sea seleccionado
            questionHeaderText.text = "Questions";
            
            NextQuestion(); // Comenzar con la primera pregunta de la IA
        }
        else
        {
            Debug.LogError("The selected character was not found.");
        }
    }
public void NextQuestion()
{
    // Verificamos si aún hay más de un personaje y preguntas disponibles
    if (remainingCharacters.Count > 1 && availableQuestions.Count > 0)
    {
        // Seleccionar un índice aleatorio de las preguntas restantes
        int randomIndex = Random.Range(0, availableQuestions.Count);
        currentQuestion = availableQuestions[randomIndex]; // Guardar la pregunta actual

        Debug.Log($"Question: {currentQuestion} (Yes/No)");
        questionText.text = currentQuestion; // Mostrar la pregunta en la UI

        // Eliminar la pregunta seleccionada de la lista de preguntas disponibles
        availableQuestions.RemoveAt(randomIndex);
    }
    else if (remainingCharacters.Count == 1)
    {
        // Si solo queda un personaje, la IA intenta adivinar
        Debug.Log($"The AI guessed that your character is {remainingCharacters[0].name}!");
        questionText.text = $"Is it {remainingCharacters[0].name}?";
    }
    else if (availableQuestions.Count == 0)
    {
        // Si ya no hay preguntas pero hay más de un personaje
        Debug.Log("The AI couldn't guess your character.");
        questionText.text = "The AI couldn't guess your character.";
    }
}

// Método para recibir respuesta del jugador (Sí)
public void OnYesButtonPressed()
{
    if (!string.IsNullOrEmpty(currentQuestion))
    {
        // Después de la respuesta, continuamos con la siguiente pregunta
        NextQuestion();
    }
}

// Método para recibir respuesta del jugador (No)
public void OnNoButtonPressed()
{
    if (!string.IsNullOrEmpty(currentQuestion))
    {
        // Después de la respuesta, continuamos con la siguiente pregunta
        NextQuestion();
    }
}
}
