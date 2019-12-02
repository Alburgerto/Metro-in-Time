using UnityEngine;

[CreateAssetMenu()]
public class SceneText : ScriptableObject
{
    [TextArea] public string[] m_sceneLines;
}
