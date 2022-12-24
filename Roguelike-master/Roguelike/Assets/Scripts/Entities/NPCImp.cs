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

        int disX = Mathf.Abs( playerCharacterCoordinates.x - Coordinates.x );
        int disY = Mathf.Abs( playerCharacterCoordinates.y - Coordinates.y );
        if ( disX + disY == 1 )
        {
            Debug.Log( Name + " attack!" );
            Entities.Step();
        }
        else
        {

            int distance = Pathfind.GetPath(Coordinates, playerCharacterCoordinates, true).Count;

            if ( distance <= RangeOfAggression )
            {
                chain = Pathfind.GetPath( Coordinates, playerCharacterCoordinates, false );

                if ( chain.Count == 0 )
                    Entities.Step();
            }
            else
            {
                chain = Pathfind.Wander( Coordinates );
            }
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
