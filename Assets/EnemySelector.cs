using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelector : MonoBehaviour
{
    public Material highlightMaterial;
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
        highlightAroundCollider(collider, Color.yellow, Color.red, 0.1f);
    }

    void highlightAroundCollider(BoxCollider2D cpType, Color beginColor, Color endColor, float hightlightSize = 0.3f)
    {
        //1. Create new Line Renderer
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = cpType.gameObject.AddComponent<LineRenderer>();

        }

        //2. Assign Material to the new Line Renderer
        lineRenderer.material = highlightMaterial;

        // float zPos = 10f;//Since this is 2D. Make sure it is in the front

        if (cpType is BoxCollider2D)
        {
            Bounds boxBounds = cpType.bounds;
            Vector2 topRight = new Vector2(boxBounds.center.x + boxBounds.extents.x, boxBounds.center.y + boxBounds.extents.y);
            Vector2 topLeft = new Vector2(boxBounds.center.x - boxBounds.extents.x, boxBounds.center.y + boxBounds.extents.y);
            Vector2 bottomRight = new Vector2(boxBounds.center.x + boxBounds.extents.x, boxBounds.center.y - boxBounds.extents.y);
            Vector2 bottomLeft = new Vector2(boxBounds.center.x - boxBounds.extents.x, boxBounds.center.y - boxBounds.extents.y);

            //3. Get the points from the BoxCollider2D
            List<Vector2> pColiderPos = new List<Vector2>();
            pColiderPos.Add(topRight);
            pColiderPos.Add(topLeft);
            pColiderPos.Add(bottomRight);
            pColiderPos.Add(bottomLeft);

            //Set color and width
            lineRenderer.SetColors(beginColor, endColor);
            lineRenderer.SetWidth(hightlightSize, hightlightSize);

            //4. Convert local to world points
            for (int i = 0; i < pColiderPos.Count; i++)
            {
                pColiderPos[i] = cpType.transform.TransformPoint(pColiderPos[i]);
            }

            //5. Set the SetVertexCount of the LineRenderer to the Length of the points
            lineRenderer.SetVertexCount(pColiderPos.Count + 1);
            for (int i = 0; i < pColiderPos.Count; i++)
            {
                //6. Draw the  line
                Vector3 finalLine = pColiderPos[i];
                // finalLine.z = zPos;
                lineRenderer.SetPosition(i, finalLine);

                //7. Check if this is the last loop. Now Close the Line drawn
                if (i == (pColiderPos.Count - 1))
                {
                    finalLine = pColiderPos[0];
                    // finalLine.z = zPos;
                    lineRenderer.SetPosition(pColiderPos.Count, finalLine);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
