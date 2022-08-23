# PowerQR
PowerQR is an image data storage system, and can encode an unlimited amount of bytes in an image, *typically* compressing it in the process as an image.

# Features
* Generate with one function.
```cs
Text.Generate(Bytes, NumericalValues)
QR.Generate(Bytes);
```

* Read with one function.
```cs
Text.Read(Text);
QR.Read(Image);
```

## Here's the entire bee movie script encoded in an image.
![Bee Movie Script](https://user-images.githubusercontent.com/85254326/186011130-dee93bce-7566-490a-949d-794500084336.png)

### And here's the beginning of it encoded as numerical text.
```
65 99 99 111 114 100 105 110 103 32 116 111 32 97 108 108 32 107 110 111 119 110 32 108 97 119 115 32 111 102 32 97 118 105 97 116 105 111 110 44 13 10 13 10 116 104 101 114 101 32 105 115 32 110 111 32 119 97 121 32 97 32 98 101 101 32 115 104 111 117 108 100 32 98 101 32 97 98 108 101 32 116 111 32 102 108 121 46 13 10 13 10 73 116 115 32 119 105 110 103 115 32 97 114 101 32 116 111 111 32 115 109 97 108 108 32 116 111 32 103 101 116 32 105 116 115 32 102 97 116 32 108 105 116 116 108 101 32 98 111 100 121 32 111 102 102 32 116 104 101 32 103 114 111 117 110 100 46 13 10 13 10 84 104 101 32 98 101 101 44 32 111 102 32 99 111 117 114 115 101 44 32 102 108 105 101 115 32 97 110 121 119 97 121 13 10 13 10 98 101 99 97 117 115 101 32 98 101 101 115 32 100 111 110 226 128 153 116 32 99 97 114 101 32 119 104 97 116 32 104 117 109 97 110 115 32 116 104 105 110 107 32 105 115 32 105 109 112 111 115 115 105 98 108 101 46

you get the point.. the file gets a bit too big for a readme
```