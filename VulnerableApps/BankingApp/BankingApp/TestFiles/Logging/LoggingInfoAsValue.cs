using Android.App;
using Android.OS;

namespace BankingApp.TestFiles
{
    [Activity(Label = "@string/app_name")]
    public class LoggingInfoAsValue : Activity
    {
        private const string Tag = "MainActivity";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            DoSomething("Log.Info(Tag, \"OnCreate\");");
        }

        public void DoSomething(string paramater)
        {
        }
    }
}