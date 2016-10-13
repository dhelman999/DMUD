General Intro and Description to will come.

Minor bug fixes are littered throughout the code with TODO Statements

Larger Enhancements/Fixes:

1.  Checking if objects are hidden before executing a command duplicates a ton of if statements and strings, there needs to be a pre_execute phase that somehow is right before the verbs execution.  The problem right now
	is that the pre_execute phase is before the main execution before we know exactly what verb it is.  It is possible but not great to have each command class have their own preexecute phase, this might be a solution, but it is still a ton of code in many areas.

2.  AbilitySpells and Actions are terrible, they need to be consolidated somehow nicely into regular commandclasses and another class that handles combat resources for each of the classes, probably another base class 
	and child classes for each of the types of resources, mana, action, etc.
	