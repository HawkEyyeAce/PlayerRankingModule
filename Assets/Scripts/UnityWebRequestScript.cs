using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

namespace Ranking
{
    public class UnityWebRequestScript : MonoBehaviour
    {
        private static UnityWebRequestScript m_instancePrecision;

        public Text message;
        public InputField userID;
        public InputField score;
        public InputField userIDToGetScore;
        public InputField userIDToDelete;
        public GameObject adminRightsTemplate;

        readonly string getURL = "http://localhost:8080/rankings/";
        readonly string getUserIDURL = "http://localhost:8080/rankings/userID/";
        readonly string postURL = "http://localhost:8080/rankings/";
        readonly string deleteURL = "http://localhost:8080/rankings/userID/";

        private Regex rgx = new Regex("[a-zA-Z0-9]*");

        int precision;
        bool adminRights;

        private void Start()
        {
            if (SceneManager.GetActiveScene().name == "Page1")
                message.text = "Press buttons to interacts with web server";
        }

        public static UnityWebRequestScript Instance
        {
            get
            {
                if (m_instancePrecision == null)
                {
                    m_instancePrecision = (UnityWebRequestScript) FindObjectOfType(typeof(UnityWebRequestScript));
                    if (m_instancePrecision == null)
                    {
                        var singletonObject = new GameObject("UnityWebRequestScriptSingleton");
                        m_instancePrecision = singletonObject.AddComponent<UnityWebRequestScript>();

                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return m_instancePrecision;
            }
        }

        public void OnButtonSendScore()
        {
            if (userID.text == string.Empty)
            {
                message.GetComponent<Text>().color = Color.red;
                message.GetComponent<Text>().fontSize = 25;
                message.text = "Error: No userID to send.\nEnter a value in the input field.";
            }
            else if (!rgx.Match(userID.text).Success)
            {
                message.GetComponent<Text>().color = Color.red;
                message.GetComponent<Text>().fontSize = 25;
                message.text = "Error: Invalid userID format.\nEnter a correct value in the input field.";
            }
            else if (score.text == string.Empty)
            {
                message.GetComponent<Text>().color = Color.red;
                message.GetComponent<Text>().fontSize = 25;
                message.text = "Error: No score to send.\nEnter a value in the input field.";
            }
            else if (score.text.Contains("."))
            {
                message.GetComponent<Text>().color = Color.red;
                message.GetComponent<Text>().fontSize = 25;
                message.text = "Error: Invalid score format\n(ex: 10.1)\nEnter decimal values (ex: 10,1)";
            }
            else
            {
                message.GetComponent<Text>().color = Color.white;
                message.GetComponent<Text>().fontSize = 44;
                message.text = "Sending data ...";

                StartCoroutine(SimplePostRequestSendScore(userID.text, score.text));
            }
        }

        public void OnButtonFetchRanks()
        {
            message.GetComponent<Text>().color = Color.white;
            message.text = "Downloading data ...";

            StartCoroutine(SimplePostRequestFetchRanks());
        }

        public void OnButtonGetScore()
        {
            if (userIDToGetScore.text == string.Empty)
            {
                message.GetComponent<Text>().color = Color.red;
                message.GetComponent<Text>().fontSize = 25;
                message.text = "Error: No userID to send.\nEnter a value in the input field.";
            }
            else
            {
                message.GetComponent<Text>().color = Color.white;
                message.GetComponent<Text>().fontSize = 44;
                message.text = "Sending data ...";

                StartCoroutine(SimplePostRequestGetScore(userIDToGetScore.text));
            }
        }

        public void OnButtonDeleteUserID()
        {
            if (userIDToDelete.text == string.Empty)
            {
                message.GetComponent<Text>().color = Color.red;
                message.GetComponent<Text>().fontSize = 25;
                message.text = "Error: No userID to send.\nEnter a value in the input field.";
            }
            else
            {
                message.GetComponent<Text>().color = Color.white;
                message.GetComponent<Text>().fontSize = 44;
                message.text = "Sending data ...";

                StartCoroutine(SimplePostRequestDeleteUserID(userIDToDelete.text));
            }
        }

        IEnumerator SimplePostRequestSendScore(string userID, string score)
        {
            RankingEntry ranking = new RankingEntry();
            ranking.userID = userID;
            ranking.score = float.Parse(score);
            string json = JsonConvert.SerializeObject(ranking);
            //Debug.Log("data to send: " + json);

            var request = new UnityWebRequest(postURL, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            //Debug.Log("Status Code: " + request.responseCode);

            if (request.isNetworkError)
            {
                //Debug.LogError("isNetworkError " + request.error);
                message.GetComponent<Text>().color = Color.red;
                message.GetComponent<Text>().fontSize = 25;
                message.text = request.error;
            }
            if (request.isHttpError)
            {
                //Debug.LogError("isHttpError " + request.error);
                message.GetComponent<Text>().color = Color.red;
                message.GetComponent<Text>().fontSize = 25;
                message.text = request.error;
            }
            else
            {
                message.text = request.downloadHandler.text;
                if (message.text.Contains("Error"))
                {
                    message.GetComponent<Text>().color = Color.red;
                    message.GetComponent<Text>().fontSize = 25;
                }
                else
                {
                    message.GetComponent<Text>().color = Color.green;
                    message.GetComponent<Text>().fontSize = 44;
                }
            }
        }

        IEnumerator SimplePostRequestFetchRanks()
        {
            UnityWebRequest request = UnityWebRequest.Get(getURL);

            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                //Debug.LogError("isNetworkError " + request.error);
                message.GetComponent<Text>().color = Color.red;
                message.GetComponent<Text>().fontSize = 25;
                message.text = request.error;
            }
            if (request.isHttpError)
            {
                //Debug.LogError("isHttpError " + request.error);
                message.GetComponent<Text>().color = Color.red;
                message.GetComponent<Text>().fontSize = 25;
                message.text = request.error;
            }
            else
            {
                message.GetComponent<Text>().color = Color.green;
                message.GetComponent<Text>().fontSize = 44;
                message.text = "Ranks Fetched !";
                string json = request.downloadHandler.text;
                json = "{\"rankingEntryList\":" + json + "}";
                PlayerPrefs.SetString("rankingTable", json);
                PlayerPrefs.Save();
                //Debug.Log(PlayerPrefs.GetString("rankingTable"));
            }
        }

        IEnumerator SimplePostRequestGetScore(string userID)
        {
            string jsonStr = PlayerPrefs.GetString("rankingTable");
            Rankings rankings = JsonUtility.FromJson<Rankings>(jsonStr);
            string userIDToCheck = "\"userID\":\"" + userID + "\"";
            if (!jsonStr.ToString().Contains(userIDToCheck))
            {
                message.GetComponent<Text>().color = Color.red;
                message.GetComponent<Text>().fontSize = 44;
                message.text = "userID " + userID + " not found in Ranks";
            }
            else
            {
                var request = new UnityWebRequest(getUserIDURL + userID, "Get");
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();
                //Debug.Log("Status Code: " + request.responseCode);

                if (request.isNetworkError)
                {
                    //Debug.LogError("isNetworkError " + request.error);
                    message.GetComponent<Text>().color = Color.red;
                    message.GetComponent<Text>().fontSize = 25;
                    message.text = request.error;
                }
                if (request.isHttpError)
                {
                    //Debug.LogError("isHttpError " + request.error);
                    message.GetComponent<Text>().color = Color.red;
                    message.GetComponent<Text>().fontSize = 25;
                    message.text = request.error;
                }
                else
                {
                    message.GetComponent<Text>().color = Color.green;
                    message.GetComponent<Text>().fontSize = 44;

                    string json = request.downloadHandler.text;
                    RankingEntry rank = JsonConvert.DeserializeObject<RankingEntry>(json);

                    float scoreReceived = rank.score;

                    precision = Instance.GetPrecision();

                    if (precision == 0)
                    {
                        scoreReceived = Mathf.Round(scoreReceived);
                    }
                    else
                    {
                        scoreReceived = Mathf.Round(scoreReceived * Mathf.Pow(10f, precision)) / Mathf.Pow(10f, precision);
                    }

                    message.text = "Score: " + scoreReceived.ToString();
                    userIDToGetScore.GetComponent<InputField>().text = "";
                }
            }
        }

        IEnumerator SimplePostRequestDeleteUserID(string userID)
        {
            string jsonStr = PlayerPrefs.GetString("rankingTable");
            Rankings rankings = JsonUtility.FromJson<Rankings>(jsonStr);
            string userIDToCheck = "\"userID\":\"" + userID + "\"";
            if (!jsonStr.ToString().Contains(userIDToCheck))
            {
                message.GetComponent<Text>().color = Color.red;
                message.GetComponent<Text>().fontSize = 44;
                message.text = "userID " + userID + " not found in Ranks";
            }
            else
            {
                var request = new UnityWebRequest(deleteURL + userID, "DELETE");
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();
                //Debug.Log("Status Code: " + request.responseCode);

                if (request.isNetworkError)
                {
                    //Debug.LogError("isNetworkError " + request.error);
                    message.GetComponent<Text>().color = Color.red;
                    message.GetComponent<Text>().fontSize = 25;
                    message.text = request.error;
                }
                if (request.isHttpError)
                {
                    //Debug.LogError("isHttpError " + request.error);
                    message.GetComponent<Text>().color = Color.red;
                    message.GetComponent<Text>().fontSize = 25;
                    message.text = request.error;
                }
                else
                {
                    message.GetComponent<Text>().color = Color.green;
                    message.GetComponent<Text>().fontSize = 44;
                    message.text = "UserID deleted !";
                    userIDToDelete.GetComponent<InputField>().text = "";
                }
            }
        }

        public void SetPrecision(int precision)
        {
            this.precision = precision;
        }

        public int GetPrecision()
        {
            return precision;
        }

        public void SetAdminRights(bool adminRights)
        {
            this.adminRights = adminRights;
            if (adminRights)
                adminRightsTemplate.SetActive(true);
            else
                adminRightsTemplate.SetActive(false);
        }

        public bool GetAdminRights()
        {
            return adminRights;
        }
    }
}
