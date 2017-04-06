using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour {

    public Color c1 = Color.red;
    public Color c2 = Color.red;
    public Material lineMaterial;
	private Material m_Material;
    public LineRenderer lineRenderer;
	public Color setColor = Color.white;
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
		m_Material = new Material (lineMaterial);
		lineRenderer.material = m_Material;
        lineRenderer.SetWidth(0.06f, 0.06f);
    }
	void Update()
	{
		m_Material.color = setColor;
	}
}
