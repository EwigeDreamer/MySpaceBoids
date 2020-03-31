using UnityEngine;

namespace MyTools.Helpers
{
    public class TimeManager : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            GameObject go = new GameObject(typeof(TimeManager).Name);
            DontDestroyOnLoad(go);
            go.AddComponent<TimeManager>();
        }
#pragma warning disable 618
        static float m_DeltaTime = 0f;
        public static float DeltaTime => m_DeltaTime;
        static float m_UnscaledDeltaTime = 0f;
        public static float UnscaledDeltaTime => m_UnscaledDeltaTime;
        static int m_FPS = 0;
        public static int FPS => m_FPS;
#pragma warning restore 618

        const float checkFpsDelay = 0.5f;
        float m_Timer = checkFpsDelay;
        int m_Counter = 0;

        private void Update()
        {
            m_DeltaTime = Time.deltaTime;
            m_UnscaledDeltaTime = Time.unscaledDeltaTime;
            var timer = m_Timer;
            var counter = m_Counter;

            ++counter;
            timer -= m_UnscaledDeltaTime;
            if (timer < 0f)
            {
                m_FPS = (int)(counter / checkFpsDelay);
                timer = checkFpsDelay;
                counter = 0;
            }
            m_Counter = counter;
            m_Timer = timer;
        }

        private void FixedUpdate()
        {
            m_DeltaTime = Time.deltaTime;
            m_UnscaledDeltaTime = Time.unscaledDeltaTime;
        }
    }
}