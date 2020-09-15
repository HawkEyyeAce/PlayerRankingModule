using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ranking
{
    public class RankingTable : MonoBehaviour
    {
        private static RankingTable instance;
        private bool ascending;
        private bool showAllRanks;
        private int maximumScoreNumbers;
        private int precision;

        private Transform rankingContainer;
        private Transform rankingTemplate;
        private List<Transform> rankingEntryTransformList;

        private void Start()
        {
            ascending = false;
            showAllRanks = true;
            maximumScoreNumbers = 0;
            precision = 2;
        }

        private void Awake()
        {
            instance = this;

            DrawTables(showAllRanks, maximumScoreNumbers);
        }

        public void DrawTables(bool var = true, int nb = 0)
        {
            rankingContainer = GameObject.Find("RankingContainer").transform;
            rankingTemplate = rankingContainer.Find("RankingTemplate");

            rankingTemplate.gameObject.SetActive(false);

            string jsonStr = PlayerPrefs.GetString("rankingTable");
            Rankings rankings = JsonUtility.FromJson<Rankings>(jsonStr);

            rankings.rankingEntryList = SortRankingScore(rankings.rankingEntryList, ascending);

            if (var == false && nb != 0)
            {
                int dataToDelete = rankings.rankingEntryList.Count - nb;
                if (dataToDelete > 0)
                {
                    rankings.rankingEntryList.RemoveRange(nb, dataToDelete);
                }
            }

            rankingEntryTransformList = new List<Transform>();
            foreach (RankingEntry rankingEntry in rankings.rankingEntryList)
            {
                CreateRankingEntryTransform(rankingEntry, rankingContainer, rankingEntryTransformList, ascending, precision);
            }
        }

        private List<RankingEntry> SortRankingScore(List<RankingEntry> rankingEntryList, bool ascending)
        {
            if (!ascending)
            {
                for (int i = 0; i < rankingEntryList.Count; i++)
                {
                    for (int j = i + 1; j < rankingEntryList.Count; j++)
                    {
                        if (rankingEntryList[j].score > rankingEntryList[i].score)
                        {
                            RankingEntry cpy = rankingEntryList[i];
                            rankingEntryList[i] = rankingEntryList[j];
                            rankingEntryList[j] = cpy;
                        }
                    }
                }
            }
            else if (ascending)
            {
                for (int i = 0; i < rankingEntryList.Count; i++)
                {
                    for (int j = i + 1; j < rankingEntryList.Count; j++)
                    {
                        if (rankingEntryList[j].score < rankingEntryList[i].score)
                        {
                            RankingEntry cpy = rankingEntryList[i];
                            rankingEntryList[i] = rankingEntryList[j];
                            rankingEntryList[j] = cpy;
                        }
                    }
                }
            }

            return rankingEntryList;
        }

        private void CreateRankingEntryTransform(RankingEntry rankingEntry, Transform rankingContainer, List<Transform> transformList, bool ascending, int precision)
        {
            float rankingTemplateHeight = 30f;

            Transform rankingTransform = Instantiate(rankingTemplate, rankingContainer);
            RectTransform rankingRectTransform = rankingTransform.GetComponent<RectTransform>();
            rankingRectTransform.anchoredPosition = new Vector2(0, -rankingTemplateHeight * transformList.Count);
            rankingTransform.gameObject.SetActive(true);

            int ranking = transformList.Count + 1;
            switch (ranking)
            {
                default:
                    break;
                case 1:
                    if (!ascending)
                        rankingTransform.Find("GoldMedal").gameObject.SetActive(true);
                    break;
                case 2:
                    if (!ascending)
                        rankingTransform.Find("SilverMedal").gameObject.SetActive(true);
                    break;
                case 3:
                    if (!ascending)
                        rankingTransform.Find("BronzeMedal").gameObject.SetActive(true);
                    break;
            }
            rankingTransform.Find("id").GetComponent<Text>().text = ranking.ToString();

            string userID = rankingEntry.userID;
            rankingTransform.Find("userID").GetComponent<Text>().text = userID;

            float score = rankingEntry.score;
            if (precision == 0)
            {
                score = Mathf.Round(score);
            }
            else
            {
                score = Mathf.Round(score * Mathf.Pow(10f, precision)) / Mathf.Pow(10f, precision);
            }
            rankingTransform.Find("score").GetComponent<Text>().text = score.ToString();

            rankingTransform.Find("background").gameObject.SetActive(ranking % 2 == 1);

            transformList.Add(rankingTransform);
        }

        public static RankingTable GetInstance()
        {
            return instance;
        }

        public void UpdateTables(bool var = true, int nb = 0)
        {
            foreach (var item in rankingEntryTransformList)
            {
                Destroy(item.gameObject);
            }
            DrawTables(var, nb);
        }

        public bool GetAscending()
        {
            return ascending;
        }

        public void SetAscendingAndUpdateRanks(bool ascending)
        {
            this.ascending = ascending;
            UpdateTables(showAllRanks, maximumScoreNumbers);
        }

        public void SetShowingAllRanks(bool showAllRanks)
        {
            this.showAllRanks = showAllRanks;
            if (showAllRanks)
                UpdateTables(showAllRanks);
        }

        public void SetMaximumScoreNumbers(int maximumScoreNumbers)
        {
            this.maximumScoreNumbers =  maximumScoreNumbers;
            UpdateTables(false, maximumScoreNumbers);
        }

        public void SetPrecisionAndUpdateTables(int precision)
        {
            this.precision = precision;
            UpdateTables(showAllRanks, maximumScoreNumbers);
        }
    }
}
