using Android.Content;

namespace JToolMobile
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private Button Button_Char;
        private Button Button_Number;
        private Button Button_Word;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Button_Char = FindViewById<Button>(Resource.Id.button_Char);
            Button_Number = FindViewById<Button>(Resource.Id.button_Number);
            Button_Word = FindViewById<Button>(Resource.Id.button_Word);

            Button_Char.Click += Button_Char_Click;
            Button_Number.Click += Button_Number_Click;
            Button_Word.Click += Button_Word_Click;
        }

        private void Button_Word_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_Number_Click(object? sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(NumberMain));

            StartActivity(intent);
        }

        private void Button_Char_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}