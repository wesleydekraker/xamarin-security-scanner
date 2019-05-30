using Android.App;
using Android.OS;
using Android.Support.V7.App;
using System.Security.Cryptography;

namespace BankingApp.TestFiles
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class CbcCipherMode : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
        }

        private ICryptoTransform GetCrypto()
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.KeySize = 256;
            rijndaelManaged.BlockSize = 256;
            rijndaelManaged.Mode = CipherMode.CBC;
            return rijndaelManaged.CreateEncryptor();
        }
    }
}