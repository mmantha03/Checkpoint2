using UnityEngine;

namespace WilderlightForager
{
    public class CampfireVisual : MonoBehaviour
    {
        [SerializeField] private Light fireLight;
        [SerializeField] private float minIntensity = 1.2f;
        [SerializeField] private float maxIntensity = 2.4f;

        public void SetFireLight(Light light)
        {
            fireLight = light;
        }

        private void Update()
        {
            if (fireLight == null || !fireLight.enabled)
            {
                return;
            }

            fireLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * 5f, 0.25f));
        }
    }
}
