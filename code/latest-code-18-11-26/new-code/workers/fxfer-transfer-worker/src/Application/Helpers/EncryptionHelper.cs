using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO;

namespace Fxfer.TransferWorker.Application.Helpers;

public static class EncryptionHelper
{
    public static string DecryptPgpData(string inputData, string outputFile, string privateKey, string password)
    {
        using (Stream inputStream = File.OpenRead(inputData))
        {
            using (Stream keyIn = IOHelper.GetStream(privateKey))
            {
                return DecryptPgpData(inputStream, outputFile, keyIn, password);
            }
        }
    }

    public static string DecryptPgpData(Stream inputStream, string outputFile, Stream privateKeyStream, string passPhrase)
    {
        string output = " ";

        try
        {
            PgpObjectFactory pgpFactory = new PgpObjectFactory(PgpUtilities.GetDecoderStream(inputStream));

            PgpObject? pgp = null;
            if (pgpFactory != null)
            {
                pgp = pgpFactory.NextPgpObject();
            }

            // the first object might be a PGP marker packet
            PgpEncryptedDataList? encryptedData = null;

            encryptedData = pgp is PgpEncryptedDataList
                ? (PgpEncryptedDataList)pgp
                : (PgpEncryptedDataList)pgpFactory.NextPgpObject();

            // decrypt
            PgpPrivateKey? privateKey = null;
            PgpPublicKeyEncryptedData? pubKeyData = null;

            //// find secret key
            PgpSecretKeyRingBundle pgpKeyRing = new PgpSecretKeyRingBundle(PgpUtilities.GetDecoderStream(privateKeyStream));

            if (encryptedData != null)
            {
                foreach (PgpPublicKeyEncryptedData pubKeyDataItem in encryptedData.GetEncryptedDataObjects())
                {
                    privateKey = FindSecretKey(pgpKeyRing, pubKeyDataItem.KeyId, passPhrase.ToCharArray());

                    if (privateKey != null)
                    {
                        pubKeyData = pubKeyDataItem;
                        break;
                    }
                }
            }
            else if (encryptedData == null)
            {
                throw new ArgumentException("Encrypted data for message not found.");
            }

            if (privateKey == null)
            {
                throw new ArgumentException("Secret key for message not found.");
            }
            using (Stream getStream = pubKeyData!.GetDataStream(privateKey))
            {
                PgpObjectFactory plainFact = new PgpObjectFactory(getStream);

                PgpObject message = plainFact.NextPgpObject();

                if (message is PgpCompressedData)
                {
                    PgpCompressedData compressedData = (PgpCompressedData)message;

                    using (Stream compDataIn = compressedData.GetDataStream())
                    {
                        PgpObjectFactory pgpCompressedFactory = new PgpObjectFactory(compDataIn);

                        Stream unc;
                        message = pgpCompressedFactory.NextPgpObject();
                        if (message != null && message is PgpOnePassSignatureList)
                        {
                            message = pgpCompressedFactory.NextPgpObject();

                            if (message != null)
                            {
                                PgpLiteralData? pgpLiteralData = null;
                                pgpLiteralData = (PgpLiteralData)message;
                                Stream streamOutput = File.Create(outputFile + "\\" + pgpLiteralData.FileName);
                                unc = pgpLiteralData.GetInputStream();
                                Streams.PipeAll(unc, streamOutput);
                                streamOutput.Close();
                                streamOutput.Dispose();
                            }
                            else
                            {
                                throw new ArgumentException("Message is null.");
                            }
                        }
                        else if (message != null)
                        {
                            PgpLiteralData? pgpLiteralData = null;
                            pgpLiteralData = (PgpLiteralData)message;
                            Stream streamOutput = File.Create(outputFile);
                            unc = pgpLiteralData.GetInputStream();
                            Streams.PipeAll(unc, streamOutput);
                            streamOutput.Close();
                            streamOutput.Dispose();
                        }
                        else
                        {
                            throw new ArgumentException("Message is null.");
                        }
                    }
                }
                else if (message is PgpLiteralData)
                {
                    PgpLiteralData literalData = (PgpLiteralData)message;
                    using (Stream streamData = literalData.GetInputStream())
                    {
                        output = IOHelper.GetString(streamData);
                    }
                }
                else if (message is PgpOnePassSignatureList)
                {
                    throw new PgpException("Encrypted message contains a signed message - not literal data.");
                }
                else
                {
                    throw new PgpException("Message is not a simple encrypted file - type unknown.");
                }
            }
        }
        catch (Exception e)
        {
            throw new PgpException("Encryption failed. please contact Support. Additional info: " + e.Message);
        }

        return output;
    }

    public static void EncryptPgpFile(
        string inputFile,
        string outputFile,
        string publicKey,
        bool armor,
        bool withIntegrityCheck,
        out int result)
    {
        result = 1;
        using (Stream publicKeyStream = IOHelper.GetStream(publicKey))
        {
            PgpPublicKey pubKey = ReadPublicKey(publicKeyStream);

            using (MemoryStream outputBytes = new MemoryStream())
            {
                PgpCompressedDataGenerator dataCompressor = new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);
                PgpUtilities.WriteFileToLiteralData(dataCompressor.Open(outputBytes), PgpLiteralData.Binary, new FileInfo(inputFile));

                dataCompressor.Close();
                PgpEncryptedDataGenerator dataGenerator = new PgpEncryptedDataGenerator(
                    SymmetricKeyAlgorithmTag.Cast5,
                    withIntegrityCheck,
                    new SecureRandom());

                dataGenerator.AddMethod(pubKey);
                byte[] dataBytes = outputBytes.ToArray();

                try
                {
                    using (Stream outputStream = File.Create(outputFile))
                    {
                        if (armor)
                        {
                            using (ArmoredOutputStream armoredStream = new ArmoredOutputStream(outputStream))
                            {
                                IOHelper.WriteStream(dataGenerator.Open(armoredStream, dataBytes.Length), ref dataBytes);
                            }
                        }
                        else
                        {
                            IOHelper.WriteStream(dataGenerator.Open(outputStream, dataBytes.Length), ref dataBytes);
                        }
                    }
                }
                catch (Exception e)
                {
                    result = 0;
                    throw new PgpException("Encryption failed " + e.Message, e);
                }
            }
        }
    }

    private static PgpPrivateKey? FindSecretKey(PgpSecretKeyRingBundle pgpSec, long keyId, char[] pass)
    {
        PgpSecretKey pgpSecKey = pgpSec.GetSecretKey(keyId);
        if (pgpSecKey == null)
        {
            return null;
        }

        return pgpSecKey.ExtractPrivateKey(pass);
    }

    private static PgpPublicKey ReadPublicKey(Stream inputStream)
    {
        inputStream = PgpUtilities.GetDecoderStream(inputStream);
        PgpPublicKeyRingBundle pgpPub = new PgpPublicKeyRingBundle(inputStream);

        foreach (PgpPublicKeyRing keyRing in pgpPub.GetKeyRings())
        {
            foreach (PgpPublicKey key in keyRing.GetPublicKeys())
            {
                if (key.IsEncryptionKey)
                {
                    return key;
                }
            }
        }

        throw new ArgumentException("Can't find encryption key in key ring.");
    }
}
