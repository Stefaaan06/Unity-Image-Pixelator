# Unity Image Pixelator

Unity Image Pixelator is a  simple tool that allows you to Pixelate & de-Pixelate images at Runtime:
# Features
    Pixelate Images
    de-Pixelate Images
    Control prefered pixel rows and columns
    Control speed
    Full documentation

# How it works

This script pixelates the texture of an image at runtime, meaning that there won´t be any changes to the image. It´s worth noting that having different pixelation sizes for the same texture is not possible as it modifies the texture directly. 
The script has only very little runtime overhead, but it might grow significantly when trying to pixelate large textures with a lot of pixels. If that´s the case its worth looking into multithreading the code.

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
