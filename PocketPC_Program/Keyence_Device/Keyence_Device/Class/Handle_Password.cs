using System;
using System.Text;

namespace Keyence_Device
{
    class Handle_Password
    {
        private const int Iterations = 10000; // phải trùng Web

        public static bool Verify(string password, string stored)
        {
            // Bảo vệ input để tránh lỗi format
            if (string.IsNullOrEmpty(stored)) return false;

            string[] parts = stored.Split(':');
            if (parts.Length != 3) return false;
            if (!string.Equals(parts[0], "v1", StringComparison.Ordinal)) return false;

            byte[] salt;
            byte[] expected;
            try
            {
                salt = Convert.FromBase64String(parts[1]);
                expected = Convert.FromBase64String(parts[2]);
            }
            catch
            {
                return false;
            }

            // Dẫn xuất đúng số byte như Web đã tạo (expected.Length)
            byte[] actual = PBKDF2_HMACSHA1(password, salt, Iterations, expected.Length);

            // So sánh constant-time
            return SlowEquals(actual, expected);
        }

        /// <summary>
        /// PBKDF2-HMAC-SHA1 thuần C# (RFC 2898)
        /// </summary>
        private static byte[] PBKDF2_HMACSHA1(string password, byte[] salt, int iterations, int dkLen)
        {
            if (salt == null) throw new ArgumentNullException("salt");
            if (iterations <= 0) throw new ArgumentOutOfRangeException("iterations");
            if (dkLen <= 0) throw new ArgumentOutOfRangeException("dkLen");

            byte[] pwdBytes = Encoding.UTF8.GetBytes(password);
            const int HashLen = 20; // SHA1 = 20 bytes
            int l = (int)Math.Ceiling((double)dkLen / HashLen);
            int r = dkLen - (l - 1) * HashLen;

            byte[] derived = new byte[dkLen];
            byte[] block = new byte[HashLen];
            int offset = 0;

            for (int i = 1; i <= l; i++)
            {
                F(pwdBytes, salt, iterations, i, block); // T_i
                int toCopy = (i == l) ? r : HashLen;
                Buffer.BlockCopy(block, 0, derived, offset, toCopy);
                offset += toCopy;
            }

            Array.Clear(pwdBytes, 0, pwdBytes.Length);
            Array.Clear(block, 0, block.Length);
            return derived;
        }

        /// <summary>
        /// F theo RFC 2898: T_i = U_1 XOR ... XOR U_c
        /// U_1 = HMAC(key, salt || INT(i)); U_j = HMAC(key, U_{j-1})
        /// </summary>
        private static void F(byte[] key, byte[] salt, int iterations, int blockIndex, byte[] output)
        {
            byte[] ctr = IntToBe(blockIndex);

            byte[] msg = new byte[salt.Length + 4];
            Buffer.BlockCopy(salt, 0, msg, 0, salt.Length);
            Buffer.BlockCopy(ctr, 0, msg, salt.Length, 4);

            byte[] u = HmacSha1(key, msg);
            Buffer.BlockCopy(u, 0, output, 0, u.Length);

            for (int i = 2; i <= iterations; i++)
            {
                u = HmacSha1(key, u);
                for (int j = 0; j < output.Length; j++)
                    output[j] ^= u[j];
            }

            Array.Clear(ctr, 0, ctr.Length);
            Array.Clear(msg, 0, msg.Length);
            Array.Clear(u, 0, u.Length);
        }

        /// <summary>
        /// HMAC-SHA1 thuần C#
        /// H(K,m) = SHA1((opad ^ K') || SHA1((ipad ^ K') || m)), blocksize = 64
        /// </summary>
        private static byte[] HmacSha1(byte[] key, byte[] message)
        {
            const int BlockSize = 64;
            byte[] k0;

            if (key.Length > BlockSize)
            {
                k0 = Sha1.ComputeHash(key); // rút gọn
                if (k0.Length < BlockSize)
                {
                    byte[] tmp = new byte[BlockSize];
                    Buffer.BlockCopy(k0, 0, tmp, 0, k0.Length);
                    k0 = tmp;
                }
            }
            else
            {
                k0 = new byte[BlockSize];
                Buffer.BlockCopy(key, 0, k0, 0, key.Length);
            }

            byte[] ipad = new byte[BlockSize];
            byte[] opad = new byte[BlockSize];
            for (int i = 0; i < BlockSize; i++)
            {
                byte b = k0[i];
                ipad[i] = (byte)(b ^ 0x36);
                opad[i] = (byte)(b ^ 0x5C);
            }

            // inner = SHA1(ipad || message)
            byte[] inner = new byte[BlockSize + message.Length];
            Buffer.BlockCopy(ipad, 0, inner, 0, BlockSize);
            Buffer.BlockCopy(message, 0, inner, BlockSize, message.Length);
            byte[] innerHash = Sha1.ComputeHash(inner);

            // outer = SHA1(opad || innerHash)
            byte[] outer = new byte[BlockSize + innerHash.Length];
            Buffer.BlockCopy(opad, 0, outer, 0, BlockSize);
            Buffer.BlockCopy(innerHash, 0, outer, BlockSize, innerHash.Length);
            byte[] result = Sha1.ComputeHash(outer);

            Array.Clear(k0, 0, k0.Length);
            Array.Clear(ipad, 0, ipad.Length);
            Array.Clear(opad, 0, opad.Length);
            Array.Clear(inner, 0, inner.Length);
            Array.Clear(outer, 0, outer.Length);
            Array.Clear(innerHash, 0, innerHash.Length);
            return result;
        }

