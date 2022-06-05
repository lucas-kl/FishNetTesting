
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKingdomMetaverse.BuildingSystem
{
    public class Building : MonoBehaviour
    {

        private PlaceableObject data;
        [SerializeField]
        private float time = 0;

        private GameObject createdBuildingModel;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if(createdBuildingModel)
            {
                if(time >= 1)
                {
                    return;
                }
                time += Time.deltaTime;
                time = Mathf.Clamp(time, 0, 1);
                createdBuildingModel.transform.localScale = new Vector3(1, time, 1);
            }
        }

        public void Init(PlaceableObject placableObject, Vector3 position, PlaceableObject.Facing facing, bool showAnimation)
        {
            this.data = placableObject;
            this.name = placableObject.name;
            CreateModel(position, Quaternion.Euler(0, placableObject.GetRotationAngle(facing),0), showAnimation);
        }

        private GameObject CreateModel(Vector3 position, Quaternion rotation, bool showAnimation)
        {
            if (!data)
            {
                return null;
            }
            // General Object
            this.transform.position = position;
            this.transform.rotation = rotation;

            if (data.baseGround)
            {
                GameObject newBaseGround = Instantiate(data.baseGround, this.transform);
                newBaseGround.transform.localScale = new Vector3(data.width, 1, data.height);
            }

            if (data.model)
            {
                GameObject newModel = Instantiate(data.model, this.transform);
                createdBuildingModel = newModel;
            }

            if (showAnimation)
                time = 0;
            else
                time = 1;

            return this.gameObject;
        }

    }

}
