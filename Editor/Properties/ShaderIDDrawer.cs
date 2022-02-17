using GatOR.Logic.Properties;
using UnityEditor;

namespace GatOR.Logic.Editor.Properties
{
    [CustomPropertyDrawer(typeof(ShaderID))]
    public class ShaderIDDrawer : HashValueDrawer
    {
        protected override string StringFieldName => nameof(ShaderID.name);
        protected override int GetHash(string value) => ShaderID.GetHash(value);
    }
}
