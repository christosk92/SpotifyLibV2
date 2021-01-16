﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Org.BouncyCastle.Utilities;

namespace SpotifyLibV2.Helpers
{
    public class Base62Test
    {
        private const int STANDARD_BASE = 256;
        private const int TARGET_BASE = 62;
        private readonly byte[] alphabet;
        private byte[] lookup;
        public static Base62Test CreateInstanceWithInvertedCharacterSet()
        {
            return new Base62Test(CharacterSets.INVERTED);
        }
        public Base62Test(byte[] alphabet)
        {
            this.alphabet = alphabet;
            CreateLookupTable();
        }
        private void CreateLookupTable()
        {
            lookup = new byte[256];
            for (int i = 0; i < alphabet.Length; i++)
                lookup[alphabet[i]] = (byte)(i & 0xFF);
        }
        public byte[] Encode(byte[] message, int length)
        {
            byte[] indices = Convert(message, STANDARD_BASE, TARGET_BASE, length);
            return Translate(indices, alphabet);
        }
        private byte[] Translate(byte[] indices, byte[] dictionary)
        {
            byte[] translation = new byte[indices.Length];
            for (int i = 0; i < indices.Length; i++)
                translation[i] = dictionary[indices[i]];

            return translation;
        }
        public byte[] Encode(byte[] message)
        {
            return Encode(message, -1);
        }
        private int estimateOutputLength(int inputLength, int sourceBase, int targetBase)
        {
            return (int)Math.Ceiling((Math.Log(sourceBase) / Math.Log(targetBase)) * inputLength);
        }

        private byte[] Convert(byte[] message, int sourceBase, int targetBase, int length)
        {
            // This algorithm is inspired by: http://codegolf.stackexchange.com/a/21672

            int estimatedLength = length == -1 ? estimateOutputLength(message.Length, sourceBase, targetBase) : length;
            using var @out = new MemoryStream(estimatedLength);
            byte[] source = message;
            while (source.Length > 0)
            {
                using var quotient = new MemoryStream(source.Length);
                int remainder = 0;
                foreach (var b in source)
                {
                    int accumulator = (b & 0xFF) + remainder * sourceBase;
                    int digit = (accumulator - (accumulator % targetBase)) / targetBase;
                    remainder = accumulator % targetBase;
                    if (quotient.Length > 0 || digit > 0)
                        quotient.WriteByte((byte)digit);
                }

                @out.WriteByte((byte)remainder);
                source = quotient.ToArray();
            }

            if (@out.Length < estimatedLength)
            {
                var size = @out.Length;
                for (int i = 0; i < estimatedLength - size; i++)
                    @out.WriteByte((byte)0);

                return Reverse(@out.ToArray());
            }
            else if (@out.Length > estimatedLength)
            {
                return Reverse(Arrays.CopyOfRange(@out.ToArray(), 0, estimatedLength));
            }
            else
            {
                return Reverse(@out.ToArray());
            }
        }
        private byte[] Reverse(byte[] arr)
        {
            int length = arr.Length;
            byte[] reversed = new byte[length];
            for (int i = 0; i < length; i++)
                reversed[length - i - 1] = arr[i];

            return reversed;
        }

        public static class CharacterSets
        {
            public static byte[] GMP = {
                (byte) '0', (byte) '1', (byte) '2', (byte) '3', (byte) '4', (byte) '5', (byte) '6', (byte) '7',
                (byte) '8', (byte) '9', (byte) 'A', (byte) 'B', (byte) 'C', (byte) 'D', (byte) 'E', (byte) 'F',
                (byte) 'G', (byte) 'H', (byte) 'I', (byte) 'J', (byte) 'K', (byte) 'L', (byte) 'M', (byte) 'N',
                (byte) 'O', (byte) 'P', (byte) 'Q', (byte) 'R', (byte) 'S', (byte) 'T', (byte) 'U', (byte) 'V',
                (byte) 'W', (byte) 'X', (byte) 'Y', (byte) 'Z', (byte) 'a', (byte) 'b', (byte) 'c', (byte) 'd',
                (byte) 'e', (byte) 'f', (byte) 'g', (byte) 'h', (byte) 'i', (byte) 'j', (byte) 'k', (byte) 'l',
                (byte) 'm', (byte) 'n', (byte) 'o', (byte) 'p', (byte) 'q', (byte) 'r', (byte) 's', (byte) 't',
                (byte) 'u', (byte) 'v', (byte) 'w', (byte) 'x', (byte) 'y', (byte) 'z'
            };
            public static byte[] INVERTED = {
                (byte) '0', (byte) '1', (byte) '2', (byte) '3', (byte) '4', (byte) '5', (byte) '6', (byte) '7',
                (byte) '8', (byte) '9', (byte) 'a', (byte) 'b', (byte) 'c', (byte) 'd', (byte) 'e', (byte) 'f',
                (byte) 'g', (byte) 'h', (byte) 'i', (byte) 'j', (byte) 'k', (byte) 'l', (byte) 'm', (byte) 'n',
                (byte) 'o', (byte) 'p', (byte) 'q', (byte) 'r', (byte) 's', (byte) 't', (byte) 'u', (byte) 'v',
                (byte) 'w', (byte) 'x', (byte) 'y', (byte) 'z', (byte) 'A', (byte) 'B', (byte) 'C', (byte) 'D',
                (byte) 'E', (byte) 'F', (byte) 'G', (byte) 'H', (byte) 'I', (byte) 'J', (byte) 'K', (byte) 'L',
                (byte) 'M', (byte) 'N', (byte) 'O', (byte) 'P', (byte) 'Q', (byte) 'R', (byte) 'S', (byte) 'T',
                (byte) 'U', (byte) 'V', (byte) 'W', (byte) 'X', (byte) 'Y', (byte) 'Z'
            };
        }
    }
}