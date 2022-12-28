using UnityEngine;

public class AttackSplash : MonoBehaviour {

    public enum Type {
        Slash, Slash_Critical, Pierce, Pierce_Critical
    }

    private static Transform Transform;
    private static Animator Animator;

    private static Vector3 offset = new Vector3( 0.5f, 0.75f );

    private void Awake() {
        Transform = GetComponent<Transform>();
        Animator = GetComponent<Animator>();
    }

    public static void Show( Vector3Int position, Type slashType ) {
        Transform.position = position + offset;

        switch ( slashType ) {
            case Type.Slash:
                Animator.Play( "Slash" );
                break;

            case Type.Slash_Critical:
                Animator.Play( "Slash_critical" );
                break;

            case Type.Pierce:
                Animator.Play( "Piere" );
                break;

            case Type.Pierce_Critical:
                Animator.Play( "Piere_critical" );
                break;
        }
    }

    public void End() => Transform.position = Vector3.zero;
}