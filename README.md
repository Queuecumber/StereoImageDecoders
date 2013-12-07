StereoImageDecoders
===================

Tools to decode stereo/multi-picture images into jpeg files


MPO
---

MPO files can store any number of jpegs. The container has no extra information so JPEG files can be extracted by looking for their start tag: FFD8FFE1 which represents the first EXIF tag (even if the exif section is empty, the jpeg will still have this tag).

### Pros
  * Can hold any number of images
  * Images don't need to be decoded for extraction
    
### Cons
  * Opaque format requires a special viewer
    
JPS
---

JPS files are just a jpeg file that holds the left and right channel images side-by-side. This means they can be viewed and extracted using any image program capable of working with JPEGs. To decode into each channel, split the image in half and write each half to a file.

### Pros
  * Can be viewed using any JPEG capable viewer
    
### Cons
  * Can only hold two images
  * Images must be decoded for extraction
    
TODO
----
  * Decode MPO files in one pass
  * Investigate decoding JPS files without first decompressing the jpeg format
