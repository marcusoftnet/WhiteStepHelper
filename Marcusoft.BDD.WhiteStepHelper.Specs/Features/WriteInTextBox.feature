Feature: Textbox automation
	In order show that the WhiteStepHelper makes you write less code
	As a developer of WhiteStepHelper
	I want to be able to automate writing and reading textbox text

Background:
	Given that the application is started

Scenario: Write in textbox
	When I write 'My test string' in the textbox 'Test textbox'
	Then the text in textbox 'Test textbox' should be 'My test string'

