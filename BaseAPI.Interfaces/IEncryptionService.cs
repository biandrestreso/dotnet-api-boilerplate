using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAPI.Interfaces
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText, out byte[] salt);
        bool Verify(string plainText, string hash, byte[] salt);
    }
}
