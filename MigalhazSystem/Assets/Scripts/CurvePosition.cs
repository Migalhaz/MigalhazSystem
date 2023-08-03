using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MigalhaSystem.Extensions;

public class CurvePosition : MonoBehaviour
{
    [SerializeField] UpdateMethod updateMethod = UpdateMethod.Update;
    [SerializeField] DrawMethod drawMethod = DrawMethod.OnDrawGizmosSelected;
    [SerializeField, Min(0)] float speed;
    float current;
    [SerializeField] Transform target;
    [SerializeField] Curve curve;

    private void Start()
    {
        current = 0f;
        curve.OnTMax += Stop;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Active();
        }

        if (updateMethod != UpdateMethod.Update) return;
        Move();
    }

    private void FixedUpdate()
    {
        if (updateMethod != UpdateMethod.FixedUpdate) return;
        Move();
    }

    private void LateUpdate()
    {
        if (updateMethod != UpdateMethod.LateUpdate) return;
        Move();
    }

    void Move()
    {
        Debug.Log("Estou Sendo Chamado no: ".Color(Color.white) + $"{updateMethod}".Color(Color.blue));
        curve.SetT(current);
        current += speed * Time.deltaTime;
        //VerifyT();
        target.position = curve.Point;
    }

    void VerifyT()
    {
        if (current > 1)
        {
            current = 0f;
        }

        if (current < 0)
        {
            current = 1f;
        }
    }

    void Stop()
    {
        Debug.Log("PAROU!".Error());
        current = 0;
        updateMethod = UpdateMethod.N;
    }

    void Active()
    {
        updateMethod = UpdateMethod.Update;
    }

    private void OnDrawGizmos()
    {
        if (drawMethod == DrawMethod.OnDrawGizmos) curve.Draw();
    }

    private void OnDrawGizmosSelected()
    {
        if (drawMethod == DrawMethod.OnDrawGizmosSelected) curve.Draw();
    }
}

[System.Serializable]
public class Curve
{
    #region Variables
    [Header("Curve Settings")]
    [SerializeField, Range(0, 1)] float t;
    [SerializeField] List<Transform> points = new();

    [Space]
    [Header("Draw Settings")]
    [SerializeField] Line externalDraw;
    [SerializeField] DrawAll midDraw;
    [SerializeField] DrawAll internalDraw;
    [SerializeField] Dot point;

    [Header("Actions")]
    public System.Action OnTMax;
    public System.Action OnTMin;

    #region Getters
    public Draw ExternalDraw => externalDraw;
    public DrawAll MidDraw => midDraw;
    public DrawAll InternalDraw => internalDraw;
    public Dot PointDrawSettings => point;
    public Vector3 Point { get { return FinalPoint(); } private set { } }
    public float T => t;

    #endregion
    #endregion

    #region Methods
    public void SetT(float _newT)
    {
        t = Mathf.Clamp01(_newT);
        if (t <= 0)
        {
            OnTMin?.Invoke();
        }

        if (t >= 1)
        {
            OnTMax?.Invoke();
        }
    }

    #region Mathf
    List<Vector3> MiddlePoints()
    {
        int count = points.Count;
        if (count < 3) return null;
        List<Vector3> middlePoints = new List<Vector3>();
        for (int i = 0; i < count - 1; i++)
        {
            middlePoints.Add(Vector3.Lerp(points[i].position, points[i + 1].position, t));
        }
        return middlePoints;
    }

    List<Vector3> LastLine()
    {
        List<Vector3> list = MiddlePoints();
        do
        {
            list = GetMiddleLines(list);
        }
        while (list.Count > 2);
        return list;
    }

    Vector3 FinalPoint()
    {
        return Vector3.Lerp(LastLine()[0], LastLine()[1], t);
    }

    List<Vector3> GetMiddleLines(List<Vector3> _lines)
    {
        int count = _lines.Count;
        if (count < 2) return null;

        List<Vector3> list = new();
        for (int i = 0; i < count - 1; i++)
        {
            list.Add(Vector3.Lerp(_lines[i], _lines[i + 1], t));
        }
        return list;
    }
    #endregion

    #region Draw
    public void Draw()
    {
        ExternalDrawMethod();
        MiddleDrawMethod();
        DrawInternalPointsMethod();
        PointDrawMethod();
    }

    void ExternalDrawMethod()
    {
        if (!CanDraw() || !externalDraw.CanDraw) return;
        Gizmos.color = externalDraw.LineColor;
        int count = points.Count;
        if (count <= 1) return;
        for (int i = 0; i < count - 1; i++)
        {
            Gizmos.DrawLine(points[i].position, points[i + 1].position);
        }
    }

    void MiddleDrawMethod()
    {
        if (!CanDraw()) return;
        
        List<Vector3> midPoints = MiddlePoints();

        if (midDraw.DotSettings.CanDraw)
        {
            Gizmos.color = midDraw.DotSettings.DotColor;
            foreach (Vector3 p in midPoints)
            {
                Gizmos.DrawSphere(p, midDraw.DotSettings.DotSize);
            }
        }

        if (midDraw.LineSettings.CanDraw)
        {
            Gizmos.color = midDraw.LineSettings.LineColor;
            for (int i = 0; i < midPoints.Count - 1; i++)
            {
                Gizmos.DrawLine(midPoints[i], midPoints[i + 1]);
            }
        }
        Gizmos.color = Color.white;
    }

    void DrawInternalPointsMethod()
    {
        
        if (!CanDraw()) return;
        List<Vector3> lastPoints = LastLine();

        if (internalDraw.DotSettings.CanDraw)
        {
            Gizmos.color = internalDraw.DotSettings.DotColor;
            foreach (Vector3 p in lastPoints)
            {
                Gizmos.DrawSphere(p, internalDraw.DotSettings.DotSize);
            }

        }

        if (internalDraw.LineSettings.CanDraw)
        {
            Gizmos.color = internalDraw.LineSettings.LineColor;
            for (int i = 0; i < lastPoints.Count - 1; i++)
            {
                Gizmos.DrawLine(lastPoints[i], lastPoints[i + 1]);
            }
        }

        Gizmos.color = Color.white;
    }

    void PointDrawMethod()
    {
        if (!CanDraw() || !point.CanDraw) return;
        Gizmos.color = point.DotColor;
        Gizmos.DrawSphere(Point, point.DotSize);

        Gizmos.color = Color.white;
    }

    bool CanDraw()
    {
        return points.Count >= 4;
    }
    #endregion
    #endregion
}

public abstract class Draw
{
    [SerializeField] protected bool canDraw = true;
    
    public bool CanDraw => canDraw;
    
}

[System.Serializable]
public class Line : Draw
{
    [SerializeField] Color lineColor = Color.white;
    public Color LineColor => lineColor;
}

[System.Serializable]
public class Dot : Draw
{
    [SerializeField] Color dotColor = Color.red;
    [SerializeField, Min(0)] float dotSize = .1f;
    public Color DotColor => dotColor;
    public float DotSize => dotSize;   
}

[System.Serializable]
public class DrawAll
{
    [SerializeField] Line line;
    [SerializeField] Dot dot;
    public Line LineSettings => line;
    public Dot DotSettings => dot;
}
