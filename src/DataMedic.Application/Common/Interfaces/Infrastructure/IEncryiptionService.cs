namespace DataMedic.Application.Common.Interfaces.Infrastructure;

public interface IEncryptionService
{
    byte[] Encrypt(byte[] data, out byte[] iv);

    byte[] Decrypt(byte[] encryptedData, byte[] iv);
}
