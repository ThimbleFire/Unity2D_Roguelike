using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class EditorBase : EditorWindow
{
    private const byte Right = 20;

    protected Sprite[] btnState;
    private Vector2 scrollPos;

    protected enum WindowStates { Main, Create }
    protected WindowStates WindowState { get; set; }
    protected int LoadIndex { get; set; }
    protected bool Loaded { get; set; }
    protected string[] LoadOptions { get; set; }
    protected bool IncludeLoadList { get; set; }
    protected bool IncludeSaveBtn { get; set; }
    protected bool IncludeBackBtn { get; set; }
    
    private void OnGUI()
    {
        ResetRow();

        if ( !Loaded )
            Load();
        
        switch ( WindowState )
        {
            case WindowStates.Main:
                MainWindow();
                break;
            case WindowStates.Create:
                EditorGUILayout.BeginVertical();
                scrollPos = EditorGUILayout.BeginScrollView( scrollPos, false, true );
                {
                    if ( IncludeSaveBtn )
                        if(PaintSaveButton("Save")
                            OnClick_SaveButton();
                    if ( IncludeBackBtn )
                        if(PaintButton("Back") {
                            WindowState = WindowStates.Main;
                            Loaded = false;
                        }
                    CreationWindow();
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
                break;
        }
    }
    protected virtual void MainWindow()
    {
        if(PaintButton("New")) {
            ResetProperties();
            WindowState = WindowStates.Create;
        }
        if ( IncludeLoadList )
            if(PaintButton("Load Selected")) {
                LoadProperties();
                WindowState = WindowStates.Create;
            }
            PaintLoadList();
    }

    /// <summary>Called when clicking New button</summary>
    protected virtual void ResetProperties() { }
    protected virtual void CreationWindow() { }
    protected virtual void OnClick_SaveButton() { }
    protected virtual void LoadProperties() { }
    private void AddRow(int mul = 1) { Y += 22 * mul; EditorGUILayout.Space( 22 * mul, true ); }
    private void ResetRow() { Y = 4; }

    private void PaintSaveButton() {
        GUILayout.BeginArea( new Rect( 4, Y, position.width - Right, 21 ) );
        if ( GUILayout.Button( string.Format( "Save" ) ) )
            OnClick_SaveButton();
        GUILayout.EndArea();
        AddRow();
    }
    private void PaintBackButton() {
        GUILayout.BeginArea( new Rect( 4, Y, position.width - Right, 21 ) );
        if ( GUILayout.Button( "Back" ) ) {
            WindowState = WindowStates.Main;
            Loaded = false;
        }
        GUILayout.EndArea();
        AddRow();
    }
    protected void PaintLoadList() => LoadIndex = PaintPopup( LoadOptions, LoadIndex );
    
    protected void PaintIntField( ref int value, string label = "" ) {
        value = EditorGUI.IntField( new Rect( 4, Y, position.width - Right, 20 ), label, value ); AddRow();
    }
    protected void PaintFloatField( ref float value, string label = "" ) {
        value = EditorGUI.FloatField( new Rect( 4, Y, position.width - Right, 20 ), label, value ); AddRow();
    }
    protected void PaintTextField( ref string value, string label = "" ) {
        value = EditorGUI.TextField( new Rect( 4, Y, position.width - Right, 20 ), label, value ); AddRow();
    }
    protected void PaintFloatRange( ref float min, ref float max, float minRange, float maxRange, string label = "" ) {
        EditorGUIUtility.wideMode = true;
        {
            EditorGUI.MinMaxSlider(
                new Rect( 4, Y, position.width - Right, 20 ),
                new GUIContent( label ),
                ref min,
                ref max,
                minRange,
                maxRange
                );
        }
        EditorGUIUtility.wideMode = false;
        AddRow();
    }
    protected void PaintHorizontalLine() {
        Handles.color = Color.gray;
        Handles.DrawLine( new Vector3( 4, Y + 11 ), new Vector3( position.width - Right, Y + 11 ) ); AddRow();
    }
    protected int PaintPopup( string[] options, int value, string label = "" ) {
        int v = EditorGUI.Popup( new Rect( 4, Y, position.width - Right, 20 ), value, options ); AddRow();
        return v;
    }
    protected void PaintSpriteField( ref Sprite sprite, ref string fileName, string label = "" ) {
        sprite = (Sprite)EditorGUI.ObjectField( new Rect( 4, Y, 64, 64 ), sprite, typeof( Sprite ), false );

        if ( sprite != null )
        {
            fileName = sprite.name;
            EditorGUI.LabelField( new Rect( 74, Y, position.width - 86, 20 ), label );
            AddRow();
            EditorGUI.LabelField( new Rect( 74, Y, position.width - 86, 20 ), label );
            AddRow(3);
        }
        else
        {
            AddRow(4);
        }
    }
    protected void PaintIntSlider( ref int value, int min, int max, string label = "" ) {
        value = EditorGUI.IntSlider( new Rect( 4, Y, position.width - Right, 20 ), label, value, min, max );
        AddRow();
    }
    protected bool PaintButton( string message ) {
        bool result = false;
        GUILayout.BeginArea( new Rect( 4, Y, position.width - Right, 20 ) );
        if ( GUILayout.Button( message ) )
            result = true;
        GUILayout.EndArea();
        AddRow();

        return result;
    }
    protected void PaintLabel( string message ) {
        EditorGUI.LabelField( new Rect( 4, Y, position.width - Right, 20 ), message );
        AddRow();
    }
}
