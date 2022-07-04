using System;
using System.Collections;
using System.Collections.Generic;
using FormationSystem;
using Generic;
using UnityEngine;

public class BattleFormation
{
    private readonly string[,] _formation;
    public string[,] Formation => _formation;

    public BattleFormation()
    {
        _formation = new string[Enum<FormationColumn>.Length, Enum<FormationRow>.Length];

        Itinerate((_, position) =>
        {
            var (x, y) = position;
            _formation[x, y] = $"{x}, {y}";
        });
    }

    public bool Set(string value, FormationPosition position)
    {
        var coordinates = position.ToVector2();
        _formation[coordinates.x, coordinates.y] = value;

        return true;
    }

    /// <summary> Retrieve unit from formation position </summary>
    public string Get(int x, int y)
    {
        return _formation[x, y];
    }

    /// <summary> Retrieve unit from formation position </summary>
    public string Get(FormationRow row, FormationColumn column) => Get(row.ToInt(), column.ToInt());

    /// <summary> Retrieve unit from formation position </summary>
    public string Get(FormationPosition position)
    {
        var (x, y) = position.ToVector2();
        return Get(x, y);
    }

    public void Itinerate(Action<string, Vector2Int> callback)
    {
        for (var y = 0; y < Enum<FormationRow>.Length; y++)
        for (var x = 0; x < Enum<FormationColumn>.Length; x++)
        {
            callback(_formation[x, y], new Vector2Int(x, y));
        }
    }
}