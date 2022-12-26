using UnityEngine;

public class NPCImp : Entity
{
    private void Start()
    {
        Name = "Imp";
        RangeOfAggression = 6;
        Speed = 3;
    }

    public override void Action() {

        Vector3Int playerCharacterCoordinates = Entities.GetPCCoordinates;

        // some AI shit

        int disX = Mathf.Abs( playerCharacterCoordinates.x - _coordinates.x );
        int disY = Mathf.Abs( playerCharacterCoordinates.y - _coordinates.y );

        int distance = disX + disY;

        if ( distance == 1 ) {
            Attack();
            AttackSplash.Show( playerCharacterCoordinates, AttackSplash.Type.Pierce );
            return;
        }

        if ( distance <= RangeOfAggression ) {
            _chain = Pathfind.GetPath( _coordinates, playerCharacterCoordinates, false );

            if ( _chain.Count == 0 )
                Entities.Step();
        }
        else {
            _chain = Pathfind.Wander( _coordinates );
        }

        base.Action();
    }

    public override void Move()
    {
        base.Move();
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
