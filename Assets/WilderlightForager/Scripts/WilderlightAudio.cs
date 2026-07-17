using UnityEngine;

namespace WilderlightForager
{
    public class WilderlightAudio : MonoBehaviour
    {
        public static WilderlightAudio Instance { get; private set; }

        private AudioClip woodPickupClip;
        private AudioClip foodPickupClip;
        private AudioClip crystalPickupClip;
        private AudioClip campfireClip;
        private AudioClip campfireLoopClip;
        private AudioClip deniedClip;
        private AudioClip promptClip;
        private AudioClip damageClip;
        private AudioClip healClip;
        private AudioClip successClip;
        private AudioClip winClip;
        private AudioClip loseClip;
        private AudioClip footstepClipA;
        private AudioClip footstepClipB;
        private AudioClip jumpClip;
        private AudioClip landClip;

        private float lastPromptTime;

        private void Awake()
        {
            Instance = this;
            woodPickupClip = CreateKnock("wood_pickup", 0.16f, 0.36f);
            foodPickupClip = CreatePop("food_pickup", 0.14f, 0.28f);
            crystalPickupClip = CreateSparkle("crystal_pickup", 0.35f, 0.22f);
            campfireClip = CreateWhoosh("campfire_light", 0.65f, 0.2f);
            campfireLoopClip = CreateCrackle("campfire_crackle_loop", 2.5f, 0.12f);
            deniedClip = CreateTone("denied", 145f, 0.2f, 0.18f);
            promptClip = CreateTone("prompt_blip", 520f, 0.08f, 0.12f);
            damageClip = CreateTone("damage", 95f, 0.18f, 0.25f);
            healClip = CreateSparkle("heal", 0.4f, 0.18f);
            successClip = CreateSparkle("success", 0.5f, 0.24f);
            winClip = CreateSparkle("win", 0.8f, 0.28f);
            loseClip = CreateTone("lose", 80f, 0.6f, 0.25f);
            footstepClipA = CreateFootstep("footstep_a", 0.12f, 0.18f);
            footstepClipB = CreateFootstep("footstep_b", 0.12f, 0.15f);
            jumpClip = CreateJump("jump", 0.18f, 0.16f);
            landClip = CreateLand("land", 0.16f, 0.22f);
            StartAmbience();
        }

        public void PlayPickup(ResourceType resourceType, Vector3 position)
        {
            switch (resourceType)
            {
                case ResourceType.Wood:
                    PlayAtPoint(woodPickupClip, position, 0.75f, Random.Range(0.92f, 1.08f));
                    break;
                case ResourceType.Food:
                    PlayAtPoint(foodPickupClip, position, 0.65f, Random.Range(0.94f, 1.1f));
                    break;
                case ResourceType.Crystal:
                    PlayAtPoint(crystalPickupClip, position, 0.75f, Random.Range(0.96f, 1.12f));
                    break;
            }
        }

        public void PlayPrompt(Vector3 position)
        {
            if (Time.time - lastPromptTime < 0.15f)
            {
                return;
            }

            lastPromptTime = Time.time;
            PlayAtPoint(promptClip, position, 0.35f, 1f);
        }

        public void PlayFootstep(Vector3 position, bool sprinting)
        {
            AudioClip clip = Random.value > 0.5f ? footstepClipA : footstepClipB;
            float volume = sprinting ? 0.42f : 0.32f;
            PlayAtPoint(clip, position, volume, Random.Range(0.88f, 1.12f));
        }

        public void PlayJump(Vector3 position)
        {
            PlayAtPoint(jumpClip, position, 0.45f, Random.Range(0.95f, 1.06f));
        }

        public void PlayLand(Vector3 position)
        {
            PlayAtPoint(landClip, position, 0.5f, Random.Range(0.9f, 1.05f));
        }

        public void PlayCampfire(Vector3 position)
        {
            PlayAtPoint(campfireClip, position, 0.75f, 1f);
            StartCampfireLoop(position);
        }

        public void PlayDenied(Vector3 position)
        {
            PlayAtPoint(deniedClip, position, 0.55f, 0.95f);
        }

