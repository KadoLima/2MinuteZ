using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static Action<int> OnEnemyDied;
    public static Action<int> OnPlayerDied;

    public static Action OnMissionIsPlayable;
    public static Action OnMissionComplete;
}
