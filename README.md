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

[img/Design.png]



