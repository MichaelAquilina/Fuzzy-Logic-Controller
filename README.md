Fuzzy Logic Controller
======================

This was an assignment i wrote for my "Machine Learning, Expert Systems and Fuzzy Logic" course at the University of Malta.

For this assignment, we were required to design and implement a simple fuzzy logic controller that would be able to keep
the pressure of a boiler at a set point by using test cases to evaluate its performance. Fuzzy logic controllers are good
for such systems because they use analog inputs instead of binary, which allows for a smoother and more refined control 
over how the pressure changes over time.

The application was implemented using Microsoft’s .NET framework due to its rich library and broad support of different
technologies, while the programming language used was C# due to its object oriented nature and ease of use. In order to
produce a rich, well defined user interface, WPF was used due to its MVC paradigm and broad range of tools. All the 
charts in the application were drawn using the WPF Charting Toolkit which is available from Microsoft’s Developer Network.

Design
------

The system was designed so that all the logic behind the fuzzy logic controller and the simulation were independent from
one another to prevent coupling. The user interface which was built for this simulation obtained data from the fuzzy 
logic controller by means of a simple an API which would allow it to obtain the necessary data when needed. The overall
architecture of the system can be seen in the diagram below.

![Design Overview](/img/Design.png)

Using the Simulator
-------------------

**Running the Simulation**
The simulation may be run from the “Simulation” tab where the parameters for the simulation may be changed according to the need of the simulation case. The following parameters may be changed from the Simulation tab:
* Defuzzification Method: specify the method to use during defuzzification. Options provided are Mean of Maxima and Center of Area (described beforehand).
*	Initial Pressure: Specifies the initial pressure the controller should start with.
*	Max No. Iterations: Changes the amount of times the fuzzy logic controller performs an iteration.

![Simulation](/img/Simulation.png)

**Rules**

The rules used in the controller can be changed and edited when needed using the Rules Editor Tab. Doing so allows 
observations about how changes in the rules of the controller may change its results and performance.
Rules may be created, deleted or edited from the Rules Editor tab. A preview of the selected rule is also shown by 
drawing the membership functions that represent the rule. Rules may also be enabled or disabled without being deleted 
by unchecking or checking the appropriate checkbox next to its name.

![Rules Editor](/img/RulesEditor.png)

**Viewing Simulation History**

Due to the nature of this assignment, it was important that the values produced by the controller over time could be 
viewed at the end of the simulation. In order to satisfy such a requirement, the fuzzy logic controller was made to 
store its output value, output graph and output rule at each iteration during the controller’s runtime.
After a simulation is run, the simulation history may be viewed in the “Simulation History” tab of the application 
where one may choose the iteration to view. The application will show the output graph which is obtained through fuzzy 
inferencing, the output value calculated by defuzification and the rule that fired at the iteration.

![Simulation History](/img/SimulationHistory.png)





