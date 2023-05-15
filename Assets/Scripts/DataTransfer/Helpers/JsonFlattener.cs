using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

public class JsonFlattener
{
    public static Dictionary<string, string> DeserializeAndFlatten(string json)
    {
        Dictionary<string, string> dict = new();
        JToken token = JToken.Parse(json);
        FillDictionaryFromJToken(dict, token, "");
        return dict;
    }

    private static void FillDictionaryFromJToken(Dictionary<string, string> dict, JToken token, string prefix)
    {
        switch (token.Type)
        {
            case JTokenType.Object:
                foreach (JProperty prop in token.Children<JProperty>())
                {
                    FillDictionaryFromJToken(dict, prop.Value, Join(prefix, prop.Name));
                }
                break;

            case JTokenType.Array:
                int index = 0;
                foreach (JToken value in token.Children())
                {
                    FillDictionaryFromJToken(dict, value, Join(prefix, index.ToString()));
                    index++;
                }
                break;

            default:
                dict.Add(prefix, ((JValue)token).Value.ToString());
                break;
        }
    }

    private static string Join(string prefix, string name)
    {
        return (string.IsNullOrEmpty(prefix) ? name : prefix + "_" + name);
    }
}
