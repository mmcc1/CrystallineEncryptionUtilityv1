# Crystalline Encryption Utility v1.0

A basic implementation of the Crystalline Cipher employing the Lattice approach.  Lattice makes use of n number of Keys/Salts to encrypt a file.  Typically, each Key/Salt is used with 10 rounds (11 due to the code) then Key/Salts are switched and the process repeated.  In all, the basic settings use 110 rounds.

It is advisable to always go with odd settings for the rounds and file size of the Key/Salts, the more unique the better.

More instructions in the help file included.

Win10 security can block the files.  After downloading the zip archive, right-click and tick the 'Unblock' box.

2D Analysis (no compression):

<p align="center">
  <img src="https://github.com/mmcc1/CrystallineEncryptionUtilityv1/blob/master/K1nNcoM.png" title="2D Analysis">
</p>

3D Analysis  (no compression):

<p align="center">
  <img src="https://github.com/mmcc1/CrystallineEncryptionUtilityv1/blob/master/3DAnalysis.png" title="3D Analysis">
</p>
https://github.com/mmcc1/CrystallineEncryptionUtilityv1/blob/master/3DAnalysis.png

R Code

3D Analysis

wh <- c(464, 464) #Change to the nearest square of file size
v <- readBin("0.cle", what = "integer", n = prod(wh), size = 1, signed = FALSE, endian = "little")

x <- 1:464 
y <- 1:464
z <- v[1:215296] #Change to whatever the square of wh[1] is.

open3d()
rgl.surface(x, y, z, col="skyblue")


2D Analysis

wh <- c(464, 464) #Change to the nearest square of file size
v <- readBin("0.cle", what = "integer", n = prod(wh), size = 1, signed = FALSE, endian = "little")

tiff("output.tif", width=wh[1], height=wh[2])
par(c(0,0,0,0))
image(matrix(v, wh[1], wh[2])[wh[1]:1,], useRaster = TRUE, col = grey.colors(256))
dev.off()
