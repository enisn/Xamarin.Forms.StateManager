using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.StateManager.Controls;
using Xunit;

namespace Xamarin.Forms.StateManager.Tests.Controls
{
    public class StateManagerBaseTests
    {
        public StateManagerBaseTests()
        {
            
        }

        [Fact]
        public void UpdateView_ShouldGenerateExpectedControl_WithExpectedState()
        {
            // Arrange
            var stateName = "State1";
            var expectedText = "Hello World!";
            var stateUnderTest = new StateManagerBase<string>();
            stateUnderTest.States.Add(new StateTemplateBase<string>() 
            {
                State = stateName,
                DataTemplate = new DataTemplate(() => new Label { Text = expectedText }) 
            });

            // Act
            stateUnderTest.UpdateView(stateName);
            var result = stateUnderTest.Children.FirstOrDefault();

            // Assert
            Assert.IsType<Label>(result);
            Assert.Equal(expectedText, (result as Label).Text);
        }
    }
}
