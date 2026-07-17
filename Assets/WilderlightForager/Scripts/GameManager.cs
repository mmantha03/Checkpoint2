using UnityEngine;
using UnityEngine.SceneManagement;

namespace WilderlightForager
{
    public enum GameEndState
    {
        Playing,
        Won,
        Lost
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private float timeLimitSeconds = 360f;

        private float remainingTime;
        private PlayerStats playerStats;
        private ExitGateInteraction exitGate;

        public bool IsCampfireLit { get; private set; }
        public GameEndState EndState { get; private set; } = GameEndState.Playing;
        public bool IsGameEnded => EndState != GameEndState.Playing;

        private void Awake()
        {
            Instance = this;
            remainingTime = timeLimitSeconds;
        }

        private void Start()
        {
            playerStats = Object.FindAnyObjectByType<PlayerStats>();
            UpdateHUD();
        }

        private void Update()
        {
            if (IsGameEnded)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                return;
            }

            remainingTime = Mathf.Max(0f, remainingTime - Time.deltaTime);

            if (remainingTime <= 0f)
            {
                Lose("Time ran out. Press R to restart.");
                return;
            }

            UpdateHUD();
        }

        public void RegisterExitGate(ExitGateInteraction gate)
        {
            exitGate = gate;
        }

        public void RegisterCampfireLit(Vector3 position)
        {
            if (IsCampfireLit)
            {
                return;
            }

            IsCampfireLit = true;
            exitGate?.Unlock();
            WilderlightHUD.Instance?.SetObjective("Campfire lit. Follow the green exit marker and press E at the gate.");
            WilderlightAudio.Instance?.PlaySuccess(position);
        }

        public void Win()
        {
            if (IsGameEnded)
            {
                return;
            }

            EndState = GameEndState.Won;
            WilderlightHUD.Instance?.SetEndMessage("You escaped the wilderness.", "Checkpoint complete. Press R to play again.");
            WilderlightAudio.Instance?.PlayWin(GetAudioPosition());
            UpdateHUD();
        }

        public void Lose(string reason)
        {
            if (IsGameEnded)
            {
                return;
            }

            EndState = GameEndState.Lost;
            WilderlightHUD.Instance?.SetEndMessage(reason, "Press R to restart.");
            WilderlightAudio.Instance?.PlayLose(GetAudioPosition());
            UpdateHUD();
        }

        private void UpdateHUD()
        {
            if (playerStats == null)
            {
                playerStats = Object.FindAnyObjectByType<PlayerStats>();
            }

            WilderlightHUD.Instance?.SetStatus(playerStats, remainingTime, IsCampfireLit, EndState);
        }

        private Vector3 GetAudioPosition()
        {
            if (Camera.main != null)
            {
                return Camera.main.transform.position;
            }

            if (playerStats != null)
            {
                return playerStats.transform.position;
            }

            return Vector3.zero;
        }
    }
}
