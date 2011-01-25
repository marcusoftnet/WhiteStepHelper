Feature: Tree navigation
	In order show that the WhiteStepHelper makes you write less code
	As a developer of WhiteStepHelper
	I want to be able to automate the basic features of a tree and it's nodes

Background:
	Given that the application is started
	When I select the 'Tree' on the tabsystem 'Main tabsystem'
	Then the control 'Test tree' should be shown

Scenario: Test tree should have 5 top level nodes
	Then the tree 'Test tree' should have 5 nodes top level nodes

Scenario: Item 1 has two subnodes
	When I expand the node 'Item 1' in tree 'Test tree'
	Then the node 'Item 1' in tree 'Test tree' has 2 subnodes

Scenario: Expand down to 'Item 6'
	When I expand down to 'Item 6' in tree 'Test tree'
	Then the node 'Item 6' in tree 'Test tree' has 2 subnodes

	