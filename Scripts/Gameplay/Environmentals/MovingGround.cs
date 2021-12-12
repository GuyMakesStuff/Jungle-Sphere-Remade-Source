using System.Collections;
using JungleSphereRemake.Managers;
using UnityEngine;

namespace JungleSphereRemake.Gameplay.Environmentals
{
    public class MovingGround : MonoBehaviour
    {
        MeshRenderer meshRenderer;
        Vector2 BaseTilling;
        Material Mat;

        // Start is called before the first frame update
        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            Mat = new Material(Shader.Find("Standard"));
            Mat.CopyPropertiesFromMaterial(meshRenderer.sharedMaterial);
            Mat.name = "Moving Ground_" + GetHashCode();
            BaseTilling = Mat.GetTextureScale("_MainTex");
            meshRenderer.sharedMaterial = Mat;
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 BaseOffset = Mat.GetTextureOffset("_MainTex");
            BaseOffset.y = -GameManager.ZPos;
            Mat.SetTextureOffset("_MainTex", BaseOffset);
            Mat.SetTextureOffset("_DetailAlbedoMap", BaseOffset);
        }
    }
}