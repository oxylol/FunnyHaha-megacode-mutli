using UnityEngine;
using System.Text;
using System.Runtime.CompilerServices;

namespace SR2MP.Packets.Utils;

public sealed class PacketReader : IDisposable
{
    private readonly MemoryStream _stream;
    private readonly BinaryReader _reader;

    private byte _currentPackedByte;
    private int _currentBitIndex = 8;

    public PacketReader(byte[] data)
    {
        _stream = new MemoryStream(data);
        _reader = new BinaryReader(_stream, Encoding.UTF8);
    }

    public byte ReadByte() => _reader.ReadByte();

    public int ReadInt() => _reader.ReadInt32();

    public double ReadDouble() => _reader.ReadDouble();

    public long ReadLong() => _reader.ReadInt64();

    public float ReadFloat() => _reader.ReadSingle();

    public string ReadString() => _reader.ReadString();

    public bool ReadBool() => _reader.ReadBoolean();

    public Vector3 ReadVector3() => new Vector3(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());

    public Quaternion ReadQuaternion() => new Quaternion(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());

    // Make sure that you are reading your packed bools in order!
    public bool ReadPackedBool()
    {
        if (_currentBitIndex >= 8)
        {
            _currentPackedByte = _reader.ReadByte();
            _currentBitIndex = 0;
        }

        var value = (_currentPackedByte & (1 << _currentBitIndex)) != 0;

        _currentBitIndex++;
        return value;
    }

    // Always call this if you switch from reading bools to reading other types!
    public void EndPackingBools() => _currentBitIndex = 8;

    public ulong ReadPackedULong()
    {
        var shift = 0;

        ulong value = 0;
        byte b;

        do
        {
            if (shift >= 70)
                throw new FormatException("VarInt too long.");

            b = _reader.ReadByte();

            value |= (ulong)(b & 0x7F) << shift;
            shift += 7;
        }
        while ((b & 0x80) != 0);

        return value;
    }

    public long ReadVarLong() => (long)ReadPackedULong();

    public uint ReadPackedUInt()
    {
        var shift = 0;

        uint value = 0;
        byte b;

        do
        {
            if (shift >= 35)
                throw new FormatException("VarInt too long.");

            b = _reader.ReadByte();

            value |= (uint)(b & 0x7F) << shift;
            shift += 7;
        }
        while ((b & 0x80) != 0);

        return value;
    }

    public int ReadPackedInt() => (int)ReadPackedUInt();

    public T[] ReadArray<T>(Func<PacketReader, T> reader)
    {
        var array = new T[ReadPackedInt()];

        for (var i = 0; i < array.Length; i++)
            array[i] = reader(this);

        return array;
    }

    public List<T> ReadList<T>(Func<PacketReader, T> reader)
    {
        var count = ReadPackedInt();
        var list = new List<T>(count);

        while (count-- > 0)
            list.Add(reader(this));

        return list;
    }

    public Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>(Func<PacketReader, TKey> keyReader, Func<PacketReader, TValue> valueReader) where TKey : notnull
    {
        var count = ReadPackedInt();
        var dict = new Dictionary<TKey, TValue>(count);

        while (count-- > 0)
            dict[keyReader(this)] = valueReader(this);

        return dict;
    }

    // All net objects MUST have a parameter-less constructor! Either make the type a struct (which always has a parameterless constructor), or at least declare a parameterless constructor for classes!
    public T ReadNetObject<T>() where T : INetObject, new()
    {
        var result = new T();
        result.DeserialiseFrom(this);
        return result;
    }

    public T ReadEnum<T>() where T : struct, Enum
    {
        var size = Unsafe.SizeOf<T>();

        switch (size)
        {
            case 1:
            {
                var b = _reader.ReadByte();
                return Unsafe.As<byte, T>(ref b);
            }
            case 2:
            {
                var s = _reader.ReadUInt16();
                return Unsafe.As<ushort, T>(ref s);
            }
            case 4:
            {
                var i = ReadPackedUInt();
                return Unsafe.As<uint, T>(ref i);
            }
            case 8:
            {
                var l = ReadPackedULong();
                return Unsafe.As<ulong, T>(ref l);
            }
            default:
                throw new NotSupportedException($"Enum size {size} not supported");
        }
    }

    public void Skip(int count) => _stream.Position += count;

    public void Dispose()
    {
        _reader?.Dispose();
        _stream?.Dispose();
        GC.SuppressFinalize(this);
    }
}