        public void PlayDamage(Vector3 position)
        {
            PlayAtPoint(damageClip, position, 0.65f, Random.Range(0.9f, 1.05f));
        }

        public void PlayHeal(Vector3 position)
        {
            PlayAtPoint(healClip, position, 0.7f, 1f);
        }

        public void PlaySuccess(Vector3 position)
        {
            PlayAtPoint(successClip, position, 0.7f, 1f);
        }

        public void PlayWin(Vector3 position)
        {
            PlayAtPoint(winClip, position, 0.85f, 1f);
        }

        public void PlayLose(Vector3 position)
        {
            PlayAtPoint(loseClip, position, 0.75f, 0.85f);
        }

        private void StartCampfireLoop(Vector3 position)
        {
            GameObject sourceObject = new GameObject("Campfire Crackle Audio");
            sourceObject.transform.position = position;
            AudioSource source = sourceObject.AddComponent<AudioSource>();
            source.clip = campfireLoopClip;
            source.loop = true;
            source.volume = 0.45f;
            source.spatialBlend = 0.85f;
            source.minDistance = 3f;
            source.maxDistance = 18f;
            source.Play();
        }

        private void StartAmbience()
        {
            AudioClip ambience = CreateAmbience("forest_ambience", 6f);
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = ambience;
            source.loop = true;
            source.volume = 0.26f;
            source.spatialBlend = 0f;
            source.Play();
        }

        private static void PlayAtPoint(AudioClip clip, Vector3 position, float volume, float pitch)
        {
            if (clip == null)
            {
                return;
            }

            GameObject sourceObject = new GameObject("One Shot Audio");
            sourceObject.transform.position = position;
            AudioSource source = sourceObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.spatialBlend = 0.65f;
            source.minDistance = 2.5f;
            source.maxDistance = 20f;
            source.Play();
            Destroy(sourceObject, (clip.length / Mathf.Max(0.1f, pitch)) + 0.1f);
        }

        private static AudioClip CreateTone(string clipName, float frequency, float duration, float volume)
        {
            int sampleRate = 44100;
            int sampleCount = Mathf.CeilToInt(sampleRate * duration);
            float[] data = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float time = i / (float)sampleRate;
                float fade = 1f - (i / (float)sampleCount);
                data[i] = Mathf.Sin(time * frequency * Mathf.PI * 2f) * volume * fade;
            }

