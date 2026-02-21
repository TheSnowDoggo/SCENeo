using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Unicode;

namespace SCENeo.Serialization;

public static class StreamExtensions
{
	public static void WriteBool(this Stream stream, bool value)
	{
		stream.WriteByte(Unsafe.BitCast<bool, byte>(value));
	}
	
	public static void WriteChar(this Stream stream, char value)
	{
		Span<byte> span = stackalloc byte[sizeof(char)];
		Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(span), value);
		stream.Write(span);
	}
	
	public static void WriteInt16(this Stream stream, short value)
	{
		Span<byte> span = stackalloc byte[sizeof(short)];
		Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(span), value);
		stream.Write(span);
	}
	
	public static void WriteUInt16(this Stream stream, ushort value)
	{
		Span<byte> span = stackalloc byte[sizeof(ushort)];
		Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(span), value);
		stream.Write(span);
	}
	
	public static void WriteInt32(this Stream stream, int value)
	{
		Span<byte> span = stackalloc byte[sizeof(int)];
		Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(span), value);
		stream.Write(span);
	}
	
	public static void WriteUInt32(this Stream stream, uint value)
	{
		Span<byte> span = stackalloc byte[sizeof(uint)];
		Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(span), value);
		stream.Write(span);
	}
	
	public static void WriteInt64(this Stream stream, long value)
	{
		Span<byte> span = stackalloc byte[sizeof(long)];
		Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(span), value);
		stream.Write(span);
	}
	
	public static void WriteUInt64(this Stream stream, ulong value)
	{
		Span<byte> span = stackalloc byte[sizeof(ulong)];
		Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(span), value);
		stream.Write(span);
	}
	
	public static void WriteSingle(this Stream stream, float value)
	{
		Span<byte> span = stackalloc byte[sizeof(float)];
		Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(span), value);
		stream.Write(span);
	}

	public static void WriteDouble(this Stream stream, double value)
	{
		Span<byte> span = stackalloc byte[sizeof(double)];
		Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(span), value);
		stream.Write(span);
	}

	public static int WriteUtf8(this Stream stream, string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			stream.WriteInt32(0);
			stream.WriteInt32(0);
			return 2 * sizeof(int);
		}
		
		byte[] buffer = ArrayPool<byte>.Shared.Rent(value.Length * 3);

		try
		{
			Utf8.FromUtf16(value.AsSpan(), buffer, out _, out int written);

			stream.WriteInt32(written);
			stream.WriteInt32(value.Length);
			stream.Write(buffer, 0, written);

			return 2 * sizeof(int) + written;
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(buffer);
		}
	}

	public static void WriteArray<T>(this Stream stream, T[] array, Action<Stream, T> serializer)
	{
		stream.WriteInt32(array.Length);

		for (int i = 0; i < array.Length; i++)
		{
			serializer(stream, array[i]);
		}
	}
	
	public static void WriteList<T>(this Stream stream, IReadOnlyList<T> list, Action<Stream, T> serializer)
	{
		int count = list.Count;
		stream.WriteInt32(count);

		for (int i = 0; i < count; i++)
		{
			serializer(stream, list[i]);
		}
	}

	public static byte ReadExactByte(this Stream stream)
	{
		int value = stream.ReadByte();

		if (value < 0)
		{
			throw new EndOfStreamException("Cannot read byte, reached end of stream.");
		}

		return (byte)value;
	}

	public static byte PeakExactByte(this Stream stream)
	{
		long position = stream.Position;
		int value = stream.ReadExactByte();
		stream.Seek(position, SeekOrigin.Begin);
		return (byte)value;
	}
	
	public static bool ReadBool(this Stream stream)
	{
		return Unsafe.BitCast<byte, bool>(stream.ReadExactByte());
	}
	
	public static char ReadChar(this Stream stream)
	{
		Span<byte> span = stackalloc byte[sizeof(char)];
		stream.ReadExactly(span);
		return Unsafe.ReadUnaligned<char>(ref MemoryMarshal.GetReference(span));
	}
	
	public static short ReadInt16(this Stream stream)
	{
		Span<byte> span = stackalloc byte[sizeof(short)];
		stream.ReadExactly(span);
		return Unsafe.ReadUnaligned<short>(ref MemoryMarshal.GetReference(span));
	}
	
	public static ushort ReadUInt16(this Stream stream)
	{
		Span<byte> span = stackalloc byte[sizeof(ushort)];
		stream.ReadExactly(span);
		return Unsafe.ReadUnaligned<ushort>(ref MemoryMarshal.GetReference(span));
	}
	
	public static int ReadInt32(this Stream stream)
	{
		Span<byte> span = stackalloc byte[sizeof(int)];
		stream.ReadExactly(span);
		return Unsafe.ReadUnaligned<int>(ref MemoryMarshal.GetReference(span));
	}
	
	public static uint ReadUInt32(this Stream stream)
	{
		Span<byte> span = stackalloc byte[sizeof(uint)];
		stream.ReadExactly(span);
		return Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(span));
	}
	
	public static long ReadInt64(this Stream stream)
	{
		Span<byte> span = stackalloc byte[sizeof(long)];
		stream.ReadExactly(span);
		return Unsafe.ReadUnaligned<long>(ref MemoryMarshal.GetReference(span));
	}
	
	public static ulong ReadUInt64(this Stream stream)
	{
		Span<byte> span = stackalloc byte[sizeof(ulong)];
		stream.ReadExactly(span);
		return Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(span));
	}
	
	public static float ReadFloat(this Stream stream)
	{
		Span<byte> span = stackalloc byte[sizeof(float)];
		stream.ReadExactly(span);
		return Unsafe.ReadUnaligned<float>(ref MemoryMarshal.GetReference(span));
	}
	
	public static double ReadDouble(this Stream stream)
	{
		Span<byte> span = stackalloc byte[sizeof(double)];
		stream.ReadExactly(span);
		return Unsafe.ReadUnaligned<double>(ref MemoryMarshal.GetReference(span));
	}

	public static string ReadUtf8(this Stream stream)
	{
		int utf8Bytes = stream.ReadInt32();
		int utf16Bytes = stream.ReadInt32();

		if (utf8Bytes == 0)
		{
			return string.Empty;
		}
		
		byte[] buffer = ArrayPool<byte>.Shared.Rent(utf8Bytes);

		try
		{
			stream.ReadExactly(buffer);

			return string.Create(utf16Bytes, (buffer, utf8Bytes), (span, state) =>
			{
				Utf8.ToUtf16(state.buffer, span, out _, out _, replaceInvalidSequences: false);
			});
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(buffer);
		}
	}

	public static T[] ReadArray<T>(this Stream stream, Func<Stream, T> deserializer)
	{
		int count = stream.ReadInt32();

		if (count < 0)
		{
			throw new InvalidDataException($"Array count {count} was negative.");
		}

		T[] array = new T[count];

		for (int i = 0; i < count; i++)
		{
			array[i] = deserializer(stream);
		}

		return array;
	}
}