Feature: Textbox automation
	In order show that the WhiteStepHelper makes you write less code
	As a developer of WhiteStepHelper
	I want to be able to automate writing and reading textbox text

Background:
	Given that the application is started

Scenario Outline: Write in textbox
	When I write '<textstring>' in the textbox '<textboxnamn>'
	Then the text in textbox '<textboxnamn>' should be '<textstring>'
Examples:
	| textstring | textboxnamn |
	| 123		 |Test textbox |
	| 123		 |Test textbox |
	| 123		 |Test textbox |
	| 123		 |Test textbox |
	| 123		 |Test textbox |
	| 123		 |Test textbox |
	| 123		 |Test textbox |
	

