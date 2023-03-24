using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(movimento))]
public class EditorCharacterExtras : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        movimento mov = (movimento)target;

        GUILayout.Space(30);

        if (GUILayout.Button("Reload scene"))
        {
            SceneManager.LoadScene(0); // SceneManager.GetActiveScene().buildIndex);
        }

        if (GUILayout.Button("Give Dash"))
        {
            mov.dash = true;
        }

        if (GUILayout.Button("Descer"))
        {
            mov.LC();
        }
    }
}
