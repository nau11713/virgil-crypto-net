﻿#region Copyright (C) Virgil Security Inc.
// Copyright (C) 2015-2016 Virgil Security Inc.
// 
// Lead Maintainer: Virgil Security Inc. <support@virgilsecurity.com>
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions 
// are met:
// 
//   (1) Redistributions of source code must retain the above copyright
//   notice, this list of conditions and the following disclaimer.
//   
//   (2) Redistributions in binary form must reproduce the above copyright
//   notice, this list of conditions and the following disclaimer in
//   the documentation and/or other materials provided with the
//   distribution.
//   
//   (3) Neither the name of the copyright holder nor the names of its
//   contributors may be used to endorse or promote products derived 
//   from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ''AS IS'' AND ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT,
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.
#endregion

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;

namespace Virgil.SDK.Cryptography.ASN1.Models
{
	public class PrivateKey:Asn1Encodable
	{
		public byte[] Key { get; private set; }

		public PrivateKey(byte[] key)
		{
			Key = key;
		}

		public override Asn1Object ToAsn1Object()
		{
			return new PrivateKeyInfo(new AlgorithmIdentifier(VirgilIdentifier.Ed25519), new DerOctetString(Key)).ToAsn1Object();
		}

		public PrivateKey (Asn1Encodable asn1)
		{
			var k = PrivateKeyInfo.GetInstance(asn1);
			if(k.PrivateKeyAlgorithm.Algorithm.Id!= VirgilIdentifier.Ed25519.Id)
				throw new Asn1Exception("Unsupported algorithm");
			var key = ((DerOctetString) k.ParsePrivateKey()).GetOctets();
			if (key.Length != 32)
				throw new Asn1Exception("Key lenght should be 32 bytes");
			Key = ((DerOctetString)k.ParsePrivateKey()).GetOctets();
		}

        public static PrivateKey GetInstance(object obj)
        {
            if (obj == null)
                return (PrivateKey)null;

            if (obj is byte[])
            {
                return new PrivateKey(Asn1Object.FromByteArray(Pem.Unwrap((byte[])obj)));
            }
            return obj as PrivateKey ?? new PrivateKey(Asn1Sequence.GetInstance(obj));
        }
    }
}