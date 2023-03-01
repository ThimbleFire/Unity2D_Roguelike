using UnityEditor;
using UnityEngine;

namespace AlwaysEast
{
    public class EditorBase : EditorWindow
    {
        protected readonly int S_RESOURCE_DIR_LENGTH = "Assets/Resources/".Length;
        protected readonly int S_OGG_EXTENSION_LENGTH = ".ogg".Length;
        protected readonly int S_PNG_EXTENSION_LENGTH = ".png".Length;
        protected readonly int S_XML_EXTENSION_LENGTH = ".xml".Length;
        protected readonly int S_OVERRIDECONTROLLER_LENGTH = ".overrideController".Length;
        protected const string S_ENTITIES_DIR = "Entities/";
        protected const string S_ITEMS_DIR = "Items/";
        protected const string S_MAPDATA_DIR = "Maps/";

        private const byte Right = 20;
        private Vector2 scrollPos;

        protected enum WindowStates { Main, Create }
        protected WindowStates WindowState { get; set; }
        protected int LoadIndex { get; set; }
        protected bool Loaded { get; set; }
        protected string[] LoadOptions { get; set; }
        protected bool IncludeLoadList { get; set; }
        protected bool IncludeSaveBtn { get; set; }
        protected bool IncludeBackBtn { get; set; }

        protected int Y { get; set; }

        protected virtual void ResetProperties() { }
        protected virtual void CreationWindow() { }
        protected virtual void OnClick_SaveButton() { }
        protected virtual void LoadProperties(TextAsset asset) { }
        protected void AddRow(int mul = 1) { Y += 22 * mul; EditorGUILayout.Space(22 * mul, true); }
        private void ResetRow() { Y = 4; }

        private void OnGUI()
        {
            ResetRow();
            switch (WindowState)
            {
                case WindowStates.Main:

                    if (!Loaded)
                    {
                        Loaded = true;
                        //Awake();
                    }

                    MainWindow();
                    break;
                case WindowStates.Create:
                    EditorGUILayout.BeginVertical();
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true);
                    {
                        if (IncludeSaveBtn)
                            PaintSaveButton();
                        OnClick_SaveButton();

                        if (IncludeBackBtn)
                            PaintBackButton();

                        CreationWindow();
                    }
                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.EndVertical();
                    break;
            }
        }
        //protected virtual void Awake() { }
        protected virtual void MainWindow()
        {

        }
        private void PaintSaveButton()
        {
            GUILayout.BeginArea(new Rect(4, Y, position.width - Right, 21));
            if (GUILayout.Button(string.Format("Save")))
                OnClick_SaveButton();
            GUILayout.EndArea();
            AddRow();
        }
        private void PaintBackButton() { if (PaintButton("Back")) { WindowState = WindowStates.Main; Loaded = false; } }
        protected void PaintLoadList() => LoadIndex = PaintPopup(LoadOptions, LoadIndex);
        protected int PaintPopup(string[] options, int value, string label = "")
        {
            int v = EditorGUI.Popup(new Rect(4, Y, position.width - Right, 20), label, value, options); AddRow();
            return v;
        }
        protected void PaintIntField(ref int value, string label = "")
        {
            value = EditorGUI.IntField(new Rect(4, Y, position.width - Right, 20), label, value); AddRow();
        }
        protected void PaintFloatField(ref float value, string label = "")
        {
            value = EditorGUI.FloatField(new Rect(4, Y, position.width - Right, 20), label, value); AddRow();
        }
        protected void PaintTextField(ref string value, string label = "")
        {
            value = EditorGUI.TextField(new Rect(4, Y, position.width - Right, 20), label, value); AddRow();
        }
        protected void PaintFloatRange(ref float min, ref float max, float minRange, float maxRange, string label = "")
        {
            EditorGUIUtility.wideMode = true;
            EditorGUI.MinMaxSlider(
                new Rect(4, Y, position.width - Right, 20),
                new GUIContent(label),
                ref min,
                ref max,
                minRange,
                maxRange
                );
            EditorGUIUtility.wideMode = false;
            AddRow();
        }
        protected void PaintIntRange(ref int min, ref int max, int minRange, int maxRange, string label = "")
        {
            float _min = min;
            float _max = max;

            EditorGUIUtility.wideMode = true;
            EditorGUI.MinMaxSlider(
                new Rect(4, Y, position.width - 112, 20),
                new GUIContent(string.Format(label, min, max)),
                ref _min,
                ref _max,
                minRange,
                maxRange
                );
            EditorGUI.IntField(new Rect(position.width - 102, Y - 1, 50, 20), min);
            EditorGUI.IntField(new Rect(position.width - 51, Y - 1, 50, 20), max);
            EditorGUIUtility.wideMode = false;

            min = Mathf.FloorToInt(_min);
            max = Mathf.FloorToInt(_max);
            AddRow();
        }
        protected void PaintHorizontalLine()
        {
            Handles.color = Color.gray;
            Handles.DrawLine(new Vector3(4, Y + 11), new Vector3(position.width - Right, Y + 11)); AddRow();
        }
        protected void PaintSpriteField(ref Sprite sprite, string label = "")
        {
            sprite = (Sprite)EditorGUI.ObjectField(new Rect(4, Y, 64, 64), sprite, typeof(Sprite), false);
            if (sprite == null)
            {
                AddRow(4);
                return;
            }
            EditorGUI.LabelField(new Rect(74, Y, position.width - 86, 20), label);
            AddRow();
            EditorGUI.LabelField(new Rect(74, Y, position.width - 86, 20), label);
            AddRow(3);
        }
        protected void PaintIntSlider(ref int value, int min, int max, string label = "")
        {
            value = EditorGUI.IntSlider(new Rect(4, Y, position.width - Right, 20), label, value, min, max);
            AddRow();
        }
        protected bool PaintButton(string message)
        {
            bool result = false;
            GUILayout.BeginArea(new Rect(4, Y, position.width - Right, 20));
            if (GUILayout.Button(message))
                result = true;
            GUILayout.EndArea();
            AddRow();

            return result;
        }
        protected void PaintLabel(string message)
        {
            EditorGUI.LabelField(new Rect(4, Y, position.width - Right, 20), message);
            AddRow();
        }
        protected bool Checkbox(ref bool state, string label)
        {
            state = EditorGUI.Toggle(new Rect(4, Y, position.width - Right, 20), label, state);
            AddRow();
            return state;
        }
        protected TextAsset PaintXMLLookup(TextAsset file, string label, bool invokeOnChange)
        {
            TextAsset v = (TextAsset)EditorGUI.ObjectField(new Rect(4, Y, position.width - Right, 20), label, file, typeof(TextAsset), false); AddRow();

            if (invokeOnChange)
            {
                if (file != v)
                {
                    LoadProperties(v);
                }
            }

            return v;
        }
        protected AnimatorOverrideController PaintAnimationOverrideControllerLookup(AnimatorOverrideController animatorOverrideController)
        {
            AnimatorOverrideController v = (AnimatorOverrideController)EditorGUI.ObjectField(new Rect(4, Y, position.width - Right, 20), animatorOverrideController, typeof(AnimatorOverrideController), false); AddRow();
            return v;
        }

        //You may want to put paint list at the bottom
        protected SerializedObject so;
        protected void PaintList<T>(string label)
        {
            EditorGUILayout.PropertyField(so.FindProperty(label));
            so.ApplyModifiedProperties();
        }
    }
}