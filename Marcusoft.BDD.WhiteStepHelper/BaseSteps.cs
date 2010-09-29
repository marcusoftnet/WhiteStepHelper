using White.Core.UIItems.ListBoxItems;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using White.Core.UIItems;
namespace Marcusoft.BDD.WhiteStepHelper
{
    [Binding()]
    public class BaseSteps : WhiteStepBase
    {

        public BaseSteps() :
            base(new AutomationNameResolver())
        {
        }

        // '' <summary>
        // '' Asserts that the application is started
        // '' </summary>
        // '' <remarks></remarks>
        [Given(@"that the application is started")]
        [Then(@"the application should be started")]
        public void ApplicationIsStarted()
        {
            Assert.AreEqual(true, IsApplicationUnderTestRunning);
        }

        // '' <summary>
        // '' Asserts that the window under test has the given title
        // '' </summary>
        // '' <param name="expectedTitle">the expected title of the window</param>
        // '' <remarks></remarks>
        [Given(@"that the '(.*)' window is shown")]
        [Then(@"the '(.*)' window is shown")]
        public void WindowUnderTestHaveTitle(string expectedTitle)
        {
            SetWindowUnderTest(expectedTitle);
            Assert.IsTrue(CurrentWindowUnderTest.Visible);
            Assert.AreEqual(expectedTitle, CurrentWindowUnderTest.Title);
        }

        // '' <summary>
        // '' Asserts that the window under test has the given title
        // '' </summary>
        // '' <param name="expectedTitle">the expected title of the window</param>
        // '' <remarks></remarks>
        [Given(@"that the window has the title '(.*)'")]
        [Then(@"the window should have the title '(.*)'")]
        public void WindowIsShown(string expectedTitle)
        {
            SetWindowUnderTest(expectedTitle);
            Assert.IsTrue(CurrentWindowUnderTest.Visible);
            Assert.AreEqual(expectedTitle, CurrentWindowUnderTest.Title);
        }

        // '' <summary>
        // '' Clicks the control with the given name
        // '' </summary>
        // '' <param name="wellKnownName">the wellknown name of the control</param>
        // '' <remarks></remarks>
        [Given(@"that I click on '(.*)'")]
        [When(@"I click on '(.*)'")]
        public void ClickControl(string wellKnownName)
        {
            GetControl(wellKnownName).Click();
        }

        // '' <summary>
        // '' Asserts that the textbox text is the expected text
        // '' </summary>
        // '' <param name="wellKnownName">the wellknown name of the textbox</param>
        // '' <param name="expectedText">the text to expect</param>
        // '' <remarks></remarks>
        [Then(@"the text in textbox '(.*)' should be '(.*)'")]
        [Given(@"that the text in textbox '(.*)' should be '(.*)'")]
        public void TextBoxShouldHaveText(string wellKnownName, string expectedText)
        {
            var txt = GetControl<TextBox>(wellKnownName);
            Assert.AreEqual(expectedText, txt.Text);
        }

        // '' <summary>
        // '' Asserts that the textbox text starts with the expected text
        // '' </summary>
        // '' <param name="wellKnownName">the wellknown name of the textbox</param>
        // '' <param name="expectedText">the text to expect</param>
        // '' <remarks></remarks>
        [Then(@"the textbox '(.*)' starts with the text '(.*)'")]
        [Given(@"that the textbox '(.*)' starts with the text '(.*)'")]
        public void TextBoxTextStartsWith(string wellKnownName, string expectedText)
        {
            var txt = GetControl<TextBox>(wellKnownName);
            Assert.IsTrue(txt.Text.StartsWith(expectedText));
        }

        // '' <summary>
        // '' Asserts that the label text contains the expected text
        // '' </summary>
        // '' <param name="wellKnownLabelName">the wellknown name of the label</param>
        // '' <param name="expectedText">the text to expect</param>
        // '' <remarks></remarks>
        [Then(@"the label '(.*)' should contain the text '(.*)'")]
        protected void Assert_LabelContainsText(string wellKnownLabelName, string expectedText)
        {
            var lbl = GetControl<Label>(wellKnownLabelName);
            Assert.AreEqual(expectedText, lbl.Text);
        }

        // '' <summary>
        // '' Writes the text into the textbox with the well known name
        // '' </summary>
        // '' <param name="wellKnownName">the name of the textbox</param>
        // '' <param name="textToWrite">the text to write</param>
        // '' <remarks></remarks>
        [Given(@"that I write '(.*)' in the textbox '(.*)'")]
        [When(@"I write '(.*)' in the textbox '(.*)'")]
        public void WriteInTextBox(string textToWrite, string wellKnownName)
        {
            GetControl<TextBox>(wellKnownName).BulkText = textToWrite;
        }

        // '' <summary>
        // '' Asserts that the control of the name is present on the 
        // '' current window under test
        // '' </summary>
        // '' <param name="wellKnownName">the well known name of the control</param>
        // '' <remarks></remarks>
        [Given(@"that the control '(.*)' is shown")]
        public void ControlIsPresentAndEnabled(string wellKnownName)
        {
            var c = GetControl(wellKnownName);
            Assert.IsNotNull(c);
            Assert.IsTrue(c.Enabled);
        }

        // '' <summary>
        // '' Asserts that the expected item text is the text of
        // '' the selected item of the combo-box
        // '' </summary>
        // '' <param name="expectedItemText"></param>
        // '' <param name="wellKnownSelectBoxName"></param>
        // '' <remarks></remarks>
        [Given(@"that '(.*)' is selected in selectbox '(.*)'")]
        [Then(@"'(.*)' should be selected in selectbox '(.*)'")]
        public void ItemShouldBeSelectedInSelectBox(string expectedItemText, string wellKnownSelectBoxName)
        {
            var sel = GetControl<ComboBox>(wellKnownSelectBoxName);
            Assert.AreEqual(expectedItemText, sel.SelectedItemText);
        }

        // '' <summary>
        // '' Selects a value by text in the selectbox
        // '' </summary>
        // '' <param name="itemTextToSelect">the text of the item to select</param>
        // '' <param name="wellKnownSelectBoxName">the wellknown name of the selectbox</param>
        // '' <remarks></remarks>
        [When(@"I select '(.*)' in the selectbox '(.*)'")]
        public void WhenISelectItem2InTheSelectboxTestSelectbox(string itemTextToSelect, string wellKnownSelectBoxName)
        {
            GetControl<ComboBox>(wellKnownSelectBoxName).Select(itemTextToSelect);
        }

        // '' <summary>
        // '' Double clicks the control with the given name
        // '' </summary>
        // '' <param name="wellKnownName">the wellknown name of the control</param>
        // '' <remarks></remarks>
        [When(@"I doubleclick on '(.*)'")]
        public void DoubleClickControl(string wellKnownName)
        {
            GetControl(wellKnownName).DoubleClick();
        }

        // '' <summary>
        // '' The dialog wit the a given title is shown
        // '' </summary>
        // '' <param name="dialogTitle">the title of the dialog</param>
        // '' <remarks></remarks>
        [Given(@"that the '(.*)' dialog is shown")]
        [Then(@"the '(.*)' dialog is shown")]
        public void DialogWindowIsShown(string dialogTitle)
        {
            var d = GetModalDialogByTitle(dialogTitle);
            Assert.AreEqual(true, d.IsFocussed);
        }
    }
}