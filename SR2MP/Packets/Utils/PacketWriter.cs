using System.Text;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace SR2MP.Packets.Utils;

public sealed class PacketWriter : IDisposable
{
    private readonly MemoryStream _stream;
    private readonly BinaryWriter _writer;

    private byte _currentPackingByte;
    private int _currentBitIndex;

    public PacketWriter()
    {
        _stream = new MemoryStream();
        _writer = new BinaryWriter(_stream, Encoding.UTF8);
    }

    // ALWAYS call this before starting a sequence of bools! Otherwise the data becomes ineligible or out of order!
    public void ResetPackingBools()
    {
        _currentPackingByte = 0;
        _currentBitIndex = 0;
    }

    public void WritePackedBool(bool value)
    {
        if (value)
            _currentPackingByte |= (byte)(1 << _currentBitIndex);

        _currentBitIndex++;

        if (_currentBitIndex < 8)
            return;

        _writer.Write(_currentPackingByte);
        ResetPackingBools();
    }

    // Call this immediately after you finish writing your last bool! It ensures that ALL of the data is sent!
    public void EndPackingBools()
    {
        if (_currentBitIndex > 0)
            _writer.Write(_currentPackingByte);

        ResetPackingBools();
    }

    public void WriteByte(byte value) => _writer.Write(value);

    public void WriteInt(int value) => _writer.Write(value);

    public void WriteLong(long value) => _writer.Write(value);

    public void WriteFloat(float value) => _writer.Write(value);

    public void WriteString(string value, int maxChars = 20, bool truncate = false)
    {
        value ??= string.Empty; // Null safety

        if (value.Length > maxChars)
        {
            if (truncate)
                value = value[..maxChars];
            else
                throw new IOException("String too long!");
        }

        _writer.Write(value);
    }

    public void WriteBool(bool value) => _writer.Write(value);

    public void WritePacket<T>(in T value) where T : IPacket
    {
        _writer.Write((byte)value.Type);
        value.SerialiseTo(this);
    }

    public void WriteNetObject<T>(in T value) where T : INetObject => value.SerialiseTo(this);

    public void WriteVector3(Vector3 value)
    {
        WriteFloat(value.x);
        WriteFloat(value.y);
        WriteFloat(value.z);
    }

    public void WriteQuaternion(Quaternion value)
    {
        WriteFloat(value.x);
        WriteFloat(value.y);
        WriteFloat(value.z);
        WriteFloat(value.w);
    }

    public void WritePackedULong(ulong value)
    {
        while (value > 0x7F)
        {
            _writer.Write((byte)((value & 0x7F) | 0x80));
            value >>= 7;
        }

        _writer.Write((byte)value);
    }

    public void WritePackedUInt(uint value)
    {
        while (value > 0x7F)
        {
            _writer.Write((byte)((value & 0x7F) | 0x80));
            value >>= 7;
        }

        _writer.Write((byte)value);
    }

    public void WritePackedLong(long value) => WritePackedULong((ulong)value);

    public void WritePackedInt(int value) => WritePackedUInt((uint)value);

    public void WriteEnum<T>(T value) where T : struct, Enum
    {
        var size = Unsafe.SizeOf<T>();

        switch (size)
        {
            case 1:
            {
                _writer.Write(Unsafe.As<T, byte>(ref value));
                break;
            }
            case 2:
            {
                _writer.Write(Unsafe.As<T, ushort>(ref value));
                break;
            }
            case 4:
            {
                WritePackedUInt(Unsafe.As<T, uint>(ref value));
                break;
            }
            case 8:
            {
                WritePackedULong(Unsafe.As<T, ulong>(ref value));
                break;
            }
            default:
                throw new ArgumentException($"Enum size {size} not supported");
        }
    }

    public void WriteArray<T>(T[] array, Action<PacketWriter, T> writer)
    {
        WritePackedInt(array.Length);

        for (var i = 0; i < array.Length; i++)
            writer(this, array[i]);
    }

    public void WriteList<T>(List<T> list, Action<PacketWriter, T> writer)
    {
        WritePackedInt(list.Count);

        for (var i = 0; i < list.Count; i++)
            writer(this, list[i]);
    }

    public void WriteDictionary<TKey, TValue>(Dictionary<TKey, TValue> dict, Action<PacketWriter, TKey> keyWriter, Action<PacketWriter, TValue> valueWriter) where TKey : notnull
    {
        WritePackedInt(dict.Count);

        foreach (var (key, value) in dict)
        {
            keyWriter(this, key);
            valueWriter(this, value);
        }
    }

    public byte[] ToArray() => _stream.ToArray();

    public void Dispose()
    {
        _writer?.Dispose();
        _stream?.Dispose();
        GC.SuppressFinalize(this);
    }
}