using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [field: SerializeField] public PlayerMovementState CurrentPlayerMovementState { get; private set; } = PlayerMovementState.Idling;
    public enum PlayerMovementState
    {
        Idling = 0,
        Running = 1,
        Jumping = 2,
        Falling = 3,
    }
}
