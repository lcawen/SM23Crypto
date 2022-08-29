# sm-crypto
国密算法sm2、sm3移植自https://github.com/JuneAndGreen/sm-crypto
在此感谢原作者

## sm2

### 加密解密
##### 示例代码
```c#
using SMCrypto;

SM2Key sm2Key= SM2.GenerateKeyPairHex();
string encryptData = SM2.DoEncrypt(msgString,sm2Key.PubKey,SM2.CipherMode.C1C3C2);
string decryptData = SM2.DoDecrypt(encryptData,sm2Key.PriKey,SM2.CipherMode.C1C3C2).ToString();

```


## sm3
##### 示例代码
```c#
using SMCrypto;

string hashData = SM3.StrSum("abc"); // 杂凑
```

## 测试结果
```shell
------------------SM3测试START------------------                                                              
数据: abc
结果: 66C7F0F462EEEDD9D1F2D46BDC10E4E24167C4875CF2F7A2297DA02B8F4BA8E0
数据: abcdefghABCDEFGH12345678abcdefghABCDEFGH12345678abcdefghABCD
结果: B8AC4203969BDE27434CE667B0ADBF3439EE97E416E73CB96F4431F478A531FE
数据: ÁáÀàĂăẮắẰằẴẵẲẳÂâẤấẦầẪẫẨẩǍǎÅåǺǻÄäǞǟÃãȦȧǠǡĄąĀāẢảȀȁȂȃẠạẶặẬậḀḁȺⱥᶏẚɐɑᶐɒᴀÆæǼǽǢǣᴂᴁ
结果: 175C329C05AA9E2CE3AD49551D404F670BC7FD59DFA51C748871FFD1A6B179A6
数据: 今天天气真不错
结果: $FBC183F5B8DD7FBF306F870CEE21BFBDD02A4BBDAB39E8D8488EDDE4E5255D3C
丧丙上䠉䰋不亐乑
结果: $382F78A3065187C40152D2F5CA283F8F4BF148909C763CFBDFA7EFB943016552
------------------SM3测试END------------------

------------------SM2测试START------------------
生成秘钥
私钥1: 1ef4b3cec25c5cc78af8cf1901cd73461e5332a8df75cc98096b5b711c1d7d77
公钥1: 046fd2618147b0eb2f3f399ea33be8c693ebe99f0e5b39c02aed3b3caae70b708b3b6b449ab186936aa739d13bd7e2f048491b048e1da2c60d7324ff4168e1c94a
私钥2: 846a0813d8e47319a734fba04dd4716224124d9d7d6f5f1c23a9f2384d98b90c
公钥2: 04ff443c8ad4bff32847595735c5da9b8628e77575ee076b94365a14f899c239bdd0b84db8882944c0b5eb47df89ab7479009f5b4a91dd94f066fcc7b20e3ecf92
私钥3: 31d3c10e030f2724090c86c620bd6194ee48e6f185d730d58aafaf0cf7c9224f
公钥3: 0493b812c9f2199611d7c3fb2557a422a837e7345383f389e2cfb08dd4ca2a732d1c40b298555f35750165b16548629aee21d2bcdeaf0b4dd5e4753f9e5a897fe0
私钥4: 9b8ce50f18e8865be6116458258c8fe7839611fff9cd8ff6ce7f5f5a11fa288a
公钥4: 045940a0576c171343beae1fc1d548f6b8eaa2067b129694d770051bea418ffc35c755fe4b1df0ef715717ae123905d80e98254ce380342746b3af85107075edc7
私钥5: 15345ac225f87f4cb939d4ab4bcf1a6b28bb48b2e95acde196321c8803f861a9
公钥5: 043cc7b43f3077f32ba4ef51660bb91283ecd09f00532050ef946fc2d25b976d529f194df7c5bc76edc8f4d8b3c4a34ac403e15e0339f0ee765d4b693cd2ba1e97

模式: C1C3C2
私钥: 15345ac225f87f4cb939d4ab4bcf1a6b28bb48b2e95acde196321c8803f861a9
数据: 国密SM2测试
加密: ee3e5447e3f38c324e3b7350ce86c35298dcf1f0f314526199ef3a2e44b4d28c469047f86b053a2dbc2a641a5b8fe402ec1ee9ab63dd8aeb1f7f8ee0831a47fa99b8d46c3417e20dd41ac7b636773d99ae9f16e6112c886dcde47f0b1bc0ad7ef686d8d645d489a69c332cb5d48eed
解密: 国密SM2测试
------------------SM2测试END------------------
```
