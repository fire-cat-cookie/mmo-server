using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace mmo_server.Cryptography {
    public class PacketEncryption {

        /// <summary>
        /// Storing the private key here can present a security vulnerability.
        /// This is just for demo purposes.
        /// </summary>
        private string privateKey =
            "<RSAKeyValue>" +
            "<Modulus>4MEU8lNX7YqPKvwg9muarH3OnVbvFbQKjXFnvFMVShOnpY4LR6/Ykd2RaZaYwPoBifblngPfBc3STnGABK8fYpeYY7/8bjiiW+p5e/HyDvAkxh90fp+NLF+TsOOXFkIuYPv1PaDTIE9MfRkcvZc7UaWJerSXIFIBQfXNy1d1sGU=</Modulus>" +
            "<Exponent>AQAB</Exponent>" +
            "<P>/4sIqaE7+9obCGJatu3+nXo5Ss05kLMp/xv9g64yoaaB7I0X48yFRI/jHa9F40BVR2p/R/JSfBbDOKyaB3vJOw==</P>" +
            "<Q>4Sf0lqtgMSS/z3e8RjdPacOEhvqj4XyO5mtis0/Pa6mPilcbfJ81/2Oqlrns4YTOCOZdvrVo/LWbvPRjtb3S3w==</Q>" +
            "<DP>RD5/G74BZOsEWSLbEwtP+gJQFpfkDa6rQoPZGjeFMgJjpCayAJX36S17+8t5II1nuODKCQ6/1H/HtvTxVZpLTQ==</DP>" +
            "<DQ>D7JyaeKuxR5TxQoK9TY78hqwbZyhukGt0MCh0/B3fIEIwdNK1khfQlvXc8SokBxrJNGyrW4GUL/0iJrdLLl8Lw==</DQ>" +
            "<InverseQ>wZUABGZw5+fKSdU9suUaoA2bV7i9mLYRhx3BRn72eAnpKnR5MaCvMO6M7voqacV0uA6iO4DH37sAnoyst5HEaA==</InverseQ>" +
            "<D>uVKcSgAs9YKV8LS7HVW20VpfPt+K/tNZDC4hgmjjmhRA3UMVzAOUSRpwp6b0x9Kd6Dv8M+IvSKLhMatnCvW3otDMsUrCGClepbRMWDt+hebFcdRSbwhwESLm+rKG+uFpGhq35UMbvnav252maeaGakf9Cg984SQ09qSTTOlLdtU=</D>" +
            "</RSAKeyValue>";
        private string publicKey = 
            "<RSAKeyValue>" +
            "<Modulus>4MEU8lNX7YqPKvwg9muarH3OnVbvFbQKjXFnvFMVShOnpY4LR6/Ykd2RaZaYwPoBifblngPfBc3STnGABK8fYpeYY7/8bjiiW+p5e/HyDvAkxh90fp+NLF+TsOOXFkIuYPv1PaDTIE9MfRkcvZc7UaWJerSXIFIBQfXNy1d1sGU=</Modulus>" +
            "<Exponent>AQAB</Exponent>" +
            "</RSAKeyValue>";

        public void GenerateKeyPair() {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider()) {
                privateKey = RSA.ToXmlString(true);
                publicKey = RSA.ToXmlString(false);
                System.Diagnostics.Debug.WriteLine("generating, private key: ");
                System.Diagnostics.Debug.Write(privateKey);
                System.Diagnostics.Debug.WriteLine("");
                System.Diagnostics.Debug.WriteLine("public key: ");
                System.Diagnostics.Debug.Write(publicKey);
            }
        }

        public byte[] Decrypt(byte[] message, int startIndex) {
            byte[] toDecrypt = new byte[message.Length - startIndex];
            Array.Copy(message, startIndex, toDecrypt, 0, toDecrypt.Length);
            byte[] decrypted = Decrypt(toDecrypt);
            byte[] combined = new byte[decrypted.Length + startIndex];
            Array.Copy(message, 0, combined, 0, startIndex);
            Array.Copy(decrypted, 0, combined, startIndex, decrypted.Length);
            return combined;
        }

        private byte[] Decrypt(byte[] message) {
            return RetrievePrivateKey().Decrypt(message, true);
        }

        private RSACryptoServiceProvider RetrievePrivateKey() {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);
            return rsa;
        }
    }
}
