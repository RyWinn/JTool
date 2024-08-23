using Android.Views.InputMethods;

namespace JToolMobile;

[Activity(Label = "NumberMemory")]
public class NumberMemory : Activity
{
    private Random Random = new Random();

    private TextView TextView_RandomNumber;
    private EditText EditText_RandomNumber;

    private KeyValuePair<int, string> Number;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.numberMemory);

        TextView_RandomNumber = FindViewById<TextView>(Resource.Id.textView_RandomNumber);
        EditText_RandomNumber = FindViewById<EditText>(Resource.Id.editText_RandomNumber);

        GetRandomNumber();

        EditText_RandomNumber.EditorAction += EditText_RandomNumber_EditorAction;
    }

    private void EditText_RandomNumber_EditorAction(object? sender, TextView.EditorActionEventArgs e)
    {
        if (e.ActionId == ImeAction.Done)
        {
            if (int.TryParse(EditText_RandomNumber.Text, out int SubmittedNumber))
            {
                if (Number.Key == SubmittedNumber)
                {
                    Toast.MakeText(this, "Correct", ToastLength.Long).Show();
                    EditText_RandomNumber.Text = "";
                    GetRandomNumber();
                }
                else
                {
                    Toast.MakeText(this, "Try Again", ToastLength.Long).Show();
                }
            }
        }
    }

    private void GetRandomNumber()
    {
        Dictionary<int, string> numbers = cNumber.Numbers;

        //Random number
        int randomNumberIndex = Random.Next(1, cNumber.Numbers.Count + 1);
        Number = cNumber.Numbers.ElementAt(randomNumberIndex - 1);

        TextView_RandomNumber.Text = Number.Value;
    }
}