using UnityEngine;

namespace WilderlightForager
{
    public class WilderlightHUD : MonoBehaviour
    {
        public static WilderlightHUD Instance { get; private set; }

        private string objectiveText = "Collect supplies and return to camp.";
        private string inventoryText = "Wood: 0   Food: 0   Crystal: 0";
        private string statusText = "Health: 100   Time: 6:00";
        private string promptText = "";
        private string endTitle = "";
        private string endSubtitle = "";
        private GUIStyle panelStyle;
        private GUIStyle mainTextStyle;
        private GUIStyle promptStyle;
        private GUIStyle endTitleStyle;
        private GUIStyle endSubtitleStyle;
        private Texture2D panelTexture;

        private void Awake()
        {
            Instance = this;
        }

        public void SetObjective(string message)
        {
            objectiveText = message;
        }

        public void RefreshInventory(PlayerInventory inventory)
        {
            inventoryText = $"Wood: {inventory.Wood}   Food: {inventory.Food}   Crystal: {inventory.Crystal}";
        }

        public void ShowPrompt(string message)
        {
            promptText = message;
        }

        public void HidePrompt()
        {
            promptText = "";
        }

        public void SetStatus(PlayerStats stats, float remainingTime, bool campfireLit, GameEndState endState)
        {
            int seconds = Mathf.CeilToInt(remainingTime);
            int minutes = seconds / 60;
            int leftoverSeconds = seconds % 60;
            int health = stats != null ? Mathf.CeilToInt(stats.CurrentHealth) : 100;
            string goal = campfireLit ? "Exit open" : "Campfire locked";
            statusText = $"Health: {health}   Time: {minutes}:{leftoverSeconds:00}   {goal}";
        }

        public void SetEndMessage(string title, string subtitle)
        {
            endTitle = title;
            endSubtitle = subtitle;
        }

        private void OnGUI()
        {
            if (panelStyle == null)
            {
                BuildStyles();
            }

            float panelWidth = Mathf.Min(440f, Screen.width - 32f);
            Rect panelRect = new Rect(16f, 16f, panelWidth, 116f);
            GUI.Box(panelRect, GUIContent.none, panelStyle);
            GUI.Label(new Rect(panelRect.x + 16f, panelRect.y + 12f, panelRect.width - 32f, 26f), objectiveText, mainTextStyle);
            GUI.Label(new Rect(panelRect.x + 16f, panelRect.y + 46f, panelRect.width - 32f, 26f), inventoryText, mainTextStyle);
            GUI.Label(new Rect(panelRect.x + 16f, panelRect.y + 78f, panelRect.width - 32f, 26f), statusText, mainTextStyle);

            if (!string.IsNullOrWhiteSpace(promptText))
            {
                Vector2 size = promptStyle.CalcSize(new GUIContent(promptText));
                Rect promptRect = new Rect((Screen.width - size.x - 36f) * 0.5f, Screen.height - 92f, size.x + 36f, 44f);
                GUI.Box(promptRect, GUIContent.none, panelStyle);
                GUI.Label(new Rect(promptRect.x + 18f, promptRect.y + 10f, promptRect.width - 36f, 24f), promptText, promptStyle);
            }

            if (!string.IsNullOrWhiteSpace(endTitle))
            {
                Rect endRect = new Rect((Screen.width - 560f) * 0.5f, (Screen.height - 160f) * 0.5f, 560f, 160f);
                GUI.Box(endRect, GUIContent.none, panelStyle);
                GUI.Label(new Rect(endRect.x + 24f, endRect.y + 28f, endRect.width - 48f, 46f), endTitle, endTitleStyle);
                GUI.Label(new Rect(endRect.x + 24f, endRect.y + 86f, endRect.width - 48f, 42f), endSubtitle, endSubtitleStyle);
            }
        }

        private void BuildStyles()
        {
            panelTexture = new Texture2D(1, 1);
            panelTexture.SetPixel(0, 0, new Color(0.05f, 0.08f, 0.07f, 0.78f));
            panelTexture.Apply();

            panelStyle = new GUIStyle(GUI.skin.box);
            panelStyle.normal.background = panelTexture;

            mainTextStyle = new GUIStyle(GUI.skin.label);
            mainTextStyle.fontSize = 18;
            mainTextStyle.normal.textColor = new Color(0.95f, 0.97f, 0.9f);
            mainTextStyle.wordWrap = true;

            promptStyle = new GUIStyle(mainTextStyle);
            promptStyle.alignment = TextAnchor.MiddleCenter;
            promptStyle.fontStyle = FontStyle.Bold;

            endTitleStyle = new GUIStyle(mainTextStyle);
            endTitleStyle.alignment = TextAnchor.MiddleCenter;
            endTitleStyle.fontSize = 30;
            endTitleStyle.fontStyle = FontStyle.Bold;

            endSubtitleStyle = new GUIStyle(mainTextStyle);
            endSubtitleStyle.alignment = TextAnchor.MiddleCenter;
            endSubtitleStyle.fontSize = 20;
        }
    }
}
