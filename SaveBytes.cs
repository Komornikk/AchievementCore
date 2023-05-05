using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>Saves classes with BinaryFormatters</summary>
public static class SaveBytes
{
    static readonly StreamingContext sc = new StreamingContext(StreamingContextStates.All);
    static readonly BinaryFormatter bf = getBinaryFormatter();

    static BinaryFormatter getBinaryFormatter()
    {
        var bf = new BinaryFormatter();
        var ss = new SurrogateSelector();

        ss.AddSurrogate(typeof(Color), sc, new sColor());
        ss.AddSurrogate(typeof(Quaternion), sc, new sQuaternion());
        ss.AddSurrogate(typeof(Vector4), sc, new sVector4());
        ss.AddSurrogate(typeof(Vector3), sc, new sVector3());
        ss.AddSurrogate(typeof(Vector2), sc, new sVector2());

        bf.SurrogateSelector = ss;
        return bf;
    }

    /// <summary>Adds a serialization surrogate to the list</summary>
    /// <remarks>Ensure you call this before loading the save</remarks>
    public static void addSurrogate(Type type, ISerializationSurrogate surrogate) => ((SurrogateSelector)bf.SurrogateSelector).AddSurrogate(type, sc, surrogate);

    /// <summary>Save a list of data to the save file</summary>
    public static void save(string saveFile, params object[] data) => save(saveFile, (object)data);

    /// <summary>Save data to the save file</summary>
    public static void save(string saveFile, object data)
    {
        if (data == null) return;

        var fs = new FileStream(saveFile, FileMode.Create);
        bf.Serialize(fs, data);
        fs.Close();
    }

    /// <summary>Load and return a list of data from the save file</summary>
    public static object[] load(string saveFile, object[] ifFail = null) => load<object[]>(saveFile, ifFail);

    /// <summary>Load and return data from the save file</summary>
    public static T load<T>(string saveFile, T ifFail = null) where T : class
    {
        try
        {
            var fs = new FileStream(saveFile, FileMode.Open);
            var data = bf.Deserialize(fs);
            fs.Close();
            return data as T;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ifFail;
        }
    }

    class sColor : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var color = (Color)obj;

            info.AddValue("r", color.r);
            info.AddValue("g", color.g);
            info.AddValue("b", color.b);
            info.AddValue("a", color.a);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var color = (Color)obj;

            color.r = info.GetSingle("r");
            color.g = info.GetSingle("g");
            color.b = info.GetSingle("b");
            color.a = info.GetSingle("a");

            return color;
        }
    }

    class sQuaternion : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var quaternion = (Quaternion)obj;

            info.AddValue("x", quaternion.x);
            info.AddValue("y", quaternion.y);
            info.AddValue("z", quaternion.z);
            info.AddValue("w", quaternion.w);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var quaternion = (Quaternion)obj;

            quaternion.x = info.GetSingle("x");
            quaternion.y = info.GetSingle("y");
            quaternion.z = info.GetSingle("z");
            quaternion.w = info.GetSingle("w");

            return quaternion;
        }
    }

    class sVector4 : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var vector = (Vector4)obj;

            info.AddValue("x", vector.x);
            info.AddValue("y", vector.y);
            info.AddValue("z", vector.z);
            info.AddValue("w", vector.w);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var vector = (Vector4)obj;

            vector.x = info.GetSingle("x");
            vector.y = info.GetSingle("y");
            vector.z = info.GetSingle("z");
            vector.w = info.GetSingle("w");

            return vector;
        }
    }

    class sVector3 : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var vector = (Vector3)obj;

            info.AddValue("x", vector.x);
            info.AddValue("y", vector.y);
            info.AddValue("z", vector.z);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var vector = (Vector3)obj;

            vector.x = info.GetSingle("x");
            vector.y = info.GetSingle("y");
            vector.z = info.GetSingle("z");

            return vector;
        }
    }

    class sVector2 : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var vector = (Vector2)obj;

            info.AddValue("x", vector.x);
            info.AddValue("y", vector.y);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var vector = (Vector2)obj;

            vector.x = info.GetSingle("x");
            vector.y = info.GetSingle("y");

            return vector;
        }
    }
}