        private static byte[] IntToBe(int i)
        {
            return new byte[]
            {
                (byte)((i >> 24) & 0xFF),
                (byte)((i >> 16) & 0xFF),
                (byte)((i >> 8)  & 0xFF),
                (byte)( i        & 0xFF)
            };
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++) diff |= (a[i] ^ b[i]);
            return diff == 0;
        }

        /// <summary>
        /// SHA-1 thuần C# (FIPS PUB 180-1/180-2), trả về 20 byte
        /// </summary>
        private static class Sha1
        {
            public static byte[] ComputeHash(byte[] data)
            {
                // Khởi tạo h0..h4
                uint h0 = 0x67452301;
                uint h1 = 0xEFCDAB89;
                uint h2 = 0x98BADCFE;
                uint h3 = 0x10325476;
                uint h4 = 0xC3D2E1F0;

                // Padding: data || 0x80 || 0x00... || length(64-bit big-endian)
                ulong bitLen = (ulong)data.Length * 8UL;

                // chiều dài sau padding là bội số của 64
                int padLen = 64 - (int)((data.Length + 8 + 1) % 64);
                if (padLen == 64) padLen = 0;

                byte[] msg = new byte[data.Length + 1 + padLen + 8];
                Buffer.BlockCopy(data, 0, msg, 0, data.Length);
                msg[data.Length] = 0x80;
                // 8 byte cuối là độ dài bit big-endian
                for (int i = 0; i < 8; i++)
                {
                    msg[msg.Length - 1 - i] = (byte)((bitLen >> (8 * i)) & 0xFF);
                }

                uint[] w = new uint[80];

                for (int chunk = 0; chunk < msg.Length; chunk += 64)
                {
                    // chuẩn bị w[0..79]
                    for (int i = 0; i < 16; i++)
                    {
                        int o = chunk + (i * 4);
                        w[i] = ((uint)msg[o] << 24) |
                               ((uint)msg[o + 1] << 16) |
                               ((uint)msg[o + 2] << 8) |
                               (uint)msg[o + 3];
                    }
                    for (int i = 16; i < 80; i++)
                    {
                        w[i] = Rol1(w[i - 3] ^ w[i - 8] ^ w[i - 14] ^ w[i - 16]);
                    }

                    uint a = h0, b = h1, c = h2, d = h3, e = h4;

                    for (int i = 0; i < 80; i++)
                    {
                        uint f, k;
                        if (i < 20)
                        {
                            f = (b & c) | ((~b) & d);
                            k = 0x5A827999;
                        }
                        else if (i < 40)
                        {
                            f = b ^ c ^ d;
                            k = 0x6ED9EBA1;
                        }
                        else if (i < 60)
                        {
                            f = (b & c) | (b & d) | (c & d);
                            k = 0x8F1BBCDC;
                        }
                        else
                        {
                            f = b ^ c ^ d;
                            k = 0xCA62C1D6;
                        }

                        uint temp = Rol5(a) + f + e + k + w[i];
                        e = d;
                        d = c;
                        c = Rol30(b);
                        b = a;
                        a = temp;
                    }

                    h0 += a; h1 += b; h2 += c; h3 += d; h4 += e;
                }

                byte[] digest = new byte[20];
                WriteBe(digest, 0, h0);
                WriteBe(digest, 4, h1);
                WriteBe(digest, 8, h2);
                WriteBe(digest, 12, h3);
                WriteBe(digest, 16, h4);
                Array.Clear(w, 0, w.Length);
                Array.Clear(msg, 0, msg.Length);
                return digest;
            }

            private static uint Rol1(uint x) { return (x << 1) | (x >> 31); }
            private static uint Rol5(uint x) { return (x << 5) | (x >> 27); }
            private static uint Rol30(uint x) { return (x << 30) | (x >> 2); }

            private static void WriteBe(byte[] buf, int ofs, uint val)
            {
                buf[ofs] = (byte)((val >> 24) & 0xFF);
                buf[ofs + 1] = (byte)((val >> 16) & 0xFF);
                buf[ofs + 2] = (byte)((val >> 8) & 0xFF);
                buf[ofs + 3] = (byte)(val & 0xFF);
            }
        }
    }
}