using System.Collections.Generic;
using UnityEngine;

public class NPCImp : Entity
{
    private void Start()
    {
        Name = "Imp";
        RangeOfAggression = 6;
        Speed = 3;
    }

    public override void Action( Vector3Int playerCharacterCoordinates )
    {
        // some AI shit

        int distance = Pathfind.GetPath(coordinates, playerCharacterCoordinates).Count;

        if ( distance <= RangeOfAggression )
        {
            chain = Pathfind.GetPath( coordinates, playerCharacterCoordinates );
        }
        else
        {
            chain = Pathfind.Wander( coordinates );
        }

        base.Action(playerCharacterCoordinates);
    }

    protected override void OnStep()
    {
        base.OnStep();
    }

    protected override void OnArrival()
    {
        base.OnArrival();
    }
}
