Feature: Button automation
	In order show that the WhiteStepHelper makes you write less code
	As a developer of WhiteStepHelper
	I want to be able to automate the click of a button without writing any step code

Background:
	Given that the application is started

Scenario: Click existing button
	When I click on 'Test button' 
	Then the text in textbox 'Test result' should be 'You clicked the Test button'

