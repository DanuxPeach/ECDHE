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

using namespace CryptoPP;
using namespace std;

void LoadPrivateKey(const std::string& filename, Integer& privateKey);
void LoadPublicKey(const string& filename, ECP::Point& key);
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
    
    Integer PrivateKeyB;
    ECP::Point PublicKeyB;
    ECP::Point PublicKeyA;

    cout << "Input your file publickey: " << endl;
    cin >> filesource;
    LoadPublicKey(filesource, PublicKeyB);

    cout << "Input your file privatekey: " << endl;
    cin >> filesource;
    LoadPrivateKey(filesource, PrivateKeyB);

    cout << "Input your friend publickey: " << endl;
    cin >> filesource;
    LoadPublicKey(filesource, PublicKeyA);

    cout << "PublicKeyA.x = " << PublicKeyA.x << endl;
    cout << "PublicKeyA.y = " << PublicKeyA.y << endl;


    //Create ShareKey ECDH
    //ShareKey A side
    ECP::Point ShareKeyB = curve384.GetCurve().Multiply(PrivateKeyB, PublicKeyA);
    cout << "ShareKeyx = " << ShareKeyA.x << endl;
    cout << "ShareKeyy = " << ShareKeyA.y << endl;

    return 0;
}

void LoadPrivateKey(const std::string& filename, Integer& privateKey)
{
    privateKey = Integer::Zero();

    // Load private key from file
    FileSource fileSource(filename.c_str(), true);

    ByteQueue privateKeyBytes;
    fileSource.CopyTo(privateKeyBytes);
    privateKeyBytes.MessageEnd();
    privateKey.BERDecode(privateKeyBytes);


    return;
}

void LoadPublicKey(const string& filename, ECP::Point& key)
{
    // Open file and get the size
    FileSource file(filename.c_str(), true /*pumpAll*/);
    const size_t fileSize = file.MaxRetrievable();

    // Check the file size
    if (fileSize % 2 != 0) {
        cout << "Invalid file size" << endl;
        return;
    }

    // Read x and y from file
    const size_t halfSize = fileSize / 2;
    CryptoPP::SecByteBlock xEncoded(halfSize), yEncoded(halfSize);
    file.Get(xEncoded, halfSize);
    file.Get(yEncoded, halfSize);

    // Convert xEncoded and yEncoded to SecByteBlock
    CryptoPP::SecByteBlock xSecBlock = ConvertToSecByteBlock(xEncoded.data(), xEncoded.size());
    CryptoPP::SecByteBlock ySecBlock = ConvertToSecByteBlock(yEncoded.data(), yEncoded.size());

    // Decode x and y
    CryptoPP::Integer x, y;
    x.Decode(xSecBlock.BytePtr(), xSecBlock.SizeInBytes());
    y.Decode(ySecBlock.BytePtr(), ySecBlock.SizeInBytes());

    // Create public key from x and y
    key = CryptoPP::ECP::Point(x, y);
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