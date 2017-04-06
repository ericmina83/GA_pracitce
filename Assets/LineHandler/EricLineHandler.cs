using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EricLineHandler : MonoBehaviour
{

	public GameObject linePrefab;

    public GameObject StaticLines;
    public GameObject DynamicLines;

	public enum LINE_TYPE { STATIC, DYNAMIC };

	//line instances storage.
	List<GameObject> staticLineInstances = new List<GameObject>();
	List<GameObject> dynamicLineInstances = new List<GameObject>();

	public void clearLines(LINE_TYPE lineType)
	{
        if (lineType == LINE_TYPE.STATIC)
        {
            for(int i = 0; i < staticLineInstances.Count; i++)
            {
                GameObject.Destroy(staticLineInstances[i]);
            }
            staticLineInstances.Clear();
        }
        else
        {
            for (int i = 0; i < dynamicLineInstances.Count; i++)
            {
                GameObject.Destroy(dynamicLineInstances[i]);
            }
            dynamicLineInstances.Clear();
        }
    }

	public GameObject drawLine(Vector3 pt1, Vector3 pt2, LINE_TYPE lineType, Color color)
	{
		GameObject line_tmp = (GameObject)GameObject.Instantiate(linePrefab, pt1, new Quaternion());

        Line line = addLine(line_tmp, lineType);
        line.setColor = color;

        line.lineRenderer.SetVertexCount(2);
        line.lineRenderer.SetPosition(0, pt1);
        line.lineRenderer.SetPosition(1, pt2);

		return line_tmp;
	}

	public GameObject drawCircle(Vector3 center, float radius, LINE_TYPE lineType, Color color)
	{
		GameObject line_tmp = ((GameObject)GameObject.Instantiate(linePrefab, center, new Quaternion()));

        Line line = addLine(line_tmp, lineType);

        line.setColor = color;
        line.lineRenderer.SetVertexCount(201);

		for (int i = 0; i <= 200; i++)
		{
			float angleRad = i / 200.0f * 2.0f * Mathf.PI;

			Vector3 circlePosition = center + new Vector3(radius * Mathf.Cos(angleRad), 0.0f, radius * Mathf.Sin(angleRad));

            line.lineRenderer.SetPosition(i, circlePosition);
		}
        
		return line_tmp;
	}

	public GameObject drawPointsList(List<Vector3> list, LINE_TYPE lineType, Color color)
	{
        GameObject line_tmp = (GameObject)GameObject.Instantiate(linePrefab, list[0], new Quaternion());

        Line line = addLine(line_tmp, lineType);
        
        line.setColor = color;
        line.lineRenderer.SetVertexCount(list.Count);
        
		for (int i = 0; i < list.Count; i++)
		{
            line.lineRenderer.SetPosition(i, list[i]);
		}

		return line_tmp;
	}

	public Color getBallColor(int id) {
		switch (id)
		{
		case 0:
			return Color.white;
		case 1:
			return new Color(1.0f,1.0f,0.0f) ;//Yellow
		case 2:
			return new Color(0.0f,0.0f,1.0f); //blue
		case 3:
			return new Color(1.0f,0.0f,0.0f); //red
		case 4:
			return new Color(0.5f, 0f, 1.0f); 
		case 5:
			return new Color(1.0f, 0.27f, 0.0f);
		case 6:
			return new Color(0.0f,0.8f,0.0f);
		case 7:
			return new Color(0.5450f, 0.2705f, 0.07451f);
		case 8:
			return new Color(0.50f, 0.50f, 0.50f);
		case 9:
			return new Color(0.8f, 0.8f, 0.0f);//Yellow
		case 10:
			return new Color(0.0f, 0.7490f, 1.0f);
		case 11:
			return new Color(0.94f,0.5f,0.5f);
		case 12:
			return new Color(0.5f, 0f, 1.0f);
		case 13:
			return new Color(1.0f, 0.5f, 0.0f);
		case 14:
			return new Color(0.0f, 1.0f, 0.0f);
		case 15:
			return new Color(165.0f/256, 42.0f/256, 42.0f/256);
		default:
			return Color.magenta;
		}
	}

    public Line addLine(GameObject line, LINE_TYPE lineType)
    {
        if (lineType == LINE_TYPE.STATIC)
        {
            staticLineInstances.Add(line);
            line.transform.parent = StaticLines.transform;
        }
        else
        {
            dynamicLineInstances.Add(line);
            line.transform.parent = DynamicLines.transform;
        }

        return line.GetComponent<Line>();
    }
}