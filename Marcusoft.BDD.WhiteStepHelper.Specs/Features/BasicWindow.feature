Feature: Basic Window automation
	In order show that the WhiteStepHelper makes you write less code
	As a developer of WhiteStepHelper
	I want to be able to automate the basic features of a window

Background:
	Given that the application is started
		And that the 'Main' window is shown

Scenario: Window has title 'Main'
	Then the window should have the title 'Main'

Scenario: Result label has the text 'Result'
	Given that the control 'Result textbox label' is shown

Scenario: Double click on Result label
	When I doubleclick on 'Result textbox label'
	Then the text in textbox 'Test result' should be 'You double clicked the Result textbox label'
