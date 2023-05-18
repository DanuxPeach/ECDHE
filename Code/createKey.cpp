#include <iostream>
#include <string>
#include <fstream>
#include "osrng.h"
#include "ecp.h"
#include "eccrypto.h"
#include "oids.h"
#include "hex.h"
#include "files.h"
#include "pubkey.h"
#include "secblock.h"
#include "cryptlib.h"

using namespace CryptoPP;
using namespace std;

void SavePublicKey(const string& filename, const ECP::Point& key);
void SavePrivateKey(const std::string& filename, const Integer& privateKey);
void ConvertSecByteBlockToBinary(const SecByteBlock& secBlock, CryptoPP::byte* &data, size_t& dataSize);
SecByteBlock ConvertToSecByteBlock(const CryptoPP::byte* data, size_t dataSize);

int main(int argc, char* argv[])
{
    AutoSeededRandomPool rng;
    string filesource;
    CryptoPP::OID oid = ASN1::secp384r1();
    /* Create curve */
    CryptoPP::DL_GroupParameters_EC<ECP> curve384;
    curve384.Initialize(oid);
    
    //Create random Key
    Integer privateKeyB(rng, Integer::One(), curve384.GetMaxExponent());
    Integer p = curve384.GetMaxExponent();
    cout<<"p: " << p << endl;
    cout << "PrivateKeyB = " << privateKeyB << endl;

    privateKeyB %= p;
    ECP::Point G = curve384.GetSubgroupGenerator();
    ECP::Point publicKeyB = curve384.GetCurve().ScalarMultiply(G, privateKeyB);
    cout << "PublicKeyB.x = " << PublicKeyB.x << endl;
    cout << "PublicKeyB.y = " << PublicKeyB.y << endl;
    SavePublicKey("publicKeyB.txt", publicKeyB);
    SavePrivateKey("privateKeyB.txt", privateKeyB);


    return 0;
}

void SavePublicKey(const string& filename, const ECP::Point& key)
{
    // Convert x and y to byte arrays
    const Integer x = key.x;
    const Integer y = key.y;
    const size_t xSize = x.MinEncodedSize();
    const size_t ySize = y.MinEncodedSize();
    SecByteBlock xEncoded(xSize), yEncoded(ySize);
    x.Encode(xEncoded, xSize);
    y.Encode(yEncoded, ySize);

    CryptoPP::byte* binaryDataX = nullptr;
    size_t binaryDataXSize = 0;
    ConvertSecByteBlockToBinary(xEncoded, binaryDataX, binaryDataXSize);

    CryptoPP::byte* binaryDataY = nullptr;
    size_t binaryDataYSize = 0;
    ConvertSecByteBlockToBinary(yEncoded, binaryDataY, binaryDataYSize);

    // Write x and y to file
    FileSink file(filename.c_str());
    file.Put(binaryDataX, binaryDataXSize);
    file.Put(binaryDataY, binaryDataYSize);

    std::cout << "Public key saved to file: " << filename << std::endl;

    delete[] binaryDataX;
    delete[] binaryDataY;
}

void SavePrivateKey(const std::string& filename, const Integer& privateKey)
{
    // Save private key to file
    ByteQueue privateKeyBytes;
    privateKey.DEREncode(privateKeyBytes);
    FileSink fileSink(filename.c_str());
    privateKeyBytes.CopyTo(fileSink);
    fileSink.MessageEnd();

    std::cout << "Private key saved to file: " << filename << std::endl;
}

void ConvertSecByteBlockToBinary(const SecByteBlock& secBlock, CryptoPP::byte* &data, size_t& dataSize)
{
    dataSize = secBlock.size();
    data = new CryptoPP::byte[dataSize];
    memcpy(data, secBlock.data(), dataSize);
}

SecByteBlock ConvertToSecByteBlock(const CryptoPP::byte* data, size_t dataSize)
{
    SecByteBlock secBlock(dataSize);
    memcpy(secBlock.data(), data, dataSize);
    return secBlock;
}
