Feature: Tab control management
	In order show that the WhiteStepHelper makes you write less code
	As a developer of WhiteStepHelper
	I want to be able to automate the basic features of tabs and tabcontrols

Background:
	Given that the application is started

Scenario: Select the List tab
	Given that 'Tree' is the selected tab on the tabsystem 'Main tabsystem'
	When I select the 'List' on the tabsystem 'Main tabsystem'
	Then 'List' is the selected tab on the tabsystem 'Main tabsystem'