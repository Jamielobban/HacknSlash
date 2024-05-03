using UnityEngine;

public static class ExtensionMethods 
{
    public static void UpdateXPosition(this Transform trans, float value)
    {
        trans.position = new Vector3(value, trans.position.y, trans.position.z);
    }
    public static void UpdateYPosition(this Transform trans, float value)
    {
        trans.position = new Vector3(trans.position.x, value, trans.position.z);
    }
    public static void UpdateZPosition(this Transform trans, float value)
    {
        trans.position = new Vector3(trans.position.x, trans.position.y, value);
    }
}
