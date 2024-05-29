Feature: RickAndMorty_Characters

@HumanCharacters
Scenario: Get two characters and check they are both human
	Given the first character has an ID of 5
		And the second character has an ID of 2
	When the two characters' species is compared to Human
	Then the result should be true

@FemaleCharacters
Scenario: Check Character Is Female
	Given A character ID of <Character>
	When A character is female
	Then Add to list of female characters
	Examples:
	| Character | 
	| 1         | 
	| 17        | 
	| 3         |
	| 4         |

