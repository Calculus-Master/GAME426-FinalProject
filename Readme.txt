Group Members: Sap Mallick, Grace Peltan, Levi Taylor

Project Name: Pet Shop Simulator

Key Controls:
- No key binds
- To add pets, left click on the "Add Pet" button in the menu. This will spawn a pet in that will then follow AI behavior.
- Certain objects within the scene can be disabled or toggled to influence pet AI behavior. These objects are placed to the left of the main room, and you can click on them to toggle them off or on.
- Clicking a pet will bring up a panel that displays a ton of info about the pet, including current state, needs, affinities and type.

AI Algorithms:
- Finite State Machines: A FSM is the top-level controller for each pet's behavior. The FSM is hierarchical: there are 2 super states, Leisure and Needs, and each of the states includes a few substates controlling behavior. Transitions between states are determined by the pets' needs and influence maps.
- Behavior Trees: Many of the FSM states have actions that are controlled entirely by a behavior tree. This behavior tree dynamically directs pets to the proper target location (food bowl, water bowl, sleeping area, other pet, toy, etc) based on the pet's affinity to that location or pet, as well as the availability of it (if the target is occupied, the pet will dynamically wait until it is freed). 
- Influence Maps: We have a layered influence map system that overlays the floor of the pet shop. In particular, we have three key influence maps: playfulness, which is exerted by the static toys around the area (the higher the influence value of a spot based on nearby toys, the more likely the pet is to start playing with one); decor, which is exerted by static decoration items around the room, and influences how slowly the pet's energy level needs decay; and lastly, positioning, which is a mobile influence map that is exerted by the pets, and updates as they move around the room.

Assets Used:
- Some code was repurposed from Activity 5 and HW2.