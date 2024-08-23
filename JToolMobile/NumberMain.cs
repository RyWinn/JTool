using Android.Content;

namespace JToolMobile;

[Activity(Label = "NumberMain")]
public class NumberMain : Activity
{
    private Button Button_LearnNum;
    private Button Button_NumMemory;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.numberMain);

        Button_LearnNum = FindViewById<Button>(Resource.Id.button_LearnNum);
        Button_NumMemory = FindViewById<Button>(Resource.Id.button_NumMemory);

        Button_LearnNum.Click += Button_LearnNum_Click;
        Button_NumMemory.Click += Button_NumMemory_Click;
    }

    private void Button_NumMemory_Click(object? sender, EventArgs e)
    {
        Intent intent = new Intent(this, typeof(NumberMemory));

        StartActivity(intent);
    }

    private void Button_LearnNum_Click(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}