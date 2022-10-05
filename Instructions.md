# JumpForRevit
In the "example" folder there is a Revit 2019 file which is an example of the mentioned problem. In order to use the addin, the command "cmdZapatas" must be executed.

![image](https://user-images.githubusercontent.com/63598902/193935340-20f374c6-d958-4e19-94a6-60270a12420f.png)

Then the graphic window will appear where the options are already marked to reproduce the error. Just click the "Ejecutar" button.

![image](https://user-images.githubusercontent.com/63598902/193935742-df3592e5-345a-4b1c-9253-9e32d4d1adb6.png)

A section view will be created with the structural element, the texts and the lines, it is recommended to delete the texts.

![image](https://user-images.githubusercontent.com/63598902/193936187-ff19dac9-0585-45c0-b303-f010ba0f7343.png)

Finally, the problem discussed in the Stackoverflow forum is exposed (https://stackoverflow.com/questions/73942748/revit-api-curves-obtained-from-the-edge?noredirect=1#comment130562807_73942748).

![image](https://user-images.githubusercontent.com/63598902/193936255-3fd6697c-f18f-4e3c-b828-4b76ff7663bc.png)

The line of code where you create the detail curves is https://github.com/kevinzurro/JumpForRevit/blob/5896b1385d86292117465b6bee00a3d1ebd70874/Jump/Comandos/Tools.cs#L1430
