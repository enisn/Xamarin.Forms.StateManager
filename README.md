# Xamarin.Forms.StateManager
A simple & light UI state manager for Xamarin Forms.

![Nuget](https://img.shields.io/nuget/v/Xamarin.Forms.StateManager?logo=nuget&style=flat-square)
![CodeFactor](https://www.codefactor.io/repository/github/enisn/xamarin.forms.statemanager/badge/master)

## Why?
### Performance

Normally, states can be managed via binding `IsVisible` property of Views Controls. But when you use `IsVisible` property, that object will be initilized and allocated lots of memory on ram and it makes latency when openning page.
This StateManager uses **DataTemplate** instead of initializing all components. Only views related to the current state will be initialized and when state has changed, they will be removed and next state elements will be placed at UI.

### Ease of use

This StateManager makes much more easier to manage states on XAML pages. This provides much more manageable XAML code and easy to read.


On the situation below, both of component will be initialized at the beginning:
```xml
<ActivityIndicator IsRunning="{Binding IsBusy}" IsVisible="{Binding IsBusy"/>
<Label Text="{Binding MyData}" IsVisible="{Binding IsBusy, Converter={StaticResource BoolInverter}}" />
```

But state manager provides you to completely seperate your states and it uses **lazy load** for better performance.


# Instructions

## Set-Up

- Install NuGet Package `Xamarin.Forms.StateManager` only to your portable library.


***

## Usage

- Add following namespace at the top of your XAML page:

`xmlns:sm="clr-namespace:Xamarin.Forms.StateManager.Controls;assembly=Xamarin.Forms.StateManager"`

- Use **StateManager** anywhere you need:

```xml
   <StackLayout VerticalOptions="CenterAndExpand" Padding="25">
        <Button Text="Reload" Command="{Binding LoadCommand}" HorizontalOptions="Center" Margin="50"/>
        <Frame CornerRadius="20" HasShadow="True">
            <sm:StateManager State="{Binding CurrentState}">

                <sm:StateTemplate State="Loading">
                    <DataTemplate>
                        <ActivityIndicator IsRunning="True"/>
                    </DataTemplate>
                </sm:StateTemplate>

                <sm:StateTemplate State="Loaded">
                    <DataTemplate>
                        <Label Text="{Binding Content}" HorizontalOptions="Center" VerticalOptions="CenterAndExpand" />
                    </DataTemplate>
                </sm:StateTemplate>
                
                <sm:StateTemplate State="Failed">
                    <DataTemplate>
                        <StackLayout>
                            <Label Text="Failed to load :(" HorizontalOptions="Center" VerticalOptions="CenterAndExpand" />
                            <Button Text="Retry" Command="{Binding LoadCommand}"/>
                        </StackLayout>
                    </DataTemplate>
                </sm:StateTemplate>
            </sm:StateManager>
        </Frame>
    </StackLayout>
```

- You must define a default State at your **ViewModel**

```csharp
public class MainViewModel : BindableObject
{
    private string currentState = "Failed"; // Must define a default state
    private string content;

    private static readonly Random random = new Random();
    public MainViewModel()
    {
        this.LoadCommand = new Command(Load);
    }

    public string CurrentState { get => currentState; set { currentState = value; OnPropertyChanged(); } }

    public string Content { get => content; set { content = value; OnPropertyChanged(); } }

    public ICommand LoadCommand { get; set; }

    private async void Load()
    {
        this.CurrentState = "Loading";

        await Task.Delay(2000);

        var tmp = random.Next(0, 100);
        if (tmp > 50)
        {
            this.CurrentState = "Failed";
        }
        else
        {
            this.CurrentState = "Loaded";
            this.Content = "This is loaded content.";
        }
    }
}
```
