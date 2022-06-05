using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKingdomMetaverse.BuildingSystem
{
    public class BuildingVisualizer : MonoBehaviour
    {
        public float snappingSpeed = 15f;

        private GameObject ghostObject;

        private void Awake()
        {
            GridBuildingSystem.Instance.OnSelectionChanged += (object sender, System.EventArgs e) =>
            {
                CreateVisualPlacer();
            };
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Instance_OnSelectionChanged(object sender, System.EventArgs e)
        {
            CreateVisualPlacer();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            bool result = GridBuildingSystem.Instance.GetMouseSnappedWorldPosition(out Vector3 targetPosition);
            if (!result)
            {
                if (ghostObject != null)
                {
                    ghostObject.SetActive(false);
                }
                return;
            }
            if (ghostObject != null)
            {
                ghostObject.SetActive(true);
            }
            targetPosition.y = 1f;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * snappingSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, GridBuildingSystem.Instance.GetPlacedObjectRotation(), Time.deltaTime * snappingSpeed);
        }


        public void CreateVisualPlacer()
        {
            if (ghostObject != null)
            {
                Destroy(ghostObject);
                ghostObject = null;
            }

            ghostObject = new GameObject();
            Building building = ghostObject.AddComponent<Building>();
            building.Init(GridBuildingSystem.Instance.selectedPlacedObject, this.transform.position, GridBuildingSystem.Instance.GetSystemFacing, false);
            ghostObject.transform.parent = transform;
            ghostObject.transform.localPosition = Vector3.zero;
            ghostObject.transform.localEulerAngles = Vector3.zero;
            SetLayerRecursive(ghostObject.gameObject, 11);
        }

        private void SetLayerRecursive(GameObject targetGO, int layer)
        {
            targetGO.layer = layer;
            foreach (Transform child in targetGO.transform)
            {
                SetLayerRecursive(child.gameObject, layer);
            }
        }
    }

}
