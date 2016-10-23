General Intro and Description to will come.

Minor bug fixes are littered throughout the code with TODO Statements

Larger Enhancements/Fixes:

1.  Checking if objects are hidden before executing a command duplicates a ton of if statements and strings, there needs to be a pre_execute phase that somehow is right before the verbs execution.  The problem right now
	is that the pre_execute phase is before the main execution before we know exactly what verb it is.  It is possible but not great to have each command class have their own preexecute phase, this might be a solution, but it is still a ton of code in many areas.

2.  AbilitySpells and Actions are terrible, they need to be consolidated somehow nicely into regular commandclasses and another class that handles combat resources for each of the classes, probably another base class 
	and child classes for each of the types of resources, mana, action, etc.
	
3.  Need the ability to detect when a combat mob has attacked their primary target because in certain cases, if multiple people are in combat, someone can attack their primary target twice or counter attack their 
    secondary targets as well as their primary target.  There are hack solutions like adding a bool to let other threads know when they have attacked for the round/their primary target and accessors to toggle those as attacks are made, but there are a lot of race conditions with this model and strange conditions like when different people all start combats with different mobs at the same time etc etc.  The proper model is probably to just have a tick time and the main combat thread simply goes through and after attacks are made, decrements the attack action time by some value, like 4 seconds or whatever and then keeps cycling through the queue and adding time to whoever has already attacked until they reach the appropriate time to attack again.  This would only need 1 thread and other mobs would simply add whoever they were attacking to this theads queue.  This would require significant rework to the combat handler and is only necessary if full multiplayer is going to be supported with many people and many combats at a time... then again, if we support this we are going to need...
	
4.  Thread safety!  Almost none of this game is thread safe at the moment and there are race conditions all over the place with game resources.  

5.  Random actions need to be more generic.  This probably means there needs to be a random action class, and each Mob has a list of random actions and probabilities for each one.  When a mob is created, or when triggers
	happen, actions can be added to this list with probabilities or other factors on when the mob can do them, then this will be easily executed as the handlers can just grab the class from the mob and execute it.
	So like they would call something like mob.randomactionhandler.getrandomaction and then get the command executer and call execute on it.
	
6.  Events are triggered only by predicate1 events and not predicate2.  Events are not a class and are not flexible, I am not sure the solution to this, I could probably just extend the current struct to support 
	predicate2 triggered events.
	
7.  Add a way to customize what the client string is when pre-post execute commands fail.  For example, if an object is hidden and you try to get it, I hardcode 'you can't find that' or something like that.  I would like
	To use the hidden check pre command check with passed targets for the above situation.  Maybe the simple solution is to just expand the error codes a ton, and then based on the error code just make the client string the same thing.