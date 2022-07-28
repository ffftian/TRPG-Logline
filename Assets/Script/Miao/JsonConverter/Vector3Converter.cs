

using Newtonsoft.Json;
using System;
using UnityEngine;

public class Vector3Converter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        //if(objectType==typeof(Vector2[]))
        //{
        //    objectType = typeof(Vector3Converter);
        //}
        return (objectType == typeof(Vector3) || objectType == typeof(Vector3[]));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        string[] v3Str = reader.Value.ToString().Split(',');
        return new Vector3(float.Parse(v3Str[0]), float.Parse(v3Str[1]), float.Parse(v3Str[2]));
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Vector3 v = (Vector3)value;
        serializer.Serialize(writer,$"{v.x},{v.y},{v.z}");
    }
}
