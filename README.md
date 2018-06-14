Initial load takes bit time because it's restoring all the packages.

The "game" saves it's state only for the lifetime of the application in IIS, if you rebuild and restart the application it will start from beginning.
I didn't use any database to save the state as it's not required and did't want to overcomplicate the application.

There are three projects, as the whole logic for the slot game is in "Bede.SlotMachine.Engine"
The other two projects use the "ISlotEngine" interface to comunicate with the game engine.
Project "Bede.SlotMachine" is a simple test and I used it before implementing the web interface.

After the game engine was ready I focused on "Bede.SlotMachine.WebUI" project which uses Vue.js to render the page.
The project contains two Vue components, one for the deposit modal and one for the slot machine itself. 
The components are located at "wwwroot/components"
