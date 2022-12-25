using UnityEngine;

public class NPCImp : Entity
{
    private void Start()
    {
        Name = "Imp";
        RangeOfAggression = 6;
        Speed = 3;
    }

    public override void Action()
    {
        _operations.Add(Move);
        _operations.Add(Attack);
        _operations.Add(Move);
        
        _operations[0]();
    
        base.Action();
    }
    
    protected override void Move()
    {
        Vector3Int playerCharacterCoordinates = Entities.GetPCCoordinates;
    
        // some AI shit

        int disX = Mathf.Abs( playerCharacterCoordinates.x - _coordinates.x );
        int disY = Mathf.Abs( playerCharacterCoordinates.y - _coordinates.y );
        
        int distance = disX + disY;
        
        if ( distance == 1 )
        {
            Debug.Log( Name + " attack!" );
            Entities.Step();
            return;
        }
        
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

    protected override void OnStep()
    {
        base.OnStep();
    }

    protected override void OnArrival()
    {
        base.OnArrival();
    }
}
