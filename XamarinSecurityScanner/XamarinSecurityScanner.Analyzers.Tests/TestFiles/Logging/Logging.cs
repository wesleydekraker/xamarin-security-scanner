using Android.App;
using Android.OS;
using Android.Util;

namespace BankingApp.TestFiles
{
    [Activity(Label = "@string/app_name")]
    public class Logging : Activity
    {
        private const string Tag = "MainActivity";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Log.Verbose(Tag, "OnCreate");
            Log.Debug(Tag, "OnCreate");
            Log.Info(Tag, "OnCreate");
            Log.Warn(Tag, "OnCreate");
            Log.Error(Tag, "OnCreate");
            Log.Wtf(Tag, "OnCreate");
            Log.WriteLine(Tag, "OnCreate");
        }
    }
}