# my-gps-ar
This is my attempt at combining data from GPS and magnetometer in order to create an immersive experience of location based augmented reality.
Given the low cost sensors that phone is using i had to come up with a way to improve accuracy.
My algorithm collects magnometer and GPS sensor measurments, and utilizing the AR coordinate system, in order to convert those measurements into the real world respective values. 
The advantage here is that no measurement is lost when the user is moving his phone in space, and the change in position or orientation doesn't cause measurements to be obsolete. The measurements are constantly converted between real world and AR world so that they all contribute to the most precise solution the phone is capable of.
The (potentially) endless measurements used in the calculation, effectivly creates a seemless smooth optical illusion of a real objects placed in real world space.
Here's a youtube video that demonstrates the app in action  
https://youtu.be/9BA4Urlic0A

Textual indication shows the word "SQUARE" on the geographic coordinate of the actual square
![image](https://user-images.githubusercontent.com/89970476/226104351-a98f6b49-20a8-4281-b639-ec4b8bdfa6f7.png)

The store has a virtual sign located on its physical location
![image](https://user-images.githubusercontent.com/89970476/226104401-0a44f206-82bf-4c7b-a55a-5613adefb884.png)