            AudioClip clip = AudioClip.Create(clipName, sampleCount, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        private static AudioClip CreateFootstep(string clipName, float duration, float volume)
        {
            return CreateNoiseEnvelope(clipName, duration, volume, 55f, 140f);
        }

        private static AudioClip CreateLand(string clipName, float duration, float volume)
        {
            return CreateNoiseEnvelope(clipName, duration, volume, 40f, 95f);
        }

        private static AudioClip CreateKnock(string clipName, float duration, float volume)
        {
            int sampleRate = 44100;
            int sampleCount = Mathf.CeilToInt(sampleRate * duration);
            float[] data = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float time = i / (float)sampleRate;
                float fade = Mathf.Exp(-time * 22f);
                float tone = Mathf.Sin(time * 185f * Mathf.PI * 2f) + Mathf.Sin(time * 310f * Mathf.PI * 2f) * 0.35f;
                data[i] = tone * fade * volume;
            }

            AudioClip clip = AudioClip.Create(clipName, sampleCount, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        private static AudioClip CreatePop(string clipName, float duration, float volume)
        {
            int sampleRate = 44100;
            int sampleCount = Mathf.CeilToInt(sampleRate * duration);
            float[] data = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float time = i / (float)sampleRate;
                float fade = Mathf.Exp(-time * 18f);
                float sweep = Mathf.Lerp(290f, 540f, time / duration);
                data[i] = Mathf.Sin(time * sweep * Mathf.PI * 2f) * fade * volume;
            }

            AudioClip clip = AudioClip.Create(clipName, sampleCount, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        private static AudioClip CreateSparkle(string clipName, float duration, float volume)
        {
            int sampleRate = 44100;
            int sampleCount = Mathf.CeilToInt(sampleRate * duration);
            float[] data = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float time = i / (float)sampleRate;
                float fade = 1f - (time / duration);
                float bell = Mathf.Sin(time * 880f * Mathf.PI * 2f) * 0.5f;
                bell += Mathf.Sin(time * 1320f * Mathf.PI * 2f) * 0.35f;
                bell += Mathf.Sin(time * 1760f * Mathf.PI * 2f) * 0.18f;
                data[i] = bell * fade * fade * volume;
            }

            AudioClip clip = AudioClip.Create(clipName, sampleCount, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        private static AudioClip CreateJump(string clipName, float duration, float volume)
        {
            int sampleRate = 44100;
            int sampleCount = Mathf.CeilToInt(sampleRate * duration);
            float[] data = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float time = i / (float)sampleRate;
                float fade = 1f - (time / duration);
                float sweep = Mathf.Lerp(240f, 520f, time / duration);
                data[i] = Mathf.Sin(time * sweep * Mathf.PI * 2f) * fade * volume;
            }

            AudioClip clip = AudioClip.Create(clipName, sampleCount, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        private static AudioClip CreateWhoosh(string clipName, float duration, float volume)
        {
            int sampleRate = 44100;
            int sampleCount = Mathf.CeilToInt(sampleRate * duration);
            float[] data = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float time = i / (float)sampleRate;
                float fadeIn = Mathf.Clamp01(time * 6f);
                float fadeOut = 1f - Mathf.Clamp01((time - duration * 0.45f) / (duration * 0.55f));
                float noise = Mathf.PerlinNoise(time * 55f, 2.2f) - 0.5f;
                float low = Mathf.Sin(time * 95f * Mathf.PI * 2f) * 0.35f;
                data[i] = (noise + low) * fadeIn * fadeOut * volume;
            }

            AudioClip clip = AudioClip.Create(clipName, sampleCount, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        private static AudioClip CreateCrackle(string clipName, float duration, float volume)
        {
            int sampleRate = 22050;
            int sampleCount = Mathf.CeilToInt(sampleRate * duration);
            float[] data = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float time = i / (float)sampleRate;
                float ember = Mathf.Sin(time * 75f * Mathf.PI * 2f) * 0.08f;
                float hiss = (Mathf.PerlinNoise(time * 80f, 8.1f) - 0.5f) * 0.18f;
                float pop = Mathf.PerlinNoise(time * 15f, 1.7f) > 0.72f ? Mathf.PerlinNoise(time * 240f, 4.4f) * 0.4f : 0f;
                data[i] = (ember + hiss + pop) * volume;
            }

            AudioClip clip = AudioClip.Create(clipName, sampleCount, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        private static AudioClip CreateNoiseEnvelope(string clipName, float duration, float volume, float lowTone, float highTone)
        {
            int sampleRate = 44100;
            int sampleCount = Mathf.CeilToInt(sampleRate * duration);
            float[] data = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float time = i / (float)sampleRate;
                float fade = Mathf.Exp(-time * 28f);
                float noise = Mathf.PerlinNoise(time * 210f, 0.35f) - 0.5f;
                float tone = Mathf.Sin(time * lowTone * Mathf.PI * 2f) * 0.45f;
                tone += Mathf.Sin(time * highTone * Mathf.PI * 2f) * 0.18f;
                data[i] = (noise + tone) * fade * volume;
            }

            AudioClip clip = AudioClip.Create(clipName, sampleCount, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        private static AudioClip CreateAmbience(string clipName, float duration)
        {
            int sampleRate = 22050;
            int sampleCount = Mathf.CeilToInt(sampleRate * duration);
            float[] data = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float time = i / (float)sampleRate;
                float low = Mathf.Sin(time * 70f * Mathf.PI * 2f) * 0.03f;
                float high = Mathf.Sin(time * 420f * Mathf.PI * 2f) * 0.012f;
                float breeze = (Mathf.PerlinNoise(time * 0.8f, 0.1f) - 0.5f) * 0.08f;
                float insects = Mathf.Sin(time * 1280f * Mathf.PI * 2f) * Mathf.PerlinNoise(time * 3.5f, 3f) * 0.009f;
                data[i] = low + high + breeze + insects;
            }

            AudioClip clip = AudioClip.Create(clipName, sampleCount, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }
    }
}
