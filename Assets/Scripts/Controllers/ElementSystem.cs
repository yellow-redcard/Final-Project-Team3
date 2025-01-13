using System;
using System.Collections.Generic;
using UnityEngine;

public class ElementSystem : MonoBehaviour
{
    public Action<ElementType, ElementType> onWin;
    public Action<ElementType, ElementType> onLose;
    public Action<ElementType, ElementType> onDraw;

    private Dictionary<ElementType, ElementType> winAgainst = new Dictionary<ElementType, ElementType>()
    {
        { ElementType.Water, ElementType.Flame },
        { ElementType.Flame, ElementType.Dark },
        { ElementType.Dark, ElementType.Electricity },
        { ElementType.Electricity, ElementType.Water }
    };

    public void DetermineOutcome(ElementType element1, ElementType element2, ref float damage)
    {
        if (winAgainst[element1] == element2)
        {
            damage *= 1.5f; // 이기는 상성 데미지 1.5배
            onWin?.Invoke(element1, element2);
        }
        else if (winAgainst[element2] == element1)
        {
            damage *= 0.5f; // 지는 상성 데미지 0.5배
            onLose?.Invoke(element1, element2);
        }
        else
        {
            damage *= 1f;
            onDraw?.Invoke(element1, element2);
        }
    }
}