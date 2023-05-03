The aim of this pre-work is to understand how developping on the wall is done :

	In order to do that, the idea of this project is to have a main panel divided into 4 equal parts (cf scheme1)
	here is the first structure I'd like to implement :
		- 4 same prefab-based GameObjects representing the screens (4 Areas/Shapes)
		- 1 big screen regrouping the 4 players GameObjects
		- 1 specific GameObject representing the ControlPad (1 Area/Shape )
		- X shapes which the operator is able to manipulate 
			-> ONLY by clicking onto these shapes in his control panel
	The main idea is to allow the Drag n Drop only in the small Screen
		And link the Big/Small Shapes with the code, then with predicate whenever small is moved -> translating new cordinates to big.
		Constantly calculating who's the owner of the shape in the BigScreen (P1-P4)

Code architecture :
	Scene.cs       --> contains the 3 game objects 'ControlPane' | 'Screen' & 'Shapes' in order to coordinate all of the program 
	ControlPane.cs --> contains a test method to know if the mouse is inside the control pane && is on a shape
	Screen.cs      --> contains all 4 players representing the 4 part of the main screen

	Operator.cs    --> represent the photon entity of the user (masterclient unlike the other ones)
	Player.cs      --> represent the photon entity of a player (screen) to allow the program to switch this

	ShapeCtrl.cs   --> only contains methods related to switching positions and indicating if it's being moved or not
	ShapeScreen.cs --> only allows access to screen's shape coordinates ? 
