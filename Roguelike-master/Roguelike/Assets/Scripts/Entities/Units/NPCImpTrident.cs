using UnityEngine;

public class NPCImpTrident : Navigator {

    private void Start() {
        Name = "Imp-Trident";
        RangeOfAggression = 6;
        Speed = 4;
        Health_Current = 5;
        Attack_Damage = 2;
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
            Entities.Attack( playerCharacterCoordinates, Attack_Damage );
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

    public override void Move() => base.Move();
    protected override void OnTileChanged() => base.OnTileChanged();
    protected override void OnArrival() => base.OnArrival();
}