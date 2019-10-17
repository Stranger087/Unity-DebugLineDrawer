using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
  public class DebugLineDrawer : MonoBehaviour
  {
    [NonSerialized] public Material lineMaterial;

    private static List<Vector3> _singleFrameLines = new List<Vector3>();
    private static List<Vector3> _premanentLines = new List<Vector3>();
    private static List<Color> _permanentLinesColors = new List<Color>();
    private static List<Color> _singleFrameColors = new List<Color>();
    private static bool _Initialized;

    private static void Initialize()
    {
      if(Camera.main==null)
        return;
      DebugLineDrawer instance = Camera.main.gameObject.AddComponent<DebugLineDrawer>();
      instance.lineMaterial = new Material(Shader.Find("Sprites/Default"));

      _Initialized = true;
    }

    private void OnDestroy()
    {
      _Initialized = false;
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color, bool permanent = false)
    {
      if (!_Initialized)
        Initialize();


      if (permanent)
      {
        _premanentLines.Add(start);
        _premanentLines.Add(end);
        _permanentLinesColors.Add(color);
      }
      else
      {
        _singleFrameLines.Add(start);
        _singleFrameLines.Add(end);
        _singleFrameColors.Add(color);
      }
    }


    public static void ClearPermanentLines()
    {
      _premanentLines.Clear();
      _permanentLinesColors.Clear();
    }


    void OnPostRender()
    {
      
      lineMaterial.SetPass(0);
      GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
      GL.PushMatrix();

      int colorIndex = 0;

      for (int i = 0; i < _premanentLines.Count; i += 2)
      {
        DrawGlLine(_premanentLines[i], _premanentLines[i + 1], _permanentLinesColors[colorIndex++]);
      }

      colorIndex = 0;

      for (int i = 0; i < _singleFrameLines.Count; i += 2)
      {
        DrawGlLine(_singleFrameLines[i], _singleFrameLines[i + 1], _singleFrameColors[colorIndex++]);
//            Debug.DrawLine(_singleFrameLines[i], _singleFrameLines[i + 1], _singleFrameColors[colorIndex++]);
      }

      _singleFrameLines.Clear();
      _singleFrameColors.Clear();

      GL.PopMatrix();
    }

    private static void DrawGlLine(Vector3 start, Vector3 end, Color color)
    {
      GL.Begin(GL.LINES);
      GL.Color(color);
      GL.Vertex(start);
      GL.Vertex(end);
      GL.End();
    }
  }
}
