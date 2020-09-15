using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Ranking
{
    public class CustomEditorWindow : EditorWindow
    {
        string ascendingStr = "";

        bool activeMaximumScoreNumbers = true;
        int maximumScoreNumbers;
        int selectedMaximumScoreNumbers = 10;
        public string[] maxOptionsStr = new string[] { "10", "25", "50", "100", "1000" };
        public int[] maxOptionsInt = new int[] { 10, 25, 50, 100, 1000 };

        int precision;
        int selectedPrecision = 2;
        public string[] digitOptionsStr = new string[] { "0", "0.1", "0.01", "0.001", "0.0001" };
        public int[] digitOptionsInt = new int[] { 0, 1, 2, 3, 4 };

        [MenuItem("Window/OptionalFunctions")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<CustomEditorWindow>("OptionalFunctions");
        }

        private void OnGUI()
        {
            if (!EditorApplication.isPlaying && SceneManager.GetActiveScene().name != "Page2")
                ascendingStr = "";

            GUILayout.Label("Classements par ordre: " + ascendingStr, EditorStyles.boldLabel);

            if (GUILayout.Button("Croissant"))
            {
                if (EditorApplication.isPlaying && SceneManager.GetActiveScene().name == "Page2")
                    RankingTable.GetInstance().SetAscendingAndUpdateRanks(true);
                ascendingStr = "Croissant";
            }

            if (GUILayout.Button("Décroissant"))
            {
                if (EditorApplication.isPlaying && SceneManager.GetActiveScene().name == "Page2")
                    RankingTable.GetInstance().SetAscendingAndUpdateRanks(false);
                ascendingStr = "Décroissant";
            }

            EditorGUILayout.Space();

            activeMaximumScoreNumbers = EditorGUILayout.Toggle("Afficher tous les scores", activeMaximumScoreNumbers);
            if (activeMaximumScoreNumbers)
            {
                if (EditorApplication.isPlaying && SceneManager.GetActiveScene().name == "Page2")
                    RankingTable.GetInstance().SetShowingAllRanks(activeMaximumScoreNumbers);
            }
            else
            {
                GUILayout.Label("Nombre de scores à afficher", EditorStyles.boldLabel);
                maximumScoreNumbers = EditorGUILayout.IntPopup("maximum", selectedMaximumScoreNumbers, maxOptionsStr, maxOptionsInt);
                selectedMaximumScoreNumbers = maximumScoreNumbers;

                if (EditorApplication.isPlaying && SceneManager.GetActiveScene().name == "Page2")
                {
                    RankingTable.GetInstance().SetShowingAllRanks(!activeMaximumScoreNumbers);
                    RankingTable.GetInstance().SetMaximumScoreNumbers(maximumScoreNumbers);
                }
            }

            EditorGUILayout.Space();

            GUILayout.Label("Nombre de digits à afficher", EditorStyles.boldLabel);
            precision = EditorGUILayout.IntPopup("Précision", selectedPrecision, digitOptionsStr, digitOptionsInt);
            selectedPrecision = precision;

            if (EditorApplication.isPlaying)
            {
                if (SceneManager.GetActiveScene().name == "Page2")
                {
                    RankingTable.GetInstance().SetPrecisionAndUpdateTables(precision);
                    //Debug.Log(SceneManager.GetActiveScene().name);
                }

                UnityWebRequestScript.Instance.SetPrecision(precision);
                //Debug.Log(SceneManager.GetActiveScene().name);
            }
        }
    }
}
