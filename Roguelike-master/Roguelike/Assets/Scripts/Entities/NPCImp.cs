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

        int disX = Mathf.Abs( playerCharacterCoordinates.x - _coordinates.x );
        int disY = Mathf.Abs( playerCharacterCoordinates.y - _coordinates.y );
        
        int distance = disX + disY;
        
        if ( distance == 1 )
        {
            Debug.Log( Name + " attack!" );
            Entities.Step();
        }
        else
        {
            //int distance = Pathfind.GetPath(_coordinates, playerCharacterCoordinates, true).Count;

            if ( distance <= RangeOfAggression )
            {
                chain = Pathfind.GetPath( _coordinates, playerCharacterCoordinates, false );

                if ( chain.Count == 0 )
                    Entities.Step();
            }
            else
            {
                chain = Pathfind.Wander( _coordinates );
            }
        }

        base.Action( playerCharacterCoordinates );
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
