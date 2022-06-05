using UnityEngine;
namespace Cinemachine
{
    [RequireComponent(typeof(CinemachineFreeLook))]
    [RequireComponent(typeof(CinemachineInputProvider))]
    class CinemachineFreeLookZoom : MonoBehaviour
    {
        private CinemachineFreeLook freelook;
        private CinemachineFreeLook.Orbit[] originalOrbits;


        public float speed = 1f;
        [Tooltip("The minimum scale for the orbits")]
        [Range(0.01f, 1f)]
        public float minScale = 0.5f;

        [Tooltip("The maximum scale for the orbits")]
        [Range(1F, 5f)]
        public float maxScale = 1;
        private float scale = 1f;
        private float targetScale = 1f;

        [Tooltip("The input provider for the orbits")]
        public CinemachineInputProvider inputProvider;


        void OnValidate()
        {
            minScale = Mathf.Max(0.01f, minScale);
            maxScale = Mathf.Max(minScale, maxScale);
        }

        void Awake()
        {
            freelook = GetComponentInChildren<CinemachineFreeLook>();
            if (freelook != null)
            {
                originalOrbits = new CinemachineFreeLook.Orbit[freelook.m_Orbits.Length];
                for (int i = 0; i < originalOrbits.Length; i++)
                {
                    originalOrbits[i].m_Height = freelook.m_Orbits[i].m_Height;
                    originalOrbits[i].m_Radius = freelook.m_Orbits[i].m_Radius;
                }
#if UNITY_EDITOR
                SaveDuringPlay.SaveDuringPlay.OnHotSave -= RestoreOriginalOrbits;
                SaveDuringPlay.SaveDuringPlay.OnHotSave += RestoreOriginalOrbits;
#endif
            }
        }

#if UNITY_EDITOR
        private void OnDestroy()
        {
            SaveDuringPlay.SaveDuringPlay.OnHotSave -= RestoreOriginalOrbits;
        }

        private void RestoreOriginalOrbits()
        {
            if (originalOrbits != null)
            {
                for (int i = 0; i < originalOrbits.Length; i++)
                {
                    freelook.m_Orbits[i].m_Height = originalOrbits[i].m_Height;
                    freelook.m_Orbits[i].m_Radius = originalOrbits[i].m_Radius;
                }
            }
        }
#endif

        void Update()
        {
            if (originalOrbits != null)
            {
                float zValue = inputProvider.ZAxis.action.ReadValue<Vector2>().y;
                if(zValue != 0)
                {
                    targetScale = Mathf.Clamp(scale - zValue * speed, minScale, maxScale);
                }
                scale = Mathf.Lerp(scale, targetScale, Time.deltaTime);

                for (int i = 0; i < originalOrbits.Length; i++)
                {
                    freelook.m_Orbits[i].m_Height = originalOrbits[i].m_Height * scale;
                    freelook.m_Orbits[i].m_Radius = originalOrbits[i].m_Radius * scale;
                }
            }
        }
    }
}
