using Fxfer.TransferWorker.Application.Helpers;
using Fxfer.TransferWorker.Domain.Exceptions;
using Fxfer.TransferWorker.Contracts.Batch;
using Fxfer.TransferWorker.Contracts.NotificationService;
using Fxfer.TransferWorker.Infrastructure.Eventing;
using Fxfer.TransferWorker.Contracts.Eventing;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace Fxfer.TransferWorker.Tests;

public sealed class BackendBusinessHelpersTests
{
    private const string SpecRoot = "C:/platform-code/specs/provider-file-transfer-v2";

    [Fact]
    public void DuplicateFileNameException_sets_Message_from_constructor_argument()
    {
        var message = "dup";

        var ex = new DuplicateFileNameException(message);

        Assert.Contains(message, ex.Message);
    }

    [Fact]
    public void IOHelper_GetStream_returns_a_stream_positioned_at_zero()
    {
        using var stream = IOHelper.GetStream("abc");

        Assert.Equal(0, stream.Position);
    }

    [Fact]
    public void IOHelper_GetString_reads_the_entire_stream()
    {
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("hello"));

        var value = IOHelper.GetString(stream);

        Assert.Equal("hello", value);
    }

    [Fact]
    public void IOHelper_WriteStream_writes_all_bytes()
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes("abc");

        var stream = new NonDisposingMemoryStream();

        IOHelper.WriteStream(stream, ref bytes);

        Assert.Equal(bytes, stream.ToArray());
    }

    [Fact]
    public void EncryptionHelper_DecryptPgpData_string_overload_reads_input_file_and_delegates_to_stream_overload()
    {
        var temp = CreateTempDirectory();

        try
        {
            var passPhrase = "pass";
            var keys = CreateKeyRings("test@example.com", passPhrase);
            var encryptedBytes = EncryptLiteralData(keys.EncryptionKey, System.Text.Encoding.UTF8.GetBytes("hello"), armor: true);

            var inputPath = Path.Combine(temp, "input.pgp");
            File.WriteAllBytes(inputPath, encryptedBytes);

            string streamResult;
            using (var inputStream = File.OpenRead(inputPath))
            using (var privateKeyStream = IOHelper.GetStream(keys.SecretKeyArmored))
            {
                streamResult = EncryptionHelper.DecryptPgpData(inputStream, Path.Combine(temp, "unused"), privateKeyStream, passPhrase);
            }

            var stringResult = EncryptionHelper.DecryptPgpData(inputPath, Path.Combine(temp, "unused2"), keys.SecretKeyArmored, passPhrase);

            Assert.Equal(streamResult, stringResult);
        }
        finally
        {
            SafeDeleteDirectory(temp);
        }
    }

    [Fact]
    public void EncryptionHelper_DecryptPgpData_throws_PgpException_when_encrypted_data_list_is_missing()
    {
        using var inputStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("not pgp"));

        var ex = Assert.Throws<PgpException>(() =>
            EncryptionHelper.DecryptPgpData(inputStream, "unused", new MemoryStream(), "pass"));

        Assert.StartsWith("Encryption failed. please contact Support. Additional info: ", ex.Message);
    }

    [Fact]
    public void EncryptionHelper_DecryptPgpData_throws_PgpException_when_secret_key_is_not_found()
    {
        var temp = CreateTempDirectory();

        try
        {
            var passPhrase = "pass";
            var encryptKeys = CreateKeyRings("enc@example.com", passPhrase);
            var decryptKeys = CreateKeyRings("dec@example.com", passPhrase);

            var encryptedBytes = EncryptLiteralData(encryptKeys.EncryptionKey, System.Text.Encoding.UTF8.GetBytes("hello"), armor: true);

            using var inputStream = new MemoryStream(encryptedBytes);
            using var privateKeyStream = IOHelper.GetStream(decryptKeys.SecretKeyArmored);

            var ex = Assert.Throws<PgpException>(() =>
                EncryptionHelper.DecryptPgpData(inputStream, Path.Combine(temp, "unused"), privateKeyStream, passPhrase));

            Assert.StartsWith("Encryption failed. please contact Support. Additional info: ", ex.Message);
            Assert.Contains("Secret key for message not found.", ex.Message);
        }
        finally
        {
            SafeDeleteDirectory(temp);
        }
    }

    [Fact]
    public void EncryptionHelper_DecryptPgpData_writes_outputFile_when_payload_is_compressed_literal_data_without_signature_list()
    {
        var temp = CreateTempDirectory();

        try
        {
            var passPhrase = "pass";
            var keys = CreateKeyRings("test@example.com", passPhrase);

            var inputFile = Path.Combine(temp, "plain.txt");
            File.WriteAllText(inputFile, "abc");

            var encryptedFile = Path.Combine(temp, "encrypted.pgp");
            EncryptionHelper.EncryptPgpFile(inputFile, encryptedFile, keys.PublicKeyArmored, armor: false, withIntegrityCheck: true, out _);

            var outputFile = Path.Combine(temp, "out.bin");
            using (var inputStream = File.OpenRead(encryptedFile))
            using (var privateKeyStream = IOHelper.GetStream(keys.SecretKeyArmored))
            {
                _ = EncryptionHelper.DecryptPgpData(inputStream, outputFile, privateKeyStream, passPhrase);
            }

            Assert.True(File.Exists(outputFile));
        }
        finally
        {
            SafeDeleteDirectory(temp);
        }
    }

    [Fact]
    public void EncryptionHelper_DecryptPgpData_writes_outputFile_plus_literal_filename_when_payload_is_signed_compressed_literal_data()
    {
        var temp = CreateTempDirectory();

        try
        {
            var passPhrase = "pass";
            var keys = CreateKeyRings("test@example.com", passPhrase);
            var outputDir = Path.Combine(temp, "out");
            Directory.CreateDirectory(outputDir);

            var signedCompressed = EncryptSignedCompressedLiteral(
                keys,
                passPhrase,
                fileName: "a.txt",
                payload: System.Text.Encoding.UTF8.GetBytes("abc"),
                armor: true);

            using var inputStream = new MemoryStream(signedCompressed);
            using var privateKeyStream = IOHelper.GetStream(keys.SecretKeyArmored);

            _ = EncryptionHelper.DecryptPgpData(inputStream, outputDir, privateKeyStream, passPhrase);

            Assert.True(File.Exists(Path.Combine(outputDir, "a.txt")));
        }
        finally
        {
            SafeDeleteDirectory(temp);
        }
    }

    [Fact]
    public void EncryptionHelper_DecryptPgpData_returns_text_when_payload_is_literal_data()
    {
        var passPhrase = "pass";
        var keys = CreateKeyRings("test@example.com", passPhrase);

        var encryptedBytes = EncryptLiteralData(keys.EncryptionKey, System.Text.Encoding.UTF8.GetBytes("hello"), armor: true);

        using var inputStream = new MemoryStream(encryptedBytes);
        using var privateKeyStream = IOHelper.GetStream(keys.SecretKeyArmored);

        var result = EncryptionHelper.DecryptPgpData(inputStream, "unused", privateKeyStream, passPhrase);

        Assert.Equal("hello", result);
    }

    [Fact]
    public void EncryptionHelper_DecryptPgpData_throws_PgpException_when_payload_is_one_pass_signature_list_without_literal_data()
    {
        var passPhrase = "pass";
        var keys = CreateKeyRings("test@example.com", passPhrase);

        var encryptedBytes = EncryptOnePassSignatureListOnly(keys, passPhrase, armor: true);

        using var inputStream = new MemoryStream(encryptedBytes);
        using var privateKeyStream = IOHelper.GetStream(keys.SecretKeyArmored);

        var ex = Assert.Throws<PgpException>(() =>
            EncryptionHelper.DecryptPgpData(inputStream, "unused", privateKeyStream, passPhrase));

        Assert.StartsWith("Encryption failed. please contact Support. Additional info: ", ex.Message);
        Assert.Contains("Encrypted message contains a signed message - not literal data.", ex.Message);
    }

    [Fact]
    public void EncryptionHelper_DecryptPgpData_throws_PgpException_when_payload_type_is_unknown()
    {
        var passPhrase = "pass";
        var keys = CreateKeyRings("test@example.com", passPhrase);

        var encryptedBytes = EncryptPgpObject(keys, keys.PublicKeyRing, armor: true);

        using var inputStream = new MemoryStream(encryptedBytes);
        using var privateKeyStream = IOHelper.GetStream(keys.SecretKeyArmored);

        var ex = Assert.Throws<PgpException>(() =>
            EncryptionHelper.DecryptPgpData(inputStream, "unused", privateKeyStream, passPhrase));

        Assert.StartsWith("Encryption failed. please contact Support. Additional info: ", ex.Message);
        Assert.Contains("Message is not a simple encrypted file - type unknown.", ex.Message);
    }

    [Fact]
    public void EncryptionHelper_EncryptPgpFile_sets_result_0_and_throws_PgpException_when_output_write_fails()
    {
        var temp = CreateTempDirectory();

        try
        {
            var passPhrase = "pass";
            var keys = CreateKeyRings("test@example.com", passPhrase);

            var inputFile = Path.Combine(temp, "plain.txt");
            File.WriteAllText(inputFile, "abc");

            var outputFile = Path.Combine(temp, "missing-dir", "out.pgp");

            var result = -1;
            var ex = Assert.Throws<PgpException>(() =>
                EncryptionHelper.EncryptPgpFile(inputFile, outputFile, keys.PublicKeyArmored, armor: true, withIntegrityCheck: true, out result));

            Assert.Equal(0, result);
            Assert.StartsWith("Encryption failed ", ex.Message);
            Assert.NotNull(ex.InnerException);
        }
        finally
        {
            SafeDeleteDirectory(temp);
        }
    }

    [Fact]
    public void EncryptionHelper_EncryptPgpFile_writes_armored_output_when_armor_true()
    {
        var temp = CreateTempDirectory();

        try
        {
            var passPhrase = "pass";
            var keys = CreateKeyRings("test@example.com", passPhrase);

            var inputFile = Path.Combine(temp, "plain.txt");
            File.WriteAllText(inputFile, "abc");

            var outputFile = Path.Combine(temp, "out.pgp");
            EncryptionHelper.EncryptPgpFile(inputFile, outputFile, keys.PublicKeyArmored, armor: true, withIntegrityCheck: true, out _);

            var firstLine = File.ReadLines(outputFile).First();
            Assert.StartsWith("-----BEGIN PGP MESSAGE-----", firstLine);
        }
        finally
        {
            SafeDeleteDirectory(temp);
        }
    }

    [Fact]
    public void EncryptionHelper_EncryptPgpFile_writes_binary_output_when_armor_false()
    {
        var temp = CreateTempDirectory();

        try
        {
            var passPhrase = "pass";
            var keys = CreateKeyRings("test@example.com", passPhrase);

            var inputFile = Path.Combine(temp, "plain.txt");
            File.WriteAllText(inputFile, "abc");

            var outputFile = Path.Combine(temp, "out.pgp");
            EncryptionHelper.EncryptPgpFile(inputFile, outputFile, keys.PublicKeyArmored, armor: false, withIntegrityCheck: true, out _);

            var bytes = File.ReadAllBytes(outputFile);
            var header = System.Text.Encoding.ASCII.GetString(bytes.Take(16).ToArray());
            Assert.DoesNotContain("BEGIN PGP MESSAGE", header);
        }
        finally
        {
            SafeDeleteDirectory(temp);
        }
    }

    [Fact]
    public void EventHelper_does_not_publish_when_event_storage_is_null()
    {
        var helper = new EventHelper(null);

        var file = new FileToDeliverModel
        {
            SourceLocation = new SourceLocationModel(),
            FileName = "a.txt",
            FileId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
        };

        helper.PublishFileTransferredEvent(file, tenantId: "t1", elapsedTime: TimeSpan.FromMilliseconds(5));
    }

    [Fact]
    public void EventHelper_does_not_publish_when_delivery_is_not_used_for_system_integration()
    {
        var storage = new CountingDistributedEventStorage();
        var helper = new EventHelper(storage);

        var file = new FileToDeliverModel
        {
            SourceLocation = new SourceLocationModel
            {
                Destinations = new List<DestinationModel>(),
            },
            FileName = "a.txt",
            FileId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
        };

        helper.PublishFileTransferredEvent(file, tenantId: "t1", elapsedTime: TimeSpan.FromMilliseconds(5));

        Assert.Equal(0, storage.AddCallCount);
    }

    [Fact]
    public void EventHelper_publishes_when_delivery_is_used_for_system_integration()
    {
        var storage = new CountingDistributedEventStorage();
        var helper = new EventHelper(storage);

        var file = new FileToDeliverModel
        {
            SourceLocation = new SourceLocationModel
            {
                Destinations = new List<DestinationModel>
                {
                    new()
                    {
                        FTEMetadataId = Guid.NewGuid(),
                        DestinationAddress = "dest",
                        ContractName = "contract",
                    },
                },
                SourceLocationId = Guid.NewGuid(),
                SourceAddress = "src",
                FileMask = "*.txt",
                SourceProtocolCode = "SFTP",
                OwnerName = "owner",
                ConfigurationId = 123,
                ConfigurationName = "cfg",
                ConfigurationDescription = "desc",
                FileDirection = "IN",
            },
            FileName = "a.txt",
            FileId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
            FileType = "TXT",
            FileSize = 42,
        };

        helper.PublishFileTransferredEvent(file, tenantId: "t1", elapsedTime: TimeSpan.FromMilliseconds(5));

        Assert.Equal(1, storage.AddCallCount);
        Assert.NotNull(storage.LastMessage);
    }

    [Fact]
    public void NotificationServiceHelper_PostEvent_sets_PreviousEvent()
    {
        var tenantId = "t1";
        var svc = new FakeNotificationService();
        var expectedId = Guid.NewGuid();
        svc.NextPostEventResult = new PostEvent { EventId = expectedId };

        var helper = new NotificationServiceHelper(tenantId, svc);

        helper.PostEvent("evt", "sub", "body", isArchived: false, operatorId: "op");

        Assert.NotNull(helper.PreviousEvent);
        Assert.Equal(expectedId, helper.PreviousEvent.EventId);
    }

    [Fact]
    public void NotificationServiceHelper_PostEvent_passes_lastEventId_when_PreviousEvent_exists()
    {
        var tenantId = "t1";
        var svc = new FakeNotificationService();

        var helper = new NotificationServiceHelper(tenantId, svc);

        var firstId = Guid.NewGuid();
        svc.NextPostEventResult = new PostEvent { EventId = firstId };
        helper.PostEvent("evt", "sub", "body", isArchived: false, operatorId: "op");

        var secondId = Guid.NewGuid();
        svc.NextPostEventResult = new PostEvent { EventId = secondId };
        helper.PostEvent("evt", "sub", "body2", isArchived: false, operatorId: "op");

        Assert.Equal(firstId, svc.LastEventId);
    }

    [Fact]
    public void NotificationServiceHelper_SendEmail_delegates_to_service()
    {
        var tenantId = "t1";
        var svc = new FakeNotificationService();
        var helper = new NotificationServiceHelper(tenantId, svc);

        helper.SendEmail(
            subject: "sub",
            emailBody: "body",
            addressList: new List<Address>(),
            fromAddress: new Address(),
            priority: MailPriority.Normal,
            attachmentList: new List<Attachments>());

        Assert.Equal(1, svc.SendEmailCallCount);
        Assert.Equal(tenantId, svc.LastTenantId);
    }

    [Fact]
    public void OpenApi_has_no_http_surface()
    {
        var openApiPath = Path.Combine(SpecRoot, "technical/components/backend-business-helpers/openapi.yaml");
        var yaml = File.ReadAllText(openApiPath);

        Assert.Contains("x-no-http-surface: true", yaml);
        Assert.Contains("paths: {}", yaml);
    }

    [Fact]
    public void Contract_declares_no_cross_component_dependencies()
    {
        var contractPath = Path.Combine(SpecRoot, "technical/components/backend-business-helpers/CONTRACT.md");
        var markdown = File.ReadAllText(contractPath);

        Assert.Contains("None (no cross-component contracts declared by this component).", markdown);
    }


    [Fact]
    public void OpenApi_has_no_http_surface_helpers_2()
    {
        var openApiPath = Path.Combine(SpecRoot, "technical/components/backend-business-helpers-2/openapi.yaml");
        var yaml = File.ReadAllText(openApiPath);

        Assert.Contains("x-no-http-surface: true", yaml);
        Assert.Contains("paths: {}", yaml);
    }

    [Fact]
    public void Contract_declares_no_cross_component_dependencies_helpers_2()
    {
        var contractPath = Path.Combine(SpecRoot, "technical/components/backend-business-helpers-2/CONTRACT.md");
        var markdown = File.ReadAllText(contractPath);

        Assert.Contains("None (no cross-component contracts declared by this component).", markdown);
    }
    private static string CreateTempDirectory()
    {
        var root = Path.Combine(Path.GetTempPath(), "fxfer-transfer-worker-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        return root;
    }

    private static void SafeDeleteDirectory(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }
        }
        catch
        {
        }
    }


    private sealed class CountingDistributedEventStorage : IDistributedEventStorage
    {
        public int AddCallCount { get; private set; }

        public EventMessage? LastMessage { get; private set; }

        public void Add(EventMessage eventMessage)
        {
            AddCallCount++;
            LastMessage = eventMessage;
        }
    }

    private sealed class FakeNotificationService : INotificationService
    {
        public PostEvent NextPostEventResult { get; set; } = new();

        public Guid? LastEventId { get; private set; }

        public int SendEmailCallCount { get; private set; }

        public string? LastTenantId { get; private set; }

        public PostEvent PostEvent(
            string eventName,
            string subSystem,
            string messageBody,
            bool isArchived,
            DateTime createdDateTimeUtc,
            Guid? lastEventId,
            string operatorId,
            string tenantId)
        {
            LastEventId = lastEventId;
            return NextPostEventResult;
        }

        public void SendEmail(
            string tenantId,
            string subject,
            string emailBody,
            List<Address> addressList,
            Address fromAddress,
            MailPriority priority,
            List<Attachments> attachmentList)
        {
            SendEmailCallCount++;
            LastTenantId = tenantId;
        }
    }
    private sealed class NonDisposingMemoryStream : MemoryStream
    {
        protected override void Dispose(bool disposing)
        {
        }
    }

    private sealed record KeyRings(
        string PublicKeyArmored,
        string SecretKeyArmored,
        PgpPublicKey EncryptionKey,
        PgpSecretKey SecretKey,
        PgpPublicKeyRing PublicKeyRing);

    private static KeyRings CreateKeyRings(string identity, string passPhrase)
    {
        var random = new SecureRandom();

        var keyGenerator = new RsaKeyPairGenerator();
        keyGenerator.Init(new KeyGenerationParameters(random, 2048));
        AsymmetricCipherKeyPair keyPair = keyGenerator.GenerateKeyPair();

        var pgpKeyPair = new PgpKeyPair(PublicKeyAlgorithmTag.RsaGeneral, keyPair, DateTime.UtcNow);

        var keyRingGenerator = new PgpKeyRingGenerator(
            PgpSignature.PositiveCertification,
            pgpKeyPair,
            identity,
            SymmetricKeyAlgorithmTag.Cast5,
            passPhrase.ToCharArray(),
            true,
            null,
            null,
            random);

        var secretKeyRing = keyRingGenerator.GenerateSecretKeyRing();
        var publicKeyRing = keyRingGenerator.GeneratePublicKeyRing();

        var encryptionKey = GetEncryptionKey(publicKeyRing);
        var secretKey = secretKeyRing.GetSecretKey();

        var publicKeyArmored = Armor(publicKeyRing);
        var secretKeyArmored = Armor(secretKeyRing);

        return new KeyRings(publicKeyArmored, secretKeyArmored, encryptionKey, secretKey, publicKeyRing);
    }

    private static PgpPublicKey GetEncryptionKey(PgpPublicKeyRing publicKeyRing)
    {
        foreach (PgpPublicKey key in publicKeyRing.GetPublicKeys())
        {
            if (key.IsEncryptionKey)
            {
                return key;
            }
        }

        throw new InvalidOperationException("Can't find encryption key in generated key ring.");
    }

    private static string Armor(PgpPublicKeyRing ring)
    {
        using var output = new MemoryStream();
        using (var armored = new ArmoredOutputStream(output))
        {
            ring.Encode(armored);
        }

        return System.Text.Encoding.ASCII.GetString(output.ToArray());
    }

    private static string Armor(PgpSecretKeyRing ring)
    {
        using var output = new MemoryStream();
        using (var armored = new ArmoredOutputStream(output))
        {
            ring.Encode(armored);
        }

        return System.Text.Encoding.ASCII.GetString(output.ToArray());
    }

    private static byte[] EncryptLiteralData(PgpPublicKey publicKey, byte[] plaintext, bool armor)
    {
        var random = new SecureRandom();
        var generator = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, true, random);
        generator.AddMethod(publicKey);

        using var output = new MemoryStream();

        if (armor)
        {
            using (var armoredOut = new ArmoredOutputStream(output))
            using (var encryptedOut = generator.Open(armoredOut, new byte[1 << 16]))
            {
                var literal = new PgpLiteralDataGenerator();
                using (var literalOut = literal.Open(
                           encryptedOut,
                           PgpLiteralData.Binary,
                           "data",
                           plaintext.Length,
                           DateTime.UtcNow))
                {
                    literalOut.Write(plaintext, 0, plaintext.Length);
                }
            }
        }
        else
        {
            using (var encryptedOut = generator.Open(output, new byte[1 << 16]))
            {
                var literal = new PgpLiteralDataGenerator();
                using (var literalOut = literal.Open(
                           encryptedOut,
                           PgpLiteralData.Binary,
                           "data",
                           plaintext.Length,
                           DateTime.UtcNow))
                {
                    literalOut.Write(plaintext, 0, plaintext.Length);
                }
            }
        }

        return output.ToArray();
    }

    private static byte[] EncryptSignedCompressedLiteral(KeyRings keys, string passPhrase, string fileName, byte[] payload, bool armor)
    {
        var random = new SecureRandom();
        var generator = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, true, random);
        generator.AddMethod(keys.EncryptionKey);

        var privateKey = keys.SecretKey.ExtractPrivateKey(passPhrase.ToCharArray());
        var signatureGenerator = new PgpSignatureGenerator(keys.SecretKey.PublicKey.Algorithm, HashAlgorithmTag.Sha1);
        signatureGenerator.InitSign(PgpSignature.BinaryDocument, privateKey);

        using var output = new MemoryStream();

        if (armor)
        {
            using (var armoredOut = new ArmoredOutputStream(output))
            using (var encryptedOut = generator.Open(armoredOut, new byte[1 << 16]))
            {
                WriteSignedCompressedLiteral(encryptedOut);
            }
        }
        else
        {
            using (var encryptedOut = generator.Open(output, new byte[1 << 16]))
            {
                WriteSignedCompressedLiteral(encryptedOut);
            }
        }

        return output.ToArray();

        void WriteSignedCompressedLiteral(Stream destination)
        {
            var compressor = new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);
            using var compressedOut = compressor.Open(destination);

            var onePass = signatureGenerator.GenerateOnePassVersion(false);
            onePass.Encode(compressedOut);

            var literal = new PgpLiteralDataGenerator();
            using (var literalOut = literal.Open(
                       compressedOut,
                       PgpLiteralData.Binary,
                       fileName,
                       payload.Length,
                       DateTime.UtcNow))
            {
                literalOut.Write(payload, 0, payload.Length);
            }

            signatureGenerator.Update(payload);
            var signature = signatureGenerator.Generate();
            signature.Encode(compressedOut);
        }
    }

    private static byte[] EncryptOnePassSignatureListOnly(KeyRings keys, string passPhrase, bool armor)
    {
        var random = new SecureRandom();
        var generator = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, true, random);
        generator.AddMethod(keys.EncryptionKey);

        var privateKey = keys.SecretKey.ExtractPrivateKey(passPhrase.ToCharArray());
        var signatureGenerator = new PgpSignatureGenerator(keys.SecretKey.PublicKey.Algorithm, HashAlgorithmTag.Sha1);
        signatureGenerator.InitSign(PgpSignature.BinaryDocument, privateKey);

        using var output = new MemoryStream();

        if (armor)
        {
            using (var armoredOut = new ArmoredOutputStream(output))
            using (var encryptedOut = generator.Open(armoredOut, new byte[1 << 16]))
            {
                signatureGenerator.GenerateOnePassVersion(false).Encode(encryptedOut);
            }
        }
        else
        {
            using (var encryptedOut = generator.Open(output, new byte[1 << 16]))
            {
                signatureGenerator.GenerateOnePassVersion(false).Encode(encryptedOut);
            }
        }

        return output.ToArray();
    }

    private static byte[] EncryptPgpObject(KeyRings keys, PgpPublicKeyRing publicKeyRing, bool armor)
    {
        var random = new SecureRandom();
        var generator = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, true, random);
        generator.AddMethod(keys.EncryptionKey);

        using var output = new MemoryStream();

        if (armor)
        {
            using (var armoredOut = new ArmoredOutputStream(output))
            using (var encryptedOut = generator.Open(armoredOut, new byte[1 << 16]))
            {
                publicKeyRing.Encode(encryptedOut);
            }
        }
        else
        {
            using (var encryptedOut = generator.Open(output, new byte[1 << 16]))
            {
                publicKeyRing.Encode(encryptedOut);
            }
        }

        return output.ToArray();
    }
}


