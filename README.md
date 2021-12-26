# Gravity-Marble

This project was made with Unity 3D and programmed in C#.

## Gameplay

The goal of this game is to role a marble from where you start to the finish platform at the end of a randomly generated track. As you role over slants and curves, gravity will change so that you always roll flat across the platforms you are over.

'WASD' to roll.
'R' to respawn.
'G' to generate a new track.

You can play the game at the following link: https://etonio99.itch.io/gravity-marble

This project got as far as I had intended for it to get. There are known issues with track generation and if you run into any issues with a track that can not be completed, generate a new one by pressing 'G'.

<img src='https://user-images.githubusercontent.com/65688007/147421012-5131d4b2-c36e-4d5b-8d5c-c4b0cdee21ae.png' width=32% height=32%> <img src='https://user-images.githubusercontent.com/65688007/147421030-0a404b1a-3a37-45b7-9370-147cea0212b7.png' width=32% height=32%> <img src='https://user-images.githubusercontent.com/65688007/147421032-addeaeb0-a4a7-447e-ac5e-efa6d6a3e841.png' width=32% height=32%>

## Code

The marble is controlled from MarbleScript.cs. It will add torque to the marble based off of the current player input, stored in a variable called 'leftStickInput'. In the same script it will look for the closest track piece (individual parts that together make up the track) and check for what direction gravity should be set to for the marble, based off of the track pieces' normals.. Each track object - in the code they are referred to as 'planets' - can either use their normals to assign the direction of gravity or they can have a forced direction for gravity that is manually set. This makes it possible to use track pieces like stairs, where gravity should only remain in one direction. GravitySourceScript.cs is attached to each track piece and allows for these settings.

LevelGeneratorManagerScript.cs is in charge of generating the track that the player will traverse. This script will put together track pieces based off of a random seed. The same piece is not allowed to be used twice in a row.

The camera is controlled with SmoothFollow3D.cs, which allows the camera to watch and follow the ball at the same angle, regardless of which direction gravity is.
