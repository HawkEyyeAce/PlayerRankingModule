using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingTable : MonoBehaviour
{
    private Transform rankingContainer;
    private Transform rankingTemplate;
    private List<Transform> rankingEntryTransformList;
    // use Unity Script Editor to change ascending value
    private bool ascending;

    public void Start()
    {
        ascending = false;
    }

    private void Awake()
    {
        rankingContainer = GameObject.Find("RankingContainer").transform;
        rankingTemplate = rankingContainer.Find("RankingTemplate");

        rankingTemplate.gameObject.SetActive(false);

        string jsonStr = PlayerPrefs.GetString("rankingTable");
        Rankings rankings = JsonUtility.FromJson<Rankings>(jsonStr);

        SortRankingScore(rankings.rankingEntryList, ascending);

        rankingEntryTransformList = new List<Transform>();
        foreach (RankingEntry rankingEntry in rankings.rankingEntryList)
        {
            CreateRankingEntryTransform(rankingEntry, rankingContainer, rankingEntryTransformList);
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

    private void CreateRankingEntryTransform(RankingEntry rankingEntry, Transform rankingContainer, List<Transform> transformList)
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
                rankingTransform.Find("GoldMedal").gameObject.SetActive(true);
                break;
            case 2:
                rankingTransform.Find("SilverMedal").gameObject.SetActive(true);
                break;
            case 3:
                rankingTransform.Find("BronzeMedal").gameObject.SetActive(true);
                break;
        }
        rankingTransform.Find("id").GetComponent<Text>().text = ranking.ToString();

        string userID = rankingEntry.userID;
        rankingTransform.Find("userID").GetComponent<Text>().text = userID;

        int randomScore = rankingEntry.score;
        rankingTransform.Find("score").GetComponent<Text>().text = randomScore.ToString();

        rankingTransform.Find("background").gameObject.SetActive(ranking % 2 == 1);

        if (ranking == 1 && !ascending)
        {
            rankingTransform.Find("id").GetComponent<Text>().color = Color.green;
            rankingTransform.Find("userID").GetComponent<Text>().color = Color.green;
            rankingTransform.Find("score").GetComponent<Text>().color = Color.green;
        }

        transformList.Add(rankingTransform);
    }
}
