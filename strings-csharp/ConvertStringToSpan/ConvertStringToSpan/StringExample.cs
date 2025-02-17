﻿using BenchmarkDotNet.Attributes;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Order;

namespace ConvertStringToSpan
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class StringExample
    {
        private string _myString = "Hello, World!";

        [Benchmark]
        public Span<char> ConvertStringToSpanUsingMemoryMarshal()
        {
            var charArray = _myString.ToCharArray();
            var memory = new Memory<char>(charArray);
            var span = MemoryMarshal.Cast<char, char>(memory.Span);

            return span;
        }

        [Benchmark]
        public unsafe Span<char> ConvertStringToSpanUsingUnsafe()
        {
            unsafe
            {
                fixed (char* ptr = _myString)
                {
                    return new Span<char>(ptr, _myString.Length);
                }
            }
        }

        [Benchmark]
        public ReadOnlySpan<char> ConvertStringToReadOnlySpanUsingAsSpan()
        {
            var span = _myString.AsSpan();

            return span;
        }

        [Benchmark]
        public Span<char> ConvertStringToSpanUsingAsSpan()
        {
            var span = _myString.AsSpan();

            return span.ToArray();
        }

        [Benchmark]
        public ReadOnlySpan<char> ConvertStringToSpan()
        {
            var span = _myString;

            return span;
        }
    }
}
