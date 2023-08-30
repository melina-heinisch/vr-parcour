using UnityEditor;
using UnityEngine;

namespace _3DUI.editor
{
    [CustomEditor(typeof(HelpMenuController))]
    public class HelpMenuControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            HelpMenuController controller = (HelpMenuController)target;
            if (GUILayout.Button("Open/Close Help Menu"))
            {
                controller.EditorOpenOrCloseHelpMenu();
            }
        }
    }
}