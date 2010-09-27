Feature: Selectbox automation
	In order show that the WhiteStepHelper makes you write less code
	As a developer of WhiteStepHelper
	I want to be able to automate the use of selectbox

Background:
	Given that the application is started
		And that the control 'Test selectbox' is shown

Scenario: Select a row in a selectbox
	When I select 'Item 3' in the selectbox 'Test selectbox'
	Then 'Item 3' should be selected in selectbox 'Test selectbox'
		And the text in textbox 'Test result' should be 'You selected Item 3'