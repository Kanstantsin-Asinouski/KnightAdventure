using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshSurfacManagement : MonoBehaviour
{
    
    public static NavMeshSurfacManagement Instance { get; private set; }

    private NavMeshSurface _navMeshSurface;

    private void Awake()
    {
        Instance = this;
        _navMeshSurface = GetComponent<NavMeshSurface>();
        _navMeshSurface.hideEditorLogs = true;
    }

    public void RebakeNavmeshSurface()
    {
        _navMeshSurface.BuildNavMesh();
    }
}