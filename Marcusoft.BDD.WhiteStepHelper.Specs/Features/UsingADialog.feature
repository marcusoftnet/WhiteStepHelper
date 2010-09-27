Feature: Using a dialog automation
	In order show that the WhiteStepHelper makes you write less code
	As a developer of WhiteStepHelper
	I want to be able to automate the use of dialogs and using them

Background:
	Given that the application is started

Scenario: Opening a new dialog
	When I click on 'New dialog button'
	Then the 'New and shiny' dialog is shown
	Given that the 'New and shiny' dialog is shown
	When I click on 'Close me button' 
	Then the 'Main' window is shown