using UnityEngine;
using UnityEditor;

public class CustomEditorWindow : EditorWindow
{

    bool activeMaximumScoreNumbers = false;
    int maximumScoreNumbers;
    public string[] optionsStr = new string[] { "10", "100", "1000" };
    public int[] optionsInt = new int[] { 10, 100, 1000 };

    float digitNumbers;

    [MenuItem("Window/OptionalFunctions")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<CustomEditorWindow>("OptionalFunctions");
    }

    private void OnGUI()
    {
        GUILayout.Label("Classements par ordre", EditorStyles.boldLabel);
        if (GUILayout.Button("Croissant"))
        {
            AscendingOrder();
        }

        if (GUILayout.Button("Décroissant"))
        {
            DescendingOrder();
        }

        EditorGUILayout.Space();

        activeMaximumScoreNumbers = EditorGUILayout.Toggle("Afficher tous les scores", activeMaximumScoreNumbers);
        if (!activeMaximumScoreNumbers)
        {
            GUILayout.Label("Nombre maximum de scores à afficher", EditorStyles.boldLabel);
            maximumScoreNumbers = EditorGUILayout.IntPopup(maximumScoreNumbers, optionsStr, optionsInt);
        }

        EditorGUILayout.Space();

        GUILayout.Label("Nombre de digits à afficher", EditorStyles.boldLabel);
        GUILayout.Label("après la virgule dans le cas de scores en nombres décimaux", EditorStyles.boldLabel);

        digitNumbers = EditorGUILayout.FloatField(digitNumbers);
    }

    void AscendingOrder()
    {

    }

    void DescendingOrder()
    {

    }
}
