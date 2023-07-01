using System.Text;
using System.Security.Cryptography;
using DataMedic.Application.Common.Interfaces.Infrastructure;

using Microsoft.Extensions.Options;

namespace DataMedic.Infrastructure.Cryptography;

public sealed class EncryptionService : IEncryptionService
{
    private const CipherMode Mode = CipherMode.CBC;
    private readonly EncryptionSettings _encryptionSettings;

    public EncryptionService(IOptions<EncryptionSettings> encryptionSettings)
    {
        _encryptionSettings = encryptionSettings.Value;
    }

    public byte[] Encrypt(byte[] data, out byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_encryptionSettings.Key);
        aes.Mode = Mode;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        iv = aes.IV;
        return encryptor.TransformFinalBlock(data, 0, data.Length);
    }

    public byte[] Decrypt(byte[] data, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_encryptionSettings.Key);
        aes.Mode = Mode;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        return decryptor.TransformFinalBlock(data, 0, data.Length);
    }
}
