using GatOR.Logic.Properties;
using UnityEditor;

namespace GatOR.Logic.Editor.Properties
{
    [CustomPropertyDrawer(typeof(AnimatorID))]
    public class AnimatorIDDrawer : HashValueDrawer
    {
        protected override string StringFieldName => nameof(AnimatorID.name);
        protected override int GetHash(string value) => AnimatorID.GetHash(value);
    }
}
