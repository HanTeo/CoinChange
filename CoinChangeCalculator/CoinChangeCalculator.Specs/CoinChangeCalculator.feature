Feature: CoinChangeCalculator
	In order to give change
	As a calculator
	I need to calculate the appropriate number of coins for each available denomination of coins that adds up to the change amount

Scenario: Make Correct Change
	Given I have a target amount of 62 which to make change for
	And I have the available coins each have values
		|	Denomination	|	Value	|
		|	2p				|	2			|
		|	5p				|	5			|
		|	10p				|	10			|
		|	50p				|	50			|
	And I have the denominations each are of quantities
		|	Denomination	|	Quantity	|
		|	2p				|	1			|
		|	5p				|	1			|
		|	10p				|	1			|
		|	50p				|	1			|
	When I calculate the change
	Then the result should be
		|	Denomination	|	Quantity	|
		|	2p				|	1			|
		|	10p				|	1			|
		|	50p				|	1			|

