# Unity Image Pixelator

Unity Image Pixelator is a  simple tool that allows you to Pixelate & de-Pixelate images at Runtime:
# Features
    Pixelate Images
    de-Pixelate Images
    Control prefered pixel rows and columns
    Control speed
    Full documentation

# How it works

The script generates a duplicate texture of the image texture and modifies that one. Meaning that you can modify multiple images differently and simultaniously. 

This might be a problem with larger image resolutions (2k, 4k) as it will take up more RAM the more images you add. Even if the images are the same.
-> if that is the case, consider removing the code from the Start method that duplicates the image so it modifies the texture directly. Be aware that this change will not allow you to modify the same images
uniquely meaning that modifying one image changes all the images with the same texture. 

# Installation
    
To use the Unity Image Pixelator in your Unity project, follow these steps:

1.
    Open the unity package manager
    Add a new package using the projects url: https://github.com/Stefaaan06/Unity-Editor-Physics-Simulation.git
2.
    Copy the ImagePixelator.cs into your project's Assets directory.

# Usage
    Add the "ImagePixelator.cs" script to a gameobject with a Image component.
    Adjust the parameters as needed. Higher values for rows and columns leads to more pixels being pixelated.
    
    
    Call the "PixelateImage" and "RePixelateImage" functions as needed.

# Warning
For the code to work, you must set the Sprite Mode to "Allow Read/Write" in the inspector, as well as set the Format RGBA 32 bit.

# Creator
[Stefaaan06](https://twitter.com/Stefaaan06) - Creator and maintainer

[Wishlist my game on steam](https://store.steampowered.com/app/2547010/Mik/)

Credit me at will.
