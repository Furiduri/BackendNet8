using Newtonsoft.Json.Linq;

namespace GCatcode.Utils
{
    /// <summary>
    /// Class Json Methods.
    /// </summary>
    public static class JsonMethods
    {
        /// <summary>
        /// Deep compare two NewtonSoft JObjects. If they don't match, returns text diffs.
        /// </summary>
        /// <param name="newObject">The expected results.</param>
        /// <param name="oldObject">The actual results.</param>
        /// <returns>Text string.</returns>
        public static JObject GetJsonDifference(JObject newObject, JObject oldObject)
        {
            JObject diff = new JObject();

            foreach (var property in newObject.Properties())
            {
                string propertyName = property.Name;
                JToken newObjectValue = property.Value;
                JToken oldObjectValue = oldObject.GetValue(propertyName);

                if (oldObjectValue == null)
                {
                    // Property exists in newObject but not in oldObject, so it was removed
                    diff[propertyName] = new JValue(string.Empty); // Use an empty string as the marker for removal
                }
                else if (newObjectValue.Type == JTokenType.Object && oldObjectValue.Type == JTokenType.Object)
                {
                    // Recursive call to handle nested objects
                    JObject nestedDiff = GetJsonDifference((JObject)newObjectValue, (JObject)oldObjectValue);
                    if (nestedDiff.HasValues)
                    {
                        diff[propertyName] = nestedDiff;
                    }
                }
                else if (!JToken.DeepEquals(newObjectValue, oldObjectValue))
                {
                    // Property values are different, update it in the diff object
                    diff[propertyName] = oldObjectValue.DeepClone();
                }
            }

            foreach (var property in oldObject.Properties())
            {
                string propertyName = property.Name;
                JToken newObjectValue = newObject.GetValue(propertyName);
                JToken oldObjectValue = property.Value;

                if (newObjectValue == null)
                {
                    // Property exists in oldObject but not in newObject, so it was added
                    diff[propertyName] = oldObjectValue.DeepClone();
                }
            }

            return diff;
        }
    }
}