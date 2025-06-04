using UnityEngine;
using UnityEngine.UI;

public class SubObjectiveManager : MonoBehaviour
{
    [System.Serializable]
    public class SubObjective
    {
        public string description;
        public bool isCompleted;
        public Text uiText;
    }

    public SubObjective[] objectives;

    void Start()
    {
        UpdateUI();
    }

    // Panggil fungsi ini saat player menyelesaikan objective tertentu
    public void CompleteObjective(int index)
    {
        if (index >= 0 && index < objectives.Length && !objectives[index].isCompleted)
        {
            objectives[index].isCompleted = true;
            Debug.Log("Objective Completed: " + objectives[index].description);
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        foreach (var obj in objectives)
        {
            if (obj.uiText != null)
            {
                obj.uiText.text = (obj.isCompleted ? "✔ " : "• ") + obj.description;
                obj.uiText.color = obj.isCompleted ? Color.green : Color.white;
            }
        }
    }

    // Opsional: mengecek apakah semua objektif selesai
    public bool AllObjectivesCompleted()
    {
        foreach (var obj in objectives)
        {
            if (!obj.isCompleted) return false;
        }
        return true;
    }
}
