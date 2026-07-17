using UnityEngine;

namespace WilderlightForager
{
    public class SimpleDayNightCycle : MonoBehaviour
    {
        [SerializeField] private Light sun;
        [SerializeField] private float fullDayLengthSeconds = 150f;
        [SerializeField] private Color dayColor = new Color(1f, 0.93f, 0.78f);
        [SerializeField] private Color nightColor = new Color(0.28f, 0.38f, 0.65f);

        private float timeOfDay = 0.2f;

        public void SetSun(Light sunlight)
        {
            sun = sunlight;
        }

        private void Update()
        {
            if (sun == null)
            {
                return;
            }

            timeOfDay += Time.deltaTime / fullDayLengthSeconds;

            if (timeOfDay > 1f)
            {
                timeOfDay = 0f;
            }

            float daylight = Mathf.Clamp01(Mathf.Sin(timeOfDay * Mathf.PI));
            sun.transform.rotation = Quaternion.Euler((timeOfDay * 360f) - 90f, 170f, 0f);
            sun.color = Color.Lerp(nightColor, dayColor, daylight);
            sun.intensity = Mathf.Lerp(0.18f, 1.25f, daylight);
            RenderSettings.ambientLight = Color.Lerp(nightColor * 0.45f, dayColor * 0.72f, daylight);
        }
    }
